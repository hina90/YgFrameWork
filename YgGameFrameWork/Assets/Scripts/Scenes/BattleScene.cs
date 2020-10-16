using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗场景
/// </summary>
public class BattleScene : BaseScene
{
    protected override void OnCreate(params object[] param)
    {
        base.OnCreate(param);
        ResName = "BattleScene";
    }
    protected override void Enter()
    {
        base.Enter();

    }
    public override Dictionary<GameEvent, Callback<object[]>> CtorEvent()
    {
        return base.CtorEvent();
    }

    public override void Quit()
    {
        base.Quit();
    }
}
