using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_WishingPool : UIBase
{
    private Button m_BtnMask;
    private Button m_BtnWish;
    private Text m_TxtWish;
    private Text m_TxtPrompt;
    private static Callback wishAction;
    private GardenModule gardenModule;
    private SkeletonGraphic animWish;
    private bool isWishing;

    public override void Init()
    {
        base.Init();
        Layer = LayerMenue.UI;
        m_BtnMask = Find<Button>(gameObject, "Mask");
        m_BtnWish = Find<Button>(gameObject, "BtnWish");
        m_TxtWish = Find<Text>(m_BtnWish.gameObject, "Text");
        m_TxtPrompt = Find<Text>(gameObject, "TxtPrompt");
        animWish = Find<SkeletonGraphic>(gameObject, "AnimWish");
        gardenModule = GameModuleManager.Instance.GetModule<GardenModule>();
        RegisterBtnEvent();
        PlayAnimation(Find(gameObject, "msgPanel"));
    }

    private void RegisterBtnEvent()
    {
        m_BtnMask.onClick.AddListener(() =>
        {
            if (isWishing) return;
            UIManager.Instance.CloseUI(Layer);
        });

        m_BtnWish.onClick.AddListener(() =>
        {
            if (gardenModule.RemainWishTimes > 0)
            {
                gardenModule.RemainWishTimes--;

                if (gardenModule.RemainWishTimes > 0)
                {
                    gardenModule.IntervalTimerCount(gardenModule.WishTimes - gardenModule.RemainWishTimes - 1);
                }
                if (gardenModule.RemainWishTimes == 0)
                {
                    //许愿次数用完时，许愿冷却计时
                    gardenModule.CDTimerCount();
                }
                m_BtnWish.interactable = false;
            }
            animWish.AnimationState.SetAnimation(0, "animation2", false);
            Invoke("ClosePanel", 1.5f);
            isWishing = true;

        });
    }

    protected override void Enter()
    {
        base.Enter();
        RefreshUI();
        animWish.AnimationState.SetAnimation(0, "animation", true);
    }

    public override Dictionary<GameEvent, Callback<object[]>> CtorEvent()
    {
        Dictionary<GameEvent, Callback<object[]>> eventDic = new Dictionary<GameEvent, Callback<object[]>>
        {
            [GameEvent.UPDATE_WISHPOOL_RESETTIME] = delegate (object[] param)
            {
                m_TxtPrompt.text = $"下次免费:{Utils.Second2Hours((int)param[0])}";
                if ((int)param[0] == 0)
                {
                    m_TxtPrompt.text = "";
                    RefreshUI();
                }
            },
            [GameEvent.UPDATE_WISHPOOL_INTERVALTTIME] = delegate (object[] param)
            {
                int time = gardenModule.WishIntervalTime[(int)param[0]];
                m_TxtPrompt.text = $"下次许愿:{Utils.Second2Hours(time)}";
                if (time == 0)
                {
                    m_TxtPrompt.text = "";
                    m_BtnWish.interactable = true;
                }
            }
        };
        return eventDic;
    }

    public static void SetWishActin(Callback callback)
    {
        wishAction = callback;
    }

    /// <summary>
    /// 刷新UI显示
    /// </summary>
    private void RefreshUI()
    {
        m_TxtWish.text = $"{gardenModule.RemainWishTimes}/{gardenModule.WishTimes}";
        bool isCd = false; int index = 0;
        if (gardenModule.RemainWishTimes < gardenModule.WishTimes)
        {
            index = gardenModule.WishTimes - gardenModule.RemainWishTimes - 1;
            isCd = gardenModule.WishIntervalTime[index] > 0;
        }
        m_BtnWish.interactable = gardenModule.RemainWishTimes > 0 && !isCd;
    }

    private void ClosePanel()
    {
        UIManager.Instance.CloseUI(Layer);
        wishAction?.Invoke();
        isWishing = false;
    }

    public override void Release()
    {
        base.Release();
    }
}
