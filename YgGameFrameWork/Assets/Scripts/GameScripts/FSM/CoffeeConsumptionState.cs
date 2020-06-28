using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 消费咖啡状态
/// </summary>
public class CoffeeConsumptionState : ConsumptionState
{
    public CoffeeConsumptionState()
    {
        waiTime = 3.2f;
        stateID = FSMStateID.CoffeeConsumption;
    }
    /// <summary>
    /// 改变状态
    /// </summary>
    protected override void ChangeTransition(BaseActor actor)
    {
        base.ChangeTransition(actor);
        int state = GameManager.Instance.GetAfterConsumptionState(stateID, actor);
        actor.AiController.SetTransition(Transition.CoffeeConsumptionOver, state);
    }
    /// <summary>
    /// 获取咖啡台状态
    /// </summary>
    protected override ConsumeEntity GetFreeConsumptionFacility()
    {
        ConsumeEntity coffeeMaker = null;
        ConsumeEntity coffee = GameManager.Instance.GetConsumeCoffeeMaker();
        if (!coffee.HaveGuest)
            coffeeMaker = coffee;

        return coffeeMaker;
    }
}
