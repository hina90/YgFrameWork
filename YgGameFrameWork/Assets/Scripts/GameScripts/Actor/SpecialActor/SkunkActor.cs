using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 臭鼬
/// </summary>
public class SkunkActor : BaseSpecialActor
{
    public override void Init()
    {
        base.Init();
    }
    /// <summary>
    /// 初始化AI
    /// </summary>
    protected override void InitAIController()
    {
        AiController = gameObject.GetOrAddComponent<SkunkAIController>();
        AiController.Initialize(this);
    }
    /// <summary>
    /// 点击逻辑
    /// </summary>
    private void OnMouseDown()
    {
        Event();
    }
    /// <summary>
    /// 事件完成回调
    /// </summary>
    protected override void EventCompleteCallback()
    {
        PlaySpineAnimation(0, "animation", true);
        if (AiController.CurrentStateID == FSMStateID.SkunkMove)
            AiController.SetTransition(Transition.SkunkMoveOver, 0);
        else if (AiController.CurrentStateID == FSMStateID.SkunkFart)
            AiController.SetTransition(Transition.SkunkFartOver, 0);
    }
}
