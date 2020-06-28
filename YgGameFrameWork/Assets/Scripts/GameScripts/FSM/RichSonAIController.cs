using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 富二代AI
/// </summary>
public class RichSonAIController : AIController
{
    protected override void ConstructFSM()
    {
        RichSonRandomWalkState randomWalk = new RichSonRandomWalkState();
        randomWalk.AddTransition(Transition.RichSonRandomMoveOver, FSMStateID.Leave);

        LeaveState leave = new LeaveState();

        AddFSMState(randomWalk);
        AddFSMState(leave);
    }
}
