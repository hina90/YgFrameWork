using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Cashier : UIBase
{
    private Button m_BtnMask;
    private Text m_TxtValue;
    private Button m_BtnGain;
    private Button m_BtnGainx2;

    private UI_Money uiMoney;
    private FacilityModule facilityModule;
    private PlayerModule playerModule;
    private static Callback<int> gainFishAction;
    private const string UI_FISH_PREFAB = "DriedFishImg";
    protected readonly Vector2 targetPos = new Vector2(-296, 585);

    public override void Init()
    {
        base.Init();
        Layer = LayerMenue.UI;
        m_BtnMask = Find<Button>(gameObject, "Mask");
        m_TxtValue = Find<Text>(gameObject, "TxtValue");
        m_BtnGain = Find<Button>(gameObject, "BtnGain");
        m_BtnGainx2 = Find<Button>(gameObject, "BtnGainx2");
        uiMoney = UIManager.Instance.GetUI<UI_Money>("UI_Money");
        facilityModule = GameModuleManager.Instance.GetModule<FacilityModule>();
        playerModule = GameModuleManager.Instance.GetModule<PlayerModule>();
        PlayAnimation(Find(gameObject, "msgPanel"));
        RegisterBtnEvent();
    }

    private void RegisterBtnEvent()
    {
        m_BtnMask.onClick.AddListener(() =>
        {
            UIManager.Instance.CloseUI(Layer);
        });

        m_BtnGain.onClick.AddListener(() =>
        {
            gainFishAction?.Invoke(1);
            UIManager.Instance.CloseUI(Layer);
            AudioManager.Instance.PlayUIAudio("button_1");
            TaskManager.Instance.UpdateAimTaskProgress(AimTaskType.ClickBtn, m_BtnGain.name);
            SDKManager.Instance.LogEvent(EventId.CC_Propaganda.ToString(), "Cashier", "Button");
        });

        m_BtnGainx2.onClick.AddListener(() =>
        {
            if (playerModule.FreeCashier == 0)
            {
                //第一次免费领取两倍小费台
                WatchVideoSuccess(default, default, default);
                playerModule.FreeCashier = 1;
                TaskManager.Instance.UpdateAimTaskProgress(AimTaskType.ClickBtn, m_BtnGainx2.name);
            }
            else
            {
                SDKManager.Instance.ShowBasedVideo(WatchVideoSuccess, WatchVideoFail, CloseVideo);
            }

            AudioManager.Instance.PlayUIAudio("button_1");
            SDKManager.Instance.LogEvent(EventId.CC_Propaganda.ToString(), "CashierWatchAd", "Button");
            UIManager.Instance.CloseUI(Layer);
        });
    }

    protected override void Enter()
    {
        base.Enter();
        m_TxtValue.text = $"+{(int)param[0]}";
    }

    public override Dictionary<GameEvent, Callback<object[]>> CtorEvent()
    {
        return base.CtorEvent();
    }

    public override void Release()
    {
        base.Release();
    }

    public static void SetGainFishActin(Callback<int> callback)
    {
        gainFishAction = callback;
    }

    /// <summary>
    /// 观看Video成功
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    /// <param name="arg3"></param>
    private void WatchVideoSuccess(string arg1, string arg2, float arg3)
    {
        //TDDebug.Log("播放激励视频完成，获得双倍奖励!");
        gainFishAction?.Invoke(2);
    }

    /// <summary>
    /// 观看Video失败
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void WatchVideoFail(string arg1, string arg2) { }

    /// <summary>
    /// 关闭Video视频广告
    /// </summary>
    /// <param name="obj"></param>
    private void CloseVideo(string obj)
    {
        //TDDebug.Log("关闭了激励视频!");
    }
}
