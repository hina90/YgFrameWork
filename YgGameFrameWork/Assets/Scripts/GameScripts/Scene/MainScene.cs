using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tool.Database;

/// <summary>
/// 地图场景
/// </summary>
public class MainScene : BaseScene
{
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="param"></param>
    protected override void OnCreate(params object[] param)
    {
        UIManager.Instance.OpenUI<UI_Black>();
        UIManager.Instance.OpenUI<UI_Loading>();

        base.OnCreate(param);
        ResName = "MainScene";
    }
    /// <summary> 
    /// 进入
    /// </summary>
    protected override void Enter()
    {
        base.Enter();
        //GameManager.Instance.NormalGuest();

        UIManager.Instance.OpenUI<UI_Money>();
        UIManager.Instance.OpenUI<UI_Main>();
        //初始化所有已使用的建造设施
        FacilitiesManager.Instance.InitAllBuilding();
    }
    /// <summary>
    /// 场景主循环
    /// </summary>
    public override void MainUpdate()
    {
        GameManager.Instance.MainUpdate();
    }

    public override void Quit()
    {
        base.Quit();
        TimerManager.Instance.AddToRemove("RandomCreatGuest");
    }
}
