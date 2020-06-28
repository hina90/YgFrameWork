using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EventId
{
    CC_PlayButton,// 登陆界面play按钮进入游戏	
    CC_Propaganda,// 点击主界面广告宣传按钮
    CC_PropagandaYes,// 点击主界面广告宣传按钮后点击观看广告按钮
    CC_Buy,// 点击购买按钮(all)
    CC_CashierNo,// 点击收银台后弹出界面直接领取按钮
    CC_CashierYes// 点击收银台后弹出界面鱼干*2按钮
}
/// <summary>
/// SDK管理类 广告，友盟统计
/// </summary>
public class SDKManager : UnitySingleton<SDKManager>
{

    private Action<string, string, float> suceessCallBack;
    private Action<string, string> failedCallback;
    private Action<string> closeCallback;
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {

    }
    private void Start()
    {
        //MopubAdsController.instance.MOnRewardedVideoClosedEvent += delegate(string str1)
        //{
        //    Debug.Log("------------关闭广告回调----------");
        //    closeCallback(str1);
        //};
        //MopubAdsController.instance.MOnRewardedVideoReceivedRewardEvent += delegate(string str1, string str2, float f1)
        //{
        //    Debug.Log("------------看完广告奖励回调----------");
        //    suceessCallBack(str1, str2, f1);
        //}; 
        //MopubAdsController.instance.MOnRewardedVideoFailedEvent += delegate(string str1, string str2)
        //{
        //    Debug.Log("------------观看广告失败回调----------");
        //    failedCallback(str1, str2);
        //};
    }

    /// <summary>
    /// 激励视频是否加载完成
    /// </summary>
    //public bool IsRewardVideoLoad()
    //{
        //Debug.Log("=================>激励视频是否加载完成<=================  " + MopubAdsController.instance.IsVideoReady().ToString());
        //return MopubAdsController.instance.IsVideoReady();

    //}

    /// <summary>
    /// 加载video广告
    /// </summary>
    public void LoadRewardVideo()
    {
        //Debug.Log("=================>加载激励视频《=== ==============");
        //MopubAdsController.instance.LoadVideo();
    }

    /// <summary>
    /// 显示video广告
    /// </summary>
    public void ShowBasedVideo(Action<string, string, float> calllback, Action<string, string> failedCallback, Action<string> closeCallback)
    {
        Debug.Log("=================>显示激励视频<=================");

        //MopubAdsController.instance.LoadVideo();

        //this.suceessCallBack = calllback;
        //this.closeCallback = closeCallback;
        //this.failedCallback = failedCallback;

        //if (IsRewardVideoLoad())
        //{
        //    MopubAdsController.instance.ShowVideo();
        //}
        //else
        //{
        //    TipManager.Instance.ShowMsg("广告商人正在招商广告中");
        //}
    }
    /// <summary>
    /// 事件记录 eventid(事件名称), m_dicKey,m_dicV
    /// </summary>
    public void LogEvent(string eventId, string m_dicKey,string m_dicV)
    {
        
        //Dictionary<string, string> m_dicevent = new Dictionary<string, string>
        //{
        //    { m_dicKey, m_dicV }
        //};
        //Umeng.GA.Event(eventId, m_dicevent);

    }
    //eventId为当前统计的事件ID。
    //attributes为当前事件的属性和取值（键值对）。
    //value为定性变量（categorical variable）。
    //做登录，分享等时候用
    public void LogEvent(string eventId, Dictionary<string, string> lable, int value)
    {
        //Umeng.GA.Event(eventId, lable, value)   ;

    }

    /// <summary>
    /// 开始关卡
    /// </summary>
    public void StartLevel(string level)
    {
        //Umeng.GA.StartLevel(level);
    }

    /// <summary>
    /// 结束关卡 code( 0, 完成光卡 1, 关卡失败 )
    /// </summary>
    public void OutLevel(string level, int code = 0)
    {
        if (code == 0)
        {
            //Umeng.GA.FinishLevel(level);
        }
        else
        {
            //Umeng.GA.FailLevel(level);
        }
    }
}
