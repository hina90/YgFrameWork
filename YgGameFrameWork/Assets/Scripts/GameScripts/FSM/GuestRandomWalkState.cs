using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 顾客随机移动
/// </summary>
public class GuestRandomWalkState : RandomWalkState
{
    public GuestRandomWalkState()
    {
        waiTime = 3f;
        stateID = FSMStateID.GuestRandomMove;
    }
     
    public override void Reason(BaseActor actor)
    {
        if (ChangeState)
        {
            moveTrans = null;

            int rate = GameManager.Instance.GetAfterConsumptionState(stateID, actor);
            actor.AiController.SetTransition(Transition.RandomMoveOver, rate);
        }
    }

    /// <summary>
    /// 随机移动行为
    /// </summary>
    /// <param name="aiController"></param>
    /// <param name="actor"></param>
    public override void Act(BaseActor actor)
    {
        RandomMove(actor);
    }

    /// <summary>
    /// 行动完成
    /// </summary>
    protected override void ActOverHandle(BaseActor actor)
    {
        if (actor.RandomMoveNumber == BaseActor.MAX_RANDOMMOVE)
        {
            ChangeState = true;
            return;
        }

        if (GameManager.Instance.GetConsumeCoffeeMaker() == null
            && GameManager.Instance.GetConsumeDessertTable() == null
                && GameManager.Instance.GetConsumeWineCabinet() == null)
        {
            actor.RandomMoveNumber++;
            RandomMove(actor);
        }
        else
            ChangeState = true;
    }
}
