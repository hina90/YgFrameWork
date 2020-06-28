using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 甜品消费状态
/// </summary>
public class DessertConsumptionState : ConsumptionState
{
    public DessertConsumptionState()
    {
        waiTime = 3.3f;
        stateID = FSMStateID.DessertConsumption;
    }
    /// <summary>
    /// 改变状态
    /// </summary>
    protected override void ChangeTransition(BaseActor actor)
    {
        base.ChangeTransition(actor);
        int state = GameManager.Instance.GetAfterConsumptionState(stateID, actor);
        actor.AiController.SetTransition(Transition.DessertConsumptionOver, state);
    }
    /// <summary>
    /// 获取甜品台
    /// </summary>
    protected override ConsumeEntity GetFreeConsumptionFacility()
    {
        ConsumeEntity dessertTable = null;
        ConsumeEntity dessert = GameManager.Instance.GetConsumeDessertTable();
        if (!dessert.HaveGuest)
            dessertTable = dessert;

        return dessertTable;
    }
}
