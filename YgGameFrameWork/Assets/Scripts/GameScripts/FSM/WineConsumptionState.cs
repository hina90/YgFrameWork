using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 消费酒柜状态
/// </summary>
public class WineConsumptionState : ConsumptionState
{
    public WineConsumptionState()
    {
        waiTime = 3.5f;
        stateID = FSMStateID.WineConsumption;

        offSetVec = new Vector3(0f, -0.3f, 0f);
    }
    /// <summary>
    /// 改变状态
    /// </summary>
    protected override void ChangeTransition(BaseActor actor)
    {
        base.ChangeTransition(actor);
        int state = GameManager.Instance.GetAfterConsumptionState(stateID, actor);
        actor.AiController.SetTransition(Transition.WineConsumptionOver, state);
    }
    /// <summary>
    /// 获取空闲酒柜
    /// </summary>
    protected override ConsumeEntity GetFreeConsumptionFacility()
    {
        ConsumeEntity wineCabinet = null;
        ConsumeEntity wineCabinetConsume = GameManager.Instance.GetConsumeWineCabinet();
        if (!wineCabinetConsume.HaveGuest)
            wineCabinet = wineCabinetConsume;

        return wineCabinet;
    }
}
