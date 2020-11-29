using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// 主界面
/// </summary>
public class UI_MainGame : UIBase
{
    /// <summary>
    /// 初始化
    /// </summary>
    public override void Initialize(object[] param = null)
    {
        base.Initialize(param);
    }

    /// <summary>
    /// 进入战斗UI
    /// </summary>
    protected override void Enter()
    {
        Find<Button>(gameObject, "Btn1").onClick.AddListener(delegate()
        {
            var panelMgr = ManagementCenter.GetManager<PanelManager>();
            panelMgr.OpenPanel<UI_Prop>(UILayer.Fixed);


            //CTimer timeMgr = ManagementCenter.GetExtManager("TimerManager") as CTimer;
            //timeMgr.AddTimer(3, 0, (obj)=>
            //{
            //    panelMgr.OpenPanel<UI_Second>(UILayer.Fixed);
            //});

        });
    }
    public override Dictionary<GameEvent, Callback<object[]>> CtorEvent()
    {
        eventDic = base.CtorEvent();


        return eventDic;
    }
    /// <summary>
    /// 释放
    /// </summary>
    public override void Release()
    {
        base.Release();
    }
 
}

