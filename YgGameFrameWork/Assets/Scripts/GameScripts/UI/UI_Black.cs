using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遮罩
/// </summary>
public class UI_Black : UIBase
{
    public override void Init()
    {
        base.Init();
        Layer = LayerMenue.BLACK;
    }

    protected override void Enter()
    {
        base.Enter();
    }
}
