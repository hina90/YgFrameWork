using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
/// <summary>
/// 排队状态
/// </summary>
public class QueueUpDinnerState : FSMState
{
    private QueueUp queueCom;

    public QueueUpDinnerState()
    {
        stateID = FSMStateID.QueueUpDinner;
        queueCom = GameManager.Instance.GetQueueDinner();
    }
    /// <summary>
    /// 排队的条件转换
    /// </summary>
    /// <param name="aiController"></param>
    /// <param name="actor"></param>
    public override void Reason(BaseActor actor)
    {

    }
    /// <summary>
    /// 排队行为
    /// </summary>
    /// <param name="aiController"></param>
    /// <param name="actor"></param>
    public override void Act(BaseActor actor)
    {
        if (queueCom.Count() > 0)
            targetPosition = queueCom.GetQueuePosition(actor);
        else
            targetPosition = queueCom.transform.position;

        actor.AiController.FindPath(targetPosition);

        queueCom.AddToQueueList(actor);
    }
}
