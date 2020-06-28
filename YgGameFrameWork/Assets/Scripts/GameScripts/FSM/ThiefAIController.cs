using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 小偷AI
/// </summary>
public class ThiefAIController : AIController
{
    protected override void ConstructFSM()
    {
        ThiefRandomWalkState randomWalk = new ThiefRandomWalkState();
        randomWalk.AddTransition(Transition.ThiefRandomMoveOver, FSMStateID.Leave);
        randomWalk.AddTransition(Transition.ThiefRandomMoveOver, FSMStateID.Steal);

        StealState steal = new StealState();
        steal.AddTransition(Transition.StealOver, FSMStateID.Leave);
        steal.AddTransition(Transition.StealOver, FSMStateID.ThiefRandomMove);

        LeaveState leave = new LeaveState();

        AddFSMState(randomWalk);
        AddFSMState(steal);
        AddFSMState(leave);
    }
}
