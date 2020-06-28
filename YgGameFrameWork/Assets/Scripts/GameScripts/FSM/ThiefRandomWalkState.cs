using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 盗贼随机移动
/// </summary>
public class ThiefRandomWalkState : RandomWalkState
{
    private const int MAX_MOVE_NUMBER = 2;
    private int currentMoveNumber = 0;

    public ThiefRandomWalkState()
    {
        waiTime = 1.5f;
        stateID = FSMStateID.ThiefRandomMove;
    }
    /// <summary>
    /// 条件转换
    /// </summary>
    /// <param name="actor"></param>
    public override void Reason(BaseActor actor)
    {
        if (ChangeState)
        {
            moveTrans = null;
            actor.AiController.SetTransition(Transition.ThiefRandomMoveOver, 1);
        }
    }
    /// <summary>
    /// 行为
    /// </summary>
    /// <param name="actor"></param>
    public override void Act(BaseActor actor)
    {
        CashierEntity cashier = FacilitiesManager.Instance.GetCashier();
        if (cashier != null)
        {
            if (currentMoveNumber == MAX_MOVE_NUMBER)
                ChangeState = true;
            else
                RandomMove(actor);
        }
        else   //获取随机移动点
        {
            RandomMove(actor);
        }
    }
    /// <summary>
    /// 行动完成后的行为
    /// </summary>
    protected override void ActOverHandle(BaseActor actor)
    {
        currentMoveNumber++;
        RandomMove(actor);
    }
}
