using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 臭鼬AI
/// </summary>
public class SkunkAIController : AIController
{
    protected override void ConstructFSM()
    {
        SkunkWalkState walk = new SkunkWalkState();
        walk.AddTransition(Transition.SkunkMoveOver, FSMStateID.Leave);
        walk.AddTransition(Transition.SkunkMoveOver, FSMStateID.SkunkFart);
        SkunkFartState fart = new SkunkFartState();
        fart.AddTransition(Transition.SkunkFartOver, FSMStateID.Leave);

        LeaveState leave = new LeaveState();

        AddFSMState(walk);
        AddFSMState(fart);
        AddFSMState(leave);
    }
}
