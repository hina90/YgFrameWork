using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 垃圾大王随机移动状态
/// </summary>
public class GarbageThrowerRandomWalkState : RandomWalkState
{
    private float garbageTime = 0;
    private float garbageIntervalTime = 10;

    public GarbageThrowerRandomWalkState()
    {
        stateID = FSMStateID.ThrowerRandomMove;
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
            actor.AiController.SetTransition(Transition.RandomMoveOver, 0);
        }
    }
    /// <summary>
    /// 行为
    /// </summary>
    /// <param name="actor"></param>
    public override void Act(BaseActor actor)
    {
        garbageTime += Time.deltaTime;
        if(garbageTime >= garbageIntervalTime)
        {
            if((actor as GarbageThrower).ThrowNumber <= 0)
            {
                ChangeState = true;
                return;
            }
            garbageTime = 0;
            (actor as GarbageThrower).ThrowGarbage();
        }

        RandomMove(actor);
    }
    /// <summary>
    /// 行动完成后的行为
    /// </summary>
    protected override void ActOverHandle(BaseActor actor)
    {
        RandomMove(actor);
    }
}
