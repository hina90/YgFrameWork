using System;
using UnityEngine.UI;

/// <summary>
/// 种子变异面板
/// </summary>
public class UI_SeedMutationAds : UIBase
{
    private Button btnBack;
    private Button btnWatch;
    private Button m_BtnMask;
    private static Action<string, string, float> suceessCallBack;
    private static Action<string, string> failedCallback;
    private static Action<string> closeCallback;
    private static Action<bool> backCallBack;

    public override void Init()
    {
        base.Init();
        Layer = LayerMenue.TIPS;
        btnBack = Find<Button>(gameObject, "BtnBack");
        btnWatch = Find<Button>(gameObject, "BtnWatch");
        m_BtnMask = Find<Button>(gameObject, "Mask");
        RegisterBtnEvent();
        PlayAnimation(Find(gameObject,"msgPanel"));
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
            backCallBack?.Invoke(false);
            UIManager.Instance.CloseUI(Layer);
        });

        btnWatch.onClick.AddListener(() =>
        {
            SDKManager.Instance.ShowBasedVideo(suceessCallBack, failedCallback, closeCallback);
        });
    }

    public static void SetCallBack(Action<string, string, float> calllback, Action<string, string> failedCallback, Action<string> closeCallback, Action<bool> backCallBack)
    {
        suceessCallBack = calllback;
        UI_SeedMutationAds.failedCallback = failedCallback;
        UI_SeedMutationAds.closeCallback = closeCallback;
        UI_SeedMutationAds.backCallBack = backCallBack;
    }
}
