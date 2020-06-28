using System;
using System.Collections.Generic;
using Tool.Database;
using UnityEngine;
using UnityEngine.UI;

public class GuideManager : Singleton<GuideManager>
{
    private GuideModel guideModel;
    private List<Button> guideBtns;
    public Queue<int> guideQueue;
    public int CurGuideId;
    public GuideConfigData CurGuideConfig;
    public int CurWeakGuideId;
    public GuideConfigData CurWeakGuideConfig;
    public Dictionary<GameEvent, List<Callback<object[]>>> guideEventDic;
    public FacilitiesItemData facilitiesItemData;

    public void Init()
    {
        guideModel = new GuideModel();
        guideBtns = new List<Button>();
        guideQueue = new Queue<int>();
        guideEventDic = new Dictionary<GameEvent, List<Callback<object[]>>>();
        CurGuideId = guideModel.GuideId;
        CurGuideConfig = ConfigDataManager.Instance.GetDatabase<GuideConfigDatabase>().GetDataByKey(CurGuideId.ToString());
        CurWeakGuideId = guideModel.WeakGuideId;
    }

    /// <summary>
    /// 新手引导开始
    /// </summary>
    private void StartGuide()
    {
        if (UIManager.Instance.GetResUI(typeof(UI_Guide).ToString()) == null)
        {
            UIManager.Instance.OpenUI<UI_Guide>();
        }
    }

    /// <summary>
    /// 检测是否存在引导
    /// </summary>
    /// <param name="uiName">场景UI</param>
    public void CheckGuide(string uiName)
    {
        if (!Global.OPEN_GUIDE) return;
        if (IsFinishGuide() || !string.Equals(uiName, CurGuideConfig.ui)) return;
        StartGuide();
        BroadcastGuideEvent(GameEvent.ENTER_GUIDE);
    }

    /// <summary>
    /// 进入下一步引导
    /// </summary>
    public void EnterNextStep()
    {
        if (IsFinishGuide()) return;
        BroadcastGuideEvent(GameEvent.ENTER_GUIDE);
    }

    /// <summary>
    /// 引导完成
    /// </summary>
    public void FinishGuide()
    {
        if (IsFinishGuide()) return;
        if (CurGuideConfig.nextStep == 0)
        {
            guideModel.CompleteCurGuide(CurGuideId);
            CurGuideId = guideModel.GuideId;
        }
        else
        {
            CurGuideId = CurGuideConfig.nextStep;
            EnterNextStep();
        }
        //UnityEngine.Debug.LogWarning("CurGuideId：" + CurGuideId);
        CurGuideConfig = ConfigDataManager.Instance.GetDatabase<GuideConfigDatabase>().GetDataByKey(CurGuideId.ToString());
    }

    #region 游戏弱引导模块

    private bool isWeakGuiding = false;
    public bool isGuideBuyTable;
    public bool forbidTouch;

    /// <summary>
    /// 完成当前弱引导
    /// </summary>
    public void FinishWeakGuide()
    {
        guideModel.CompleteCurWeakGuide(CurWeakGuideId);
        isWeakGuiding = false;
    }

    /// <summary>
    /// 执行下一步弱引导
    /// </summary>
    public void EnterWeakStep(int weakGuideId)
    {
        if (IsFinishWeakGuide() || isWeakGuiding) return;
        if (weakGuideId != 1101)
        {
            if (weakGuideId != CurWeakGuideId + 1) return;
        }
        else
        {
            isGuideBuyTable = true;
            if (weakGuideId <= CurWeakGuideId) return;
        }
        CurWeakGuideId = weakGuideId;
        CurWeakGuideConfig = ConfigDataManager.Instance.GetDatabase<GuideConfigDatabase>().GetDataByKey(weakGuideId.ToString());
        new GuideWeakClick(CurWeakGuideConfig);
        isWeakGuiding = true;
    }

    /// <summary>
    /// 是否已完成弱引导
    /// </summary>
    /// <returns></returns>
    public bool IsFinishWeakGuide()
    {
        return guideModel.IsFinishWeakGuide();
    }

    #endregion

    /// <summary>
    /// 游戏是否需要引导
    /// </summary>
    /// <returns></returns>
    public bool IsFinishGuide()
    {
        return guideModel.IsFinishGuide();
    }

    /// <summary>
    /// 注册引导按钮
    /// </summary>
    /// <param name="btn"></param>
    /// <param name="uiName"></param>
    public void RegisterBtn(Button btn)
    {
        if (IsFinishGuide()) return;
        List<Button> nullList = guideBtns.FindAll(o => o == null);
        for (int i = 0; i < nullList.Count; i++)
        {
            guideBtns.Remove(nullList[i]);
        }
        if (!guideBtns.Contains(btn))
        {
            guideBtns.Add(btn);
        }
    }

    /// <summary>
    /// 移除引导按钮
    /// </summary>
    /// <param name="btnName"></param>
    public void RemoveBtn(string btnName)
    {
        Button btn = guideBtns.Find(o => o.name == btnName);
        if (btn != null)
        {
            guideBtns.Remove(btn);
        }
    }

    /// <summary>
    /// 获取引导按钮
    /// </summary>
    /// <param name="btnName"></param>
    /// <returns></returns>
    public Button GetButton(string btnName)
    {
        return guideBtns.Find(o => o.name == btnName);
    }

    /// <summary>
    /// 引导事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="args"></param>
    public void BroadcastGuideEvent(GameEvent eventType, params object[] args)
    {
        if (guideEventDic.TryGetValue(eventType, out List<Callback<object[]>> callBacks))
        {
            for (int i = 0; i < callBacks.Count; i++)
            {
                callBacks[i]?.Invoke(args);
            }
        }
    }

    /// <summary>
    /// 监听引导事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="args"></param>
    public void AddGuideListener(GameEvent eventType, Callback<object[]> fun)
    {
        if (!guideEventDic.TryGetValue(eventType, out List<Callback<object[]>> callBacks))
        {
            guideEventDic[eventType] = new List<Callback<object[]>>();
        }
        if (guideEventDic[eventType].Contains(fun)) return;
        guideEventDic[eventType].Add(fun);
    }

    /// <summary>
    /// 移除引导事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="args"></param>
    public void RemoveGuideListener(GameEvent eventType, Callback<object[]> fun)
    {
        if (guideEventDic.TryGetValue(eventType, out List<Callback<object[]>> callBacks))
        {
            if (guideEventDic[eventType].Contains(fun))
            {
                callBacks.Remove(fun);
            }
            if (callBacks.Count <= 0)
            {
                guideEventDic.Remove(eventType);
            }
        }
    }
}
