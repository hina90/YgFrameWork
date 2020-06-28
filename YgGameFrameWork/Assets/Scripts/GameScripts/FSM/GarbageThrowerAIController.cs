using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 垃圾大王AI
/// </summary>
public class GarbageThrowerAIController : AIController
{
    protected override void ConstructFSM()
    {
        GarbageThrowerRandomWalkState randomMoveState = new GarbageThrowerRandomWalkState();
        randomMoveState.AddTransition(Transition.RandomMoveOver, FSMStateID.Leave);

        LeaveState leave = new LeaveState();

        AddFSMState(randomMoveState);
        AddFSMState(leave);
    }
}
