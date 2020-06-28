using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 二代随机移动状态
/// </summary>
public class RichSonRandomWalkState : RandomWalkState
{
    public RichSonRandomWalkState()
    {
        waiTime = 1.5f;
        stateID = FSMStateID.RichSonRandomMove;
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
            actor.AiController.SetTransition(Transition.RichSonRandomMoveOver, 0);
        }
    }
    /// <summary>
    /// 行为
    /// </summary>
    /// <param name="actor"></param>
    public override void Act(BaseActor actor)
    {
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
