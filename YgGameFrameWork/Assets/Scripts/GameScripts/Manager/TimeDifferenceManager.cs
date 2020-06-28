using System;
using System.Net;
using System.Net.Sockets;

public class TimeDifferenceManager : UnitySingleton<TimeDifferenceManager>
{
    #region 访问服务器
    // //到“传输时间戳”字段的偏移量（以64位时间戳格式，应答离开客户端服务器的时间）
    // const byte serverReplyTime = 40;
    // // 默认NTP服务器
    // const string ntpServer = "ntp1.aliyun.com";
    // IPAddress[] addresses;
    // IPEndPoint ipEndPoint;
    #endregion
    private PlayerModule playerModule;
    private TimeSpan offlineSub;//离线时间差
    private TimeSpan timeSpan;//共用timeSpan
    private DateTime uptime;//上线时间
    private bool whetherTheSameDay;
    /// <summary>
    /// 同一天就返回true，不是同一天就返回false
    /// </summary>
    /// <value></value>
    public bool WhetherTheSameDay { get { return whetherTheSameDay; } }
    public int OfflineDays { get { return (int)offlineSub.TotalDays; } }                    //离线天数
    public int OfflineHours { get { return (int)offlineSub.TotalHours; } }                  //离线小时
    public int OfflienMinutes { get { return (int)offlineSub.TotalMinutes; } }              //离线分钟
    public int OfflineSeconds { get { return (int)offlineSub.TotalSeconds; } }              //离线秒数
    public int OnlineHours { get { return (int)ObtainTimeDifference().TotalHours; } }       //在线小时
    public int OnlineMinutes { get { return (int)ObtainTimeDifference().TotalMinutes; } }   //在线分钟
    public void Init()
    {
        playerModule = GameModuleManager.Instance.GetModule<PlayerModule>();
        TDDebug.DebugLog("上次离线时间：" + playerModule.OfflineTime.ToString("yyyy-MM-dd HH:mm:ss"));

        whetherTheSameDay = WhetherTheSameDays();
        uptime = GetWebTime();
        offlineSub = playerModule.OfflineTime.Subtract(uptime).Duration();
        TimerManager.Instance.CreateTimer("SaveTime", 0, 1, () => { playerModule.OfflineTime = GetWebTime(); });

        TDDebug.DebugLog("离线时间秒：" + offlineSub.TotalSeconds);
    }
    private TimeSpan ObtainTimeDifference()
    {
        timeSpan = uptime.Subtract(GetWebTime()).Duration();
        return timeSpan;
    }

    #region 离线收益
    //离线单位小时收益
    public int HoursProfit(int profit)
    {
        return OfflineHours * profit;
    }
    //离线单位分钟收益
    public int MinutesProfit(int profit)
    {
        return OfflienMinutes * profit;
    }
    //离线单位秒数收益
    public int SecondsProfit(int profit)
    {
        return OfflineSeconds * profit;
    }
    //离线单位半小时收益
    public int SemihProfit(int profit)
    {
        return OfflineHours * profit * 2;
    }
    //离线单位半分钟收益
    public int HalfAMinuteProfit(int profit)
    {
        return OfflienMinutes * profit * 2;
    }
    #endregion

    /// <summary>
    /// 每日任务上的倒计时
    /// </summary>
    public string ToZeroTime()
    {
        DateTime zeroTime = Convert.ToDateTime("00:00:00");
        DateTime nowTime = GetWebTime();
        timeSpan = zeroTime.Subtract(nowTime).Duration();
        return $"{24 - timeSpan.Hours}小时{60 - timeSpan.Minutes}分{60 - timeSpan.Seconds}秒";
    }
    /// <summary>
    /// 与目标时间的时间差
    /// </summary>
    /// <param name="targetTime"></param>
    /// <param name="currentTime"></param>
    /// <returns></returns>
    public TimeSpan CountDown(DateTime targetTime)
    {
        if (GetWebTime().CompareTo(targetTime) >= 0)
            return default(TimeSpan);
        //timeSpan = targetTime.Subtract(DateTime.Now);
        timeSpan = targetTime.Subtract(GetWebTime());
        return timeSpan;
    }
    /// <summary>
    /// 传一个时间长度(分钟)，以当前时间计算目标时间是多少
    /// </summary>
    public DateTime TargetTime(float minuteTime)
    {
        float addTime = minuteTime / 60f;
        return GetWebTime().AddHours(addTime);
    }
    /// <summary>
    /// 目标时间与当前时间的比，已经过了目标时间返回true
    /// </summary>
    public bool CompareTime(DateTime targetTime)
    {
        int idx = GetWebTime().CompareTo(targetTime);
        if (idx >= 0)
            return true;
        else
            return false;
    }
    /// <summary>
    /// 判断是不是同一天
    /// </summary>
    private bool WhetherTheSameDays()
    {
        DateTime oldDay = playerModule.LoadingTime;
        DateTime newDay = GetWebTime();

        if (oldDay.Year == newDay.Year && oldDay.Month == newDay.Month && oldDay.Day == newDay.Day)//判断是不是同一天登陆
            return true;
        if (oldDay.Year >= newDay.Year && oldDay.Month >= newDay.Month && oldDay.Day >= newDay.Day)//如果更改时间为之前的时间则同样的为同一天
            return true;
        playerModule.LoadingTime = GetWebTime();
        playerModule.SurvivalDays += 1;
        return false;
    }
    //获取网络时间
    public DateTime GetWebTime()
    {
        return DateTime.Now;
    }
}
