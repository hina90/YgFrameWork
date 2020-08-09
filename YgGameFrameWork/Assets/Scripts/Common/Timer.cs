
using UnityEngine;
//计时器
public class Timer
{
    public delegate void TimerHandler();
    public delegate void TimerArgsHandler(System.Object[] args);

    public TimerHandler Handler;         //无参回调
    public TimerArgsHandler ArgsHandler; //带参回调
    public bool IsComplete = true;       //是否完成
    public float DelayTime;              //时间延迟
    public float Frequency;              //时间间隔
    public System.Object[] Args;         //参数
    public float LastTickTime;
    public float CurrentTime = 0;        //当前时间
    public string TimerName;             //计时器标示

    public Timer()
    {

    }
    /// <summary>
    /// 创建一个时间事件对象
    /// </summary>
    /// <param name="Handler">回调函数</param>
    /// <param name="ArgsHandler">带参数的回调函数</param>
    /// <param name="frequency">时间内执行</param>
    /// <param name="repeats">重复次数</param>
    /// <param name="Args">参数  可以任意的传不定数量，类型的参数</param>
    public Timer(string name, TimerHandler Handler, TimerArgsHandler ArgsHandler, float delayTime, float frequenTime, System.Object[] Args)
    {
        this.TimerName = name;
        this.Handler = Handler;
        this.ArgsHandler = ArgsHandler;
        this.DelayTime = delayTime;
        this.Frequency = frequenTime;
        this.Args = Args;
        this.LastTickTime = Time.time;
    }
    ///执行函数
    public void Notify()
    {
        if (Handler != null)
            Handler();
        if (ArgsHandler != null)
            ArgsHandler(Args);
    }
    //清楚timer数据
    public void CleanUp()
    {
        TimerName = "";
        Handler = null;
        ArgsHandler = null;
        IsComplete = true;
        DelayTime = 0;
        Frequency = 0;
        CurrentTime = 0;
    }
}
