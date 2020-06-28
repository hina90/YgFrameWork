using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 流浪歌手AI
/// </summary>
public class SinggerAIController : AIController
{
    protected override void ConstructFSM()
    {
        SpecialWalkState specialWalk = new SpecialWalkState();
        specialWalk.AddTransition(Transition.SpecialPointMoveOver, FSMStateID.Leave);

        LeaveState leave = new LeaveState();

        AddFSMState(specialWalk);
        AddFSMState(leave);
    }
}
