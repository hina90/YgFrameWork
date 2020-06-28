using System;
using UnityEngine.UI;

/// <summary>
/// 快速上货面板
/// </summary>
public class UI_DeliveryAds : UIBase
{
    private Button btnBack;
    private Button btnWatch;
    private Button m_BtnMask;
    private static Action<string, string, float> suceessCallBack;
    private static Action<string, string> failedCallback;
    private static Action<string> closeCallback;

    public override void Init()
    {
        base.Init();
        Layer = LayerMenue.TIPS;
        btnBack = Find<Button>(gameObject, "BtnBack");
        btnWatch = Find<Button>(gameObject, "BtnWatch");
        m_BtnMask = Find<Button>(gameObject, "Mask");
        RegisterBtnEvent();
        PlayAnimation(Find(gameObject, "msgPanel"));
    }

    protected override void Enter()
    {
        base.Enter();
    }

    private void RegisterBtnEvent()
    {
        m_BtnMask.onClick.AddListener(() =>
        {
            UIManager.Instance.CloseUI(Layer);
        });

        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.CloseUI(Layer);
        });

        btnWatch.onClick.AddListener(() =>
        {
            SDKManager.Instance.ShowBasedVideo(suceessCallBack, failedCallback, closeCallback);
        });
    }

    public static void SetCallBack(Action<string, string, float> calllback, Action<string, string> failedCallback, Action<string> closeCallback)
    {
        suceessCallBack = calllback;
        UI_DeliveryAds.failedCallback = failedCallback;
        UI_DeliveryAds.closeCallback = closeCallback;
    }
}
