using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 小偷
/// </summary>
public class Thief : BaseSpecialActor
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
        AiController = gameObject.GetOrAddComponent<ThiefAIController>();
        AiController.Initialize(this);
    }
    /// <summary>
    /// 点击事件
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
        if (AiController.CurrentStateID == FSMStateID.ThiefRandomMove)
            AiController.SetTransition(Transition.ThiefRandomMoveOver, 0);
        else if (AiController.CurrentStateID == FSMStateID.Steal)
            AiController.SetTransition(Transition.StealOver, 0);
    }
}
