using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 加载Loading界面
/// </summary>
public class UI_Loading : UIBase
{
    private Image bar;
    private Text barText;

    /// <summary>
    /// 初始化UI
    /// </summary>
    public override void Initialize(object[] param = null)
    {
        base.Initialize(param);
    }
    /// <summary>
    /// 进入LoadingUI
    /// </summary>
    protected override void Enter()
    {
        bar = Find<Image>(gameObject, "bar");
        barText = Find<Text>(gameObject, "barTxt");
        bar.fillAmount = 0;
    }
    /// <summary>
    /// 显示进度
    /// </summary>
    /// <param name="value"></param>
    private void SetProgress(float value)
    {
        barText.text = (value * 100).ToString();
        bar.fillAmount = value;
    }
    /// <summary>
    /// UI自定义事件列表
    /// </summary>
    /// <returns></returns>
    public override Dictionary<GameEvent, Callback<object[]>> CtorEvent()
    {
        Dictionary<GameEvent, Callback<object[]>> eventDic = new Dictionary<GameEvent, Callback<object[]>>();

        //加载进度条事件
        eventDic[GameEvent.SCENE_LOAD_PROGRESS] = delegate (object[] param)
        {
            SetProgress((float)param[0]);
        };
        //加载完成
        eventDic[GameEvent.SCENE_LOAD_COMPLETE] = delegate (object[] param)
        {
            var timeMgr = ManagementCenter.GetExtManager("TimerManager") as CTimer;
            timeMgr.AddTimer(0.5f, 0, (obj) =>
            {
                var panelMgr = ManagementCenter.GetManager<PanelManager>();
                panelMgr.ClosePanel("UI_Loading");
            });
        };

        return eventDic;
    }

    public override void Release()
    {
        base.Release();
    }
}
