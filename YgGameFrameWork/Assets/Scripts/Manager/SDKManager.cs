using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum EventId
{
    GameLogo,
    GameLogin,
    GameStart_1,
    PlatformDialogue_1,
    ClickPlatform_1,
    CustomerDialogue,
    CollectDialogue,
    Collect_1,
    UpgradeDialogue,
    ClickUpgradeView,
    ClickUpgrade_1,
    ClickUpgrade_2,
    ClickUpgrade_3,
    ClickUpgrade_4,
    ClickUpgrade_5,
    CloseUpgradeView,
    DigDialog_1,
    DigEntr_1,
    CloseAwardPreview,
    DigDialog_2,
    ClickStone_1,
    ClickBombDialog,
    ClickBomb_1,
    Rewards_1,
    Upgrade_1,
    GuideEndDialog_1,
    ClickNum_CoinUfoAds,
    ClickNum_CallAds,
    ClickNum_DigAds,
    ClickNum_OfflineDoubleAds,
    ClickNum_Christmas,
    ClickNum_ChristmasX3Ads,
    ClickNum_ChristmasX4Ads,
    ClickNum_ChristmasX5Ads,
    DailyGameLogin,
    DailyGameStart,
    ClickNum_UpgradeX3,
    ClickNum_AddDiamond,
    CoinUfoAdsSucceed,//金币ufo广告回调成功时					
    CoinUfoAdsClose,//金币ufo广告点击关闭时					
    CoinUfoAdsDefeat,//金币ufo广告点击加载失败时
    CallAdsSucceed,//电话亭广告回调成功时
    CallAdsClose,//电话亭广告点击关闭时
    CallAdsDefeat,//电话亭广告加载失败时					
    DigAdsSucceed,//挖掘广告回调成功时
    DigAdsClose,//挖掘广告点击关闭时
    DigAdsDefeat,//挖掘广告加载失败时
    OfflineDoubleAdsSucceed,//离线收益x2广告回调成功时
    OfflineDoubleAdsClose,//离线收益x2广告点击关闭时
    OfflineDoubleAdsDefeat,//离线收益x2广告加载失败时
    Christmasx3AdsSucceed,//圣诞老人钻石*3广告回调成功时
    Christmasx3AdsClose,//圣诞老人钻石*3广告点击关闭时
    Christmasx3AdsDefeat,//圣诞老人钻石*3广告加载失败时
    Christmasx4AdsSucceed,//圣诞老人钻石*4广告回调成功时
    Christmasx4AdsClose,//圣诞老人钻石*4广告点击关闭时
    Christmasx4AdsDefeat,//圣诞老人钻石*4广告加载失败时
    Christmasx5AdsSucceed,//圣诞老人钻石*5广告回调成功时
    Christmasx5AdsClose,//圣诞老人钻石*5广告点击关闭时
    Christmasx5AdsDefeat,//圣诞老人钻石*5广告加载失败时
    UpgradeX3AdsSucceed,//展馆升级奖励*3广告回调成功时
    UpgradeX3AdsClose,//展馆升级奖励*3广告点击关闭时
    UpgradeX3AdsDefeat,//展馆升级奖励*3广告加载失败时
    AddDiamondAdsSucceed,//钻石红包广告回调成功时
    AddDiamondAdsClose,//钻石红包广告点击关闭时
    AddDiamondAdsDefeat,//钻石红包广告加载失败时
    Unlock_stand1,//玩家解锁1号展台时
    Unlock_stand2,//玩家解锁2号展台时
    Unlock_stand3,//玩家解锁3号展台时
    Unlock_stand4,//玩家解锁4号展台时
    Unlock_stand5,//玩家解锁5号展台时
    Unlock_stand6,//玩家解锁6号展台时
    StarClass_2,//玩家展馆升至2级时
    StarClass_3,//玩家展馆升至3级时
    StarClass_4,//玩家展馆升至4级时
    StarClass_5,//玩家展馆升至5级时
    StarClass_6,//玩家展馆升至6级时
    StarClass_7,//玩家展馆升至7级时
    StarClass_8,//玩家展馆升至8级时
    StarClass_9,//玩家展馆升至9级时
    StarClass_10,//玩家展馆升至10级时
    StarClass_11,//玩家展馆升至11级时
    StarClass_12,//玩家展馆升至12级时
    StarClass_13,//玩家展馆升至13级时
    StarClass_14,//玩家展馆升至14级时
    StarClass_15,//玩家展馆升至15级时
    StarClass_16,//玩家展馆升至16级时
    StarClass_17,//玩家展馆升至17级时
    StarClass_18,//玩家展馆升至18级时
    StarClass_19,//玩家展馆升至19级时
    StarClass_20,//玩家展馆升至20级时
}
/// <summary>
/// SDK管理类 广告，友盟统计
/// </summary>
public class SDKManager : UnitySingleton<SDKManager>
{
    /// <summary>
    /// 加载广告成功回调
    /// </summary>
    private Callback<string> LoadSuceessCallBack;
    /// <summary>
    /// 加载广告失败
    /// </summary>
    private Callback<string, string> LoadFailedCallback;
    /// <summary>
    /// 发放奖励回调
    /// </summary>
    private Callback<string, string, float> RewardCallback;
    /// <summary>
    /// 关闭广告回调
    /// </summary>
    private Callback<string> ClosedAdCallBack;
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        //初始化SDK
        //PottingMobile.InitializeSdk(PottingMobileContent.AdJsonString,
        //                        PottingMobileContent.FacebookAppid,
        //                        PottingMobileContent.AppsFlyerAppid,
        //                        PottingMobileContent.UmengAppid);

        //PottingMobileManger.Instance.MOnRewardedVideoLoadedEvent += delegate (string str)
        //{
        //    LoadSuceessCallBack?.Invoke(str);
        //    Debug.Log("加载成功~~~~~~~~~~~~");
        //};
        //PottingMobileManger.Instance.MOnRewardedVideoFailedEvent += delegate (string str, string str1)
        //{
        //    LoadFailedCallback?.Invoke(str, str1);
        //    Debug.Log("加载失败~~~~~~~~~~~~");
        //};
        //PottingMobileManger.Instance.MOnRewardedVideoReceivedRewardEvent += delegate (string str, string str1, float f1)
        //{
        //    RewardCallback?.Invoke(str, str1, f1);
        //    Debug.Log("获得奖励~~~~~~~~~~~~");
        //};
        //PottingMobileManger.Instance.MOnRewardedVideoClosedEvent += delegate (string str)
        //{
        //    ClosedAdCallBack?.Invoke(str);
        //    Debug.Log("关闭广告~~~~~~~~~~~~");
        //};
    }
    /// <summary>
    /// 激励视频是否加载完成
    /// </summary>
    public bool IsRewardVideoLoad
    {
        get
        {
#if UNITY_ANDROID || UNITY_IOS
            //return PottingMobile._HasRewardedVideo();
#else
            return true;
#endif
            return true;
        }
    }
    /// <summary>
    /// 加载video广告
    /// </summary>
    public void LoadRewardVideo()
    {
        //PottingMobile._LoadRewardedVideoAd();
    }

    /// <summary>
    /// 显示video广告
    /// </summary>
    /// <param name="rewardCallback">发放奖励回调</param>
    /// <param name="loadFailedCallback">广告加载失败</param>
    /// <param name="closeCallback">关闭广告后回调</param>
    public void ShowBasedVideo(Callback<string, string, float> rewardCallback, Callback<string, string> loadFailedCallback, Callback<string> closeCallback)
    {
#if UNITY_ANDROID || UNITY_IOS

        //this.RewardCallback = rewardCallback;
        //this.LoadFailedCallback = loadFailedCallback;
        //this.ClosedAdCallBack = closeCallback;

        rewardCallback.Invoke("", "", 0);

        LoadRewardVideo();
        StartCoroutine(ShowVideo());
#else
        rewardCallback("","",0f);
        return;
#endif
    }
    private IEnumerator ShowVideo()
    {
        yield return null;
        //if (IsRewardVideoLoad)
        //    PottingMobile._ShowRewardedVideoAd();
        //else
        //    TipManager.Instance.ShowADMsg();
    }
    /// <summary>
    /// 事件记录 eventid(事件名称)
    /// </summary>
    public void LogEvent(EventId eventId, string dicKey = "EventKey", string dicV = "EventValue")
    {
        Dictionary<string, string> dicEvent = new Dictionary<string, string>
        {
            { dicKey, dicV }
        };
        //PottingMobile._CustomEvent(eventId.ToString(), dicEvent);
    }
}
