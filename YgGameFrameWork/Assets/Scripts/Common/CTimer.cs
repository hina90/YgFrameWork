using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimerInfo
{
    public string name;
    public float expire;
    public float tick;
    public float interval;
    public object param;
    public Action<object> timerfunc;
}

public class TimeTicker
{
    public uint typeId;
    public uint frameCount;
    public uint refCount;
    public object param;
    public Action<uint, object> action;
}

public class Ticker
{
    public uint remain;
    public uint frameCount;
    public object param;
    public Action<uint, object> action;
}

/// <summary>
/// 事件管理器
/// </summary>
public class CTimer : BaseObject
{
    private static CTimer instance;

    private float interval = 0;
    private object mlock = new object();
    private List<TimeTicker> expireTickers = new List<TimeTicker>();
    private List<TimeTicker> timeTickers = new List<TimeTicker>();

    private List<TimerInfo> expireTimers = new List<TimerInfo>();
    private List<TimerInfo> timers = new List<TimerInfo>();

    private List<Ticker> delTickers = new List<Ticker>();
    static HashSet<Ticker> tickers = new HashSet<Ticker>(); 

    /// <summary>
    /// 创建计时管理器
    /// </summary>
    /// <returns></returns>
    public static CTimer Create()
    {
        if(instance == null)
        {
            instance = new CTimer();
        }
        return instance;
    }

    /// <summary>
    /// 添加计时器
    /// </summary>
    /// <param name="expires">延迟时间</param>
    /// <param name="interval">间隔时间</param>
    /// <param name="func">执行函数</param>
    /// <param name="param">参数</param>
    /// <param name="runNow">是否立即执行</param>
    /// <returns></returns>
    public TimerInfo AddTimer(float expires, float interval, Action<object> func, object param = null, bool runNow = false)
    {
        var timer = new TimerInfo();
        timer.interval = interval;
        timer.timerfunc = func;
        timer.param = param;
        timer.tick = runNow ? interval : 0;
        timers.Add(timer);

        return timer;
    }
    /// <summary>
    /// 删除计时器
    /// </summary>
    /// <param name="timer"></param>
    public void RemoveTimer(TimerInfo timer)
    {
        if(timer != null)
        {
            expireTimers.Add(timer);
        }
    }
    /// <summary>
    /// 时间计时器计时
    /// </summary>
    /// <param name="deltaTime"></param>
    void OnTimer(float deltaTime)
    {
        if (timers.Count == 0)
            return;

        foreach(var timer in timers)
        {
            if (expireTimers.Contains(timer))
                continue;

            timer.tick += deltaTime;
            if(timer.expire > 0)
            {
                if(timer.tick >= timer.expire)
                {
                    expireTimers.Add(timer);
                    if(timer.timerfunc != null)
                    {
                        timer.timerfunc.Invoke(timer.param);
                    }
                }
            }
            else
            {
                if(timer.tick >= timer.interval)
                {
                    timer.tick = 0;
                    if(timer.timerfunc != null)
                    {
                        timer.timerfunc.Invoke(timer.param);
                    }
                }
            }
        }
        lock(mlock)
        {
            foreach(var timer in expireTimers)
            {
                timers.Remove(timer);
            }
            expireTimers.Clear();
        }
    }



    public override void Initialize()
    {
        isOnUpdate = true;
    }

    public override void OnUpdate(float deltaTime)
    {
        
    }

    public override void OnDispose()
    {
        
    }
}

