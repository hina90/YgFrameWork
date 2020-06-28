using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 离开状态
/// </summary>
public class LeaveState : FSMState
{
    //门
    private Transform door;

    public LeaveState()
    {
        waiTime = 2f;
        stateID = FSMStateID.Leave;
        door = GameManager.Instance.GetDoor();
    }

    public override void Reason(BaseActor actor)
    {
        if (ChangeState)
        {
            GameManager.Instance.RemoveGuest(actor);
        }
    }

    /// <summary>
    /// 离开行为
    /// </summary>
    public override void Act(BaseActor actor)
    {
        actor.AiController.FindPath(door.transform.position);
        moveOver = actor.AiController.IsReached();

        currenTime += Time.fixedDeltaTime;
        if (currenTime >= waiTime)
        {
            moveOver = actor.AiController.IsReached();
            if (moveOver)
            {
                ChangeState = true;
            }
        }
    }
}
