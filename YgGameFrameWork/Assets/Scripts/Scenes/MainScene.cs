using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主场景
/// </summary>
public class MainScene : BaseScene
{
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="param"></param>
    protected override void OnCreate(params object[] param)
    {
        base.OnCreate(param);
        ResName = "MainScene";
    }

    /// <summary>
    /// 进入主场景
    /// </summary>
    protected override void Enter()
    {
        base.Enter();

        var panelMgr = ManagementCenter.GetManager<PanelManager>();
        panelMgr.OpenPanel<UI_MainGame>(UILayer.Common);
    }
    /// <summary>
    /// 退出主场景
    /// </summary>
    public override void Quit()
    {
        base.Quit();
    }
}
