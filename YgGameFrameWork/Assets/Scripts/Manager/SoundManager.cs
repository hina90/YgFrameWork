using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tool.Database;
using UnityEngine;

/// <summary>
/// 声音管理器
/// </summary>
public class SoundManager : BaseManager
{

    private AudioSource audio = null;
    private Hashtable sounds = new Hashtable();
    /// <summary>
    /// 初始化
    /// </summary>
    public override void Initialize()
    {
        audio = ManagementCenter.managerObject.GetComponent<AudioSource>();
    }


    void Add(string key, AudioClip value)
    {
        if(sounds[key] != null || value == null)
            return;

        sounds.Add(key, value);
    }

    AudioClip Get(string key)
    {
        if (sounds[key] == null)
            return null;

        return sounds[key] as AudioClip;
    }
    /// <summary>
    /// 加载声音资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="action"></param>
    void LoadAudioClip(string path, Action<AudioClip> action)
    {
        AudioClip ac = Get(path);
        if(ac == null)
        {
            var name = Path.GetFileNameWithoutExtension(path);
            resMgr.LoadAssetAsync<AudioClip>(path, new[] { name }, delegate (UnityEngine.Object[] objs)
            {
                if (objs.Length == 0 || objs[0] == null)
                    return;

                var clip = objs[0] as AudioClip;
                if(clip != null)
                {
                    Add(path, clip);
                    action(clip);
                }
            });
        }
        else
        {
            action(ac);
        }
    }

    public bool CanPlayBacksound()
    {
        string key = AppConst.AppPrefix + "BackSound";
        int i = PlayerPrefs.GetInt(key, 1);

        return i == 1;
    }
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name"></param>
    /// <param name="canPlay"></param>
    public void PlayBacksound(string name, bool canPlay)
    {
        if(audio.clip != null)
        {
            if(name.IndexOf(audio.clip.name) > -1)
            {
                if(!canPlay)
                {
                    audio.Stop();
                    audio.clip = null;
                    Utils.ClearMemory();
                }
                return;
            }
        }
        if(canPlay)
        {
            audio.loop = true;
            LoadAudioClip(name, delegate(AudioClip clip)
            {
                audio.clip = clip;
                audio.Play();
            });
        }
        else
        {
            audio.Stop();
            audio.clip = null;
            Utils.ClearMemory();
        }
    }

    public bool CanPlaySoundEffect()
    {
        string key = AppConst.AppPrefix + "SoundEffect";
        int i = PlayerPrefs.GetInt(key, 1);
        return i == 1;
    }

    void PlayInternal(AudioClip clip, Vector3 position)
    {
        if(!CanPlaySoundEffect())
            return;

        AudioSource.PlayClipAtPoint(clip, position);
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="path"></param>
    public void Play(string path)
    {
        LoadAudioClip(path, delegate (AudioClip clip)
        {
            if(clip != null)
            {
                this.PlayInternal(clip, Vector3.zero);
            }
        });
    }

    public override void OnUpdate(float deltaTime)
    {
         
    }

    public override void OnDispose()
    {
         
    }
}

