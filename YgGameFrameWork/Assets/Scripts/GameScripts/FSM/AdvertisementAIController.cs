using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 广告功能角色AI
/// </summary>
public class AdvertisementAIController : AIController
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
