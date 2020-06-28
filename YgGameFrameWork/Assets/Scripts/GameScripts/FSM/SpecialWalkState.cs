using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 特殊点移动状态
/// </summary>
public class SpecialWalkState : FSMState
{
    private bool waitOperation = false;

    public SpecialWalkState()
    {
        waiTime = 1.5f;
        stateID = FSMStateID.SpecialPointMove;

        Transform moveTrans = GameManager.Instance.GetSpecialPoint();
        targetPosition = moveTrans.position;
    }
    /// <summary>
    /// 条件转换
    /// </summary>
    /// <param name="actor"></param>
    public override void Reason(BaseActor actor)
    {
        
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
                actor.FaceDirection(new Vector3(1, 1, 1));
            }
        }
    }
}
