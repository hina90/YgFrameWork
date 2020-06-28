using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkunkWalkState : FSMState
{
    private bool waitOperation = false;

    public SkunkWalkState()
    {
        waiTime = 1.5f;
        stateID = FSMStateID.SkunkMove;

        Transform moveTrans = GameManager.Instance.GetSkunkMovePoint();
        targetPosition = moveTrans.position;
    }
    /// <summary>
    /// 条件转换到放屁状态
    /// </summary>
    /// <param name="actor"></param>
    public override void Reason(BaseActor actor)
    {
        if(ChangeState)
        {
            actor.AiController.SetTransition(Transition.SkunkMoveOver, 1);
        }
    }
    /// <summary>
    /// 行为
    /// </summary>
    /// <param name="actor"></param>
    public override void Act(BaseActor actor)
    {
        if (waitOperation)
            return;

        actor.AiController.FindPath(targetPosition);
        moveOver = actor.AiController.IsReached();

        currenTime += Time.fixedDeltaTime;
        if (currenTime >= waiTime)
        {
            moveOver = actor.AiController.IsReached();
            if (moveOver)
            {
                waitOperation = true;
                actor.StartOperation = true;
                ChangeState = true;
            }
        }
    }
}
