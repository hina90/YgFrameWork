using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System;

public class BaseEffect : SpineActor
{
    private EffectVO effectInfo;            //特效信息
    public bool isActive = false;           //是否活动状态
    public bool isPause = false;            //是否暂停

    private Renderer grd;
    private TrailRenderer tr;
    private SkeletonAnimation ani;
    private float tr_time = 0;

    private List<Renderer> rlst = new List<Renderer>();              //render数组
    private List<Animation> alst = new List<Animation>();            //动画数组 
    private List<ParticleSystem> clst = new List<ParticleSystem>();  //粒子系统数组

    public EffectVO EffectInfo { get; set; }
    public SkeletonAnimation Ani { get; set; }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Awake()
    {
        base.Init();
        grd = gameObject.GetComponent<Renderer>();
        //ani = gameObject.GetComponent<SkeletonAnimation>();
        tr = gameObject.GetComponentInChildren<TrailRenderer>();
        if (tr != null)
        {
            if (tr_time == 0) tr_time = tr.time;
            else tr.time = -tr_time;
        }
        rlst.Clear();
        clst.Clear();
        alst.Clear();
        GetAllEffect(transform);
    }


    /// <summary>
    /// 获取特效的所有组件
    /// </summary>
    /// <param name="tf"></param>
    private Renderer tmp_r;
    private ParticleSystem tmp_c;
    private Animation tmp_a;
    private void GetAllEffect(Transform tf)
    {
        tmp_r = gameObject.GetComponent<Renderer>();
        if (tmp_r != null) rlst.Add(tmp_r);
        tmp_c = gameObject.GetComponent<ParticleSystem>();
        if (tmp_c != null) clst.Add(tmp_c);
        tmp_a = gameObject.GetComponent<Animation>();
        if (tmp_a != null) alst.Add(tmp_a);

        for (int i = 0; i < tf.childCount; i++)
        {
            GetAllEffect(tf.GetChild(i));
        }
    }
    /// <summary>
    /// 播放特效
    /// </summary>
    public virtual void Play()
    {
        gameObject.transform.position = EffectInfo.StartVector;

        isActive = true;

        // 生命周期开始计时
        if (EffectInfo.LifeTime > 0)
        {
            StartCoroutine(LifeTimeEnd(EffectInfo.LifeTime));
        }

        // render开启
        for (int i = 0; i < rlst.Count; i++)
        {
            rlst[i].enabled = true;
        }

        // 自身绘制
        if (grd != null) grd.enabled = true;

        // 粒子开启
        for (int i = 0; i < clst.Count; i++)
        {
            clst[i].Play(true);
        }

        // 开启动画
        for (int i = 0; i < alst.Count; i++)
        {
            alst[i].Play();
        }
        if (GetSkeletonAnimation() != null)
        {
            GetSkeletonAnimation().AnimationState.SetAnimation(0, EffectInfo.AniName, EffectInfo.Loop);
        }

        Invoke("ResetTrail", 0.05f);
    }
    private void ResetTrail()
    {
        // 带子的时间取反
        if (tr != null)
        {
            tr.time = Math.Abs(tr.time);
            //tr.time = tr_time;
        }
    }

    IEnumerator LifeTimeEnd(float lifeTime)
    {

        float startTime = Time.time;

        while (Time.time - startTime <= lifeTime || isPause)
        {
            yield return null;
        }

        EffectEnd(false);
    }

    /// <summary>
    /// 销毁特效
    /// </summary>
    public void Destroy()
    {
        // 关闭特效
        Stop();
        EffectManager.Instance.RemoveEffect(gameObject.transform.parent.gameObject);
    }
    /// <summary>
    /// 停止特效
    /// </summary>
    public void Stop()
    {
        isActive = false;
        // 停止粒子
        for (int i = 0; i < clst.Count; i++)
        {
            clst[i].Stop(true);
        }

        // 停止绘制
        Renderer re;
        for (int i = 0; i < rlst.Count; i++)
        {
            re = rlst[i];
            // 粒子继续绘制
            if (re.gameObject.GetComponent<ParticleSystem>() != null)
                continue;

            if (re.transform != null && re.transform.parent != null)
                re.enabled = false;
        }

        // 自身不绘制
        if (grd != null) grd.enabled = false;

        // 停止动画
        for (int i = 0; i < alst.Count; i++)
        {
            alst[i].Stop();
        }

        // 带子的时间取反
        if (tr != null)
        {
            tr.time = -Math.Abs(tr.time);
        }
    }
    /// <summary>
    /// 特效达到结束条件
    /// </summary>
    /// <param name="isArrive"></param>
    public void EffectEnd(bool isArrive = true)
    {
        isActive = false;
        // 特效结束，回调
        if (EffectInfo.ArriveCallback != null)
        {
            EffectInfo.ArriveCallback(isArrive);
        }

        Destroy();
    }
    /// <summary>
    /// 是否暂停特效
    /// </summary>
    /// <param name="pause"></param>
    public void Pause(bool pause)
    {
        isPause = pause;

        // 停止粒子
        for (int i = 0; i < clst.Count; i++)
        {
            if (pause)
            {
                clst[i].Pause();
            }
            else
            {
                clst[i].Play();
            }
        }

        // 停止动画
        for (int i = 0; i < alst.Count; i++)
        {
            if (pause)
            {
                alst[i].Stop();
            }
            else
            {
                alst[i].Play();
            }

        }

        if (tr != null)
        {
            if (pause)
            {
                tr.time = 9999999;
            }
            else
            {
                tr.time = tr_time;
            }
        }

    }

}
