using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : UnitySingleton<TimerManager>
{
    private Dictionary<string, Timer> timer_list; //时间管理器
    private List<string> remove_list = new List<string>();


    protected override void Awake()
    {
        base.Awake();
        if (timer_list == null)
            timer_list = new Dictionary<string, Timer>();
    }

    /// <summary>
    /// 创建延迟计时器
    /// </summary>
    /// <param name="seconds"></param>
    /// <param name="action"></param>
    public void CreateUnityTimer(float seconds, Callback action)
    {
        StartCoroutine(Delay(seconds, action));
    }
    
    private IEnumerator Delay(float seconds, Callback action)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }

    /// <summary>
    /// 创建计时器
    /// </summary>
    /// <param name="name">timer标示名字</param>
    /// <param name="delayTime">延迟执行timer时间</param>
    /// <param name="frequenTime">间隔执行timer时间</param>
    /// <param name="callBack">timer执行的回调函数</param>
    public Timer CreateTimer(string name, float delayTime, float frequenTime, Timer.TimerHandler callBack)
    {
        return Create(name, callBack, null, delayTime, frequenTime);
    }
    ///创建带参数的Timer
    public Timer CreateTimer(string name, float delayTime, float frequenTime, Timer.TimerArgsHandler callBack, params System.Object[] args)
    {
        return Create(name, null, callBack, delayTime, frequenTime, args);
    }
    private Timer Create(string name, Timer.TimerHandler callBack, Timer.TimerArgsHandler callBackArgs, float delayTime, float frequenTime, params System.Object[] args)
    {
        if (timer_list.ContainsKey(name))
        {
            TDDebug.DebugLogWarning("创建的timer名字重复，需要换个名字～～～");
            return null;
        }
        Timer timer = new Timer(name, callBack, callBackArgs, delayTime, frequenTime, args);
        timer_list.Add(name, timer);
        return timer;
    }
    /// <summary>
    /// 添加要销毁的timer
    /// </summary>

    public void AddToRemove(string removeName)
    {
        remove_list.Add(removeName);
    }
    /// <summary>
    /// 销毁timer
    /// </summary>
    private void DestroyTimer(string timerName)
    {
        Timer timer;
        timer_list.TryGetValue(timerName, out timer);
        if (timer != null)
        {
            timer_list.Remove(timerName);
            timer.CleanUp();
            timer = null;
            TDDebug.DebugLog("---------------------  timer destoryed, timer numbers:" + timer_list.Count);
        }
    }

    public void ReleaseTimerManager()
    {
        foreach (string timerName in timer_list.Keys)
        {
            remove_list.Add(timerName);
        }
    }
    /// <summary>
    /// 固定更新timer事件
    /// </summary>
    void Update()
    {
        if (timer_list.Count != 0)
        {
            foreach (Timer timer in timer_list.Values)
            {
                timer.CurrentTime += Time.deltaTime;
                if (timer.Frequency > 0)
                {
                    if (timer.DelayTime > 0) 
                    {
                        if (timer.CurrentTime >= timer.DelayTime)
                        {
                            timer.Notify();
                            timer.CurrentTime = 0;
                            timer.DelayTime = 0;
                            continue;
                        }
                    }
                    else
                    {
                        if (timer.CurrentTime >= timer.Frequency)
                        {
                            timer.Notify();
                            timer.CurrentTime = 0;
                            continue;
                        }
                    }
                }
                else
                {
                    if (timer.CurrentTime >= timer.DelayTime)
                    {
                        timer.Notify();
                        timer.CurrentTime = 0;
                        remove_list.Add(timer.TimerName);
                    }
                }
            }
        }
    }

    void LateUpdate()
    {
        for (int i = 0; i < remove_list.Count; i++)
        {
            DestroyTimer(remove_list[i]);
            remove_list.RemoveAt(i);
        }
    }

    /// <summary>
    /// 开始计算游戏开始时间
    /// </summary>
    private float spendTime = 0;
    private int currentMinute = 0;
    private int lastMinute = 0;
    private int gameTotalTime = 0;

    /// <summary>
    /// 开始游戏计时
    /// </summary>
    public void StartGameTime()
    {
        string gameTime = ConfigManager.Instance.GetValue("GameTotalTime");
        if(gameTime != null)
            gameTotalTime = int.Parse(gameTime);
    }
    /// <summary>
    /// 更新游戏时间
    /// </summary>
    public void UpdateGameTime()
    {
        spendTime = Time.realtimeSinceStartup;
        currentMinute = (int)spendTime / 60;
        if(currentMinute != lastMinute)
        {
            gameTotalTime++;
            lastMinute = currentMinute;
            ConfigManager.Instance.AddValue("GameTotalTime", gameTotalTime.ToString());
            ConfigManager.Instance.Save();
        }
    }
}

