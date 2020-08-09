using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.IO;

/// <summary>
/// 喝水
/// </summary>
public class DrinkingState : FSMState
{
    private bool isDrinking = false;
    private Transform waterTank;
    //事件触发点
    private Transform triggerPoint;

    public DrinkingState()
    {
        waiTime = 3f;
        stateID = FSMStateID.Drink;
    }

    /// <summary>
    /// 状态转换
    /// </summary>
    /// <param name="actor"></param>
    public override void Reason(BaseActor actor)
    {
        if (ChangeState)
        {
            actor.AiController.SetTransition(actor, FSMStateID.VisitStart);
        }
    }
    /// <summary>
    /// 状态行为
    /// </summary>
    /// <param name="actor"></param>
    public override void Act(BaseActor actor)
    {

    }
    
}
