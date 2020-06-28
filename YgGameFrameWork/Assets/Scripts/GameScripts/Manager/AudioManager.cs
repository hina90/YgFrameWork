using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 声音播放管理器
/// </summary>
public class AudioManager : UnitySingleton<AudioManager>
{
    //最大缓存数
    const int MAX_NUMBER = 10;
    //背景声音
    float bgmVolume = 1.0f;
    //音效声音
    float audioVolume = 1.0f;
    //音效数组
    Dictionary<string, List<AudioItem>> audioDictionary = new Dictionary<string, List<AudioItem>>();
    //音效缓存池
    List<AudioItem> audioPool = new List<AudioItem>();
    private PlayerModule playerModule;

    public void Init()
    {
        playerModule = GameModuleManager.Instance.GetModule<PlayerModule>();
        AudioItem audio;
        for (int i = 0; i < MAX_NUMBER; i++)
        {
            GameObject audioGo = ResourceManager.Instance.GetResourceInstantiate("AudioItem", AudioManager.Instance.transform, ResouceType.Audio);
            audio = audioGo.GetOrAddComponent<AudioItem>();
            audio.SetActive(false);
            audioPool.Add(audio);
        }
        BGMVolume = playerModule.BGMVolume;
        AudioVolum = playerModule.AudioVolume;
    }
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    public void PlayBGMAudio()
    {
        CreateAudio("BGM_1", true, false, 0);
    }
    ///
    public void CloseBGMAudio()
    {
        //audioDictionary.TryGet("BGM_1");
    }
    /// <summary>
    /// 播放UI音效
    /// </summary>
    /// <param name="audioName"></param>
    public void PlayUIAudio(string audioName)
    {
        CreateAudio(audioName, false, true, 1);
    }
    /// <summary>
    /// 创建音乐
    /// </summary>
    /// <param name="audioName"></param>
    /// <param name="isAutoDestoryed"></param>
    /// <param name="type"></param>
    /// <param name="callback"></param>
    private void CreateAudio(string audioName, bool isLoop, bool autoDestory, int type, Callback callback = null)
    {
        if (type == 1 && playerModule.AudioVolume == 0)
            return;
        AudioItem audio = null;
        for (int i = 0; i < audioPool.Count; i++)
        {
            if ((audioPool[i].Audio == null || !audioPool[i].IsPlaying))
            {
                audio = audioPool[i];
                break;
            }
        }
        if (audio)
        {
            audio.SetActive(true);
            audio.Type = type;
            audio.IsLoop = isLoop;
            audio.ResName = audioName;
            audio.callback = callback;
            audio.IsAutoDestroy = autoDestory;
            audio.Play();
        }
    }
    /// <summary>
    /// 设置背景音乐音量
    /// </summary>
    public float BGMVolume
    {
        get
        {
            return bgmVolume;
        }
        set
        {
            bgmVolume = value;
            playerModule.BGMVolume = bgmVolume;
            for (int i = 0; i < audioPool.Count; i++)
            {
                if (audioPool[i].Type == 0)
                {
                    audioPool[i].Audio.volume = bgmVolume;
                    break;
                }
            }
        }
    }
    /// <summary>
    /// 设置音效音量
    /// </summary>
    public float AudioVolum
    {
        get
        {
            return audioVolume;
        }
        set
        {
            audioVolume = value;
            playerModule.AudioVolume = audioVolume;
            for (int i = 0; i < audioPool.Count; i++)
            {
                if (audioPool[i].Type == 1)
                {
                    audioPool[i].Audio.volume = audioVolume;
                }
            }
        }
    }
}

/// <summary>
/// 播放Item
/// </summary>
public class AudioItem : MonoBehaviour
{
    //是否循环
    public bool IsLoop { get; set; }
    //资源名字
    public string ResName { get; set; }
    //类型  0.背景音乐  1.音效
    public int Type { get; set; }
    //是否自动销毁
    public bool IsAutoDestroy { get; set; }
    //回调
    public Callback callback;
    //播放器
    private AudioSource audioSource = null;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public AudioSource Audio { get { return audioSource; } }

    /// <summary>
    /// 播放
    /// </summary>
    public void Play()
    {
        audioSource.clip = ResourceManager.Instance.GetAudioClip(ResName);
        audioSource.loop = IsLoop;
        audioSource.Play();
    }
    /// <summary>
    /// 停止播放
    /// </summary>
    public void Stop()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
    /// <summary>
    /// 是否正在播放
    /// </summary>
    public bool IsPlaying
    {
        get
        {
            if (audioSource == null)
                return false;
            return audioSource.isPlaying;
        }
    }

    private void Update()
    {
        if (null != audioSource)
        {
            if (!IsPlaying)
            {
                if (IsAutoDestroy)
                {
                    Release();
                }
            }
        }
    }

    /// <summary>
    /// 释放
    /// </summary>
    public void Release()
    {
        if (audioSource != null)
            audioSource.Stop();

        IsLoop = false;
        ResName = "";
        callback?.Invoke();
        callback = null;
        gameObject.SetActive(false);
    }
}
