using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Prop : UIBase
{

    public override void Initialize(object[] param = null)
    {
        base.Initialize(param);
        UICamera = true;
    }

    protected override void Enter()
    {
        base.Enter();

        Find<Button>(gameObject, "Button").onClick.AddListener(()=>
        {
            var sceneMgr = ManagementCenter.GetManager<CSceneManager>();
            sceneMgr.ChangeScene("BattleScene", "Map_1");

            var panelMgr = ManagementCenter.GetManager<PanelManager>();
            panelMgr.ClosePanel("UI_Prop");
        });
    }
}
