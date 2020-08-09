using System.Collections;
using System.Collections.Generic;
using System.Text;
using Tool.Database;
using UnityEngine;

public class GameSoundManager : UnitySingleton<GameSoundManager>
{
    [Tooltip("IS")]
    AudioSource _backMusicSource = null;
    AudioSource _SoundSource = null;
    Dictionary<string, AudioClip> mAudioClipDict = new Dictionary<string, AudioClip>();
    Dictionary<string, float> mAudioVolum = new Dictionary<string, float>();
    protected override void Awake()
    {
        base.Awake();
        _backMusicSource = gameObject.AddComponent<AudioSource>();
        _backMusicSource.loop = true;
        _SoundSource = gameObject.AddComponent<AudioSource>();
        AddAudioClipResource();
    }
    private void AddAudioClipResource()
    {
        var list = ConfigDataManager.Instance.GetDatabase<SoundConfigDatabase>().FindAll();
        foreach (var item in list)
        {
            if (!mAudioClipDict.ContainsKey(item.name))
            {
                string audioPath = item.path;
                AudioClip audioClip = Resources.Load(audioPath) as AudioClip;
                if (audioClip != null)
                {
                    mAudioClipDict.Add(item.name, audioClip);
                    mAudioVolum.Add(item.name, item.volume);
                }
            }
        }
    }  
    private AudioClip getAudioClip(string audioName)
    {
        if (mAudioClipDict.ContainsKey(audioName))
        {
            return mAudioClipDict[audioName];
        }
        return null;
    }
    private float getAudioVolume(string audioName)
    {
        if (!mAudioVolum.ContainsKey(audioName))
            return 1.0f;
        return mAudioVolum[audioName];
    }

    public void PlayBgMusic(string musicPath)
    {
        StartCoroutine(PlayBgMusicInCoroutine(musicPath));
    }
    IEnumerator PlayBgMusicInCoroutine(string musicPath)
    {
        if (!GameDataManager.Instance.playerData.mIsGameBGMOpen)
           yield break;
        var clip = getAudioClip(musicPath);
        if (clip == null)
            yield break;
        _backMusicSource.clip = clip;
        _backMusicSource.volume = getAudioVolume(musicPath);
        _backMusicSource.Play();
        yield break;
    }

    public void setBgMusic(bool isOn)
    {
        if (_backMusicSource.clip == null)
        {
            PlayBgMusic("music_main");
            return;
        }
        if (isOn)
        {
            _backMusicSource.UnPause();
        }
        else
        {
            if (_backMusicSource.isPlaying)
                _backMusicSource.Pause();
        }
    }

    public void PlaySound(string soundPath)
    {
        StartCoroutine(PlaySoundInCoroutine(soundPath));
    }
    IEnumerator PlaySoundInCoroutine(string soundPath)
    {
        if (!GameDataManager.Instance.playerData.mIsGameSoundOpen)
            yield break;
        var clip = getAudioClip(soundPath);
        if (clip == null)
            yield break;
        var volume = getAudioVolume(soundPath);
        _SoundSource.PlayOneShot(clip, volume);
        yield break;
    }
}

