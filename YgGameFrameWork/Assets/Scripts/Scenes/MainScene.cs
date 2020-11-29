using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主场景
/// </summary>
public class MainScene : BaseScene
{
    private LogicManager logicMgr = null;

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="param"></param>
    protected override void OnCreate(params object[] param)
    {
        base.OnCreate(param);
        ResName = "MainScene";
        sceneMgr.SetCamera(new Vector3(16.12f, 27.86f, -14.48f), Quaternion.Euler(50.49f, -78.1f, 9.4f));

    }

    /// <summary>
    /// 进入主场景
    /// </summary>
    protected override void Enter()
    {
        base.Enter();

        var panelMgr = ManagementCenter.GetManager<PanelManager>();
        panelMgr.OpenPanel<UI_MainGame>(UILayer.Common);

        logicMgr = new LogicManager();
        logicMgr.Initialize();
    }
    /// <summary>
    /// 退出主场景
    /// </summary>
    public override void Quit()
    {
        base.Quit();
    }
}
