using UnityEngine;


/// <summary>
/// 偷小鱼干状态
/// </summary>
public class StealState : FSMState
{
    private CashierEntity cashier;

    public StealState()
    {
        waiTime = 1.5f;
        stateID = FSMStateID.Steal;
    }
    /// <summary>
    /// 条件转换
    /// </summary>
    /// <param name="aiController"></param>
    /// <param name="actor"></param>
    public override void Reason(BaseActor actor)
    {
        if (ChangeState)
        {
            actor.AiController.SetTransition(Transition.StealOver, 1);
        }
    }
    /// <summary>
    /// 偷鱼干行为
    /// </summary>
    /// <param name="aiController"></param>
    /// <param name="actor"></param>
    public override void Act(BaseActor actor)
    {
        if (null == cashier)
            cashier = FacilitiesManager.Instance.GetCashier();

        if (null != cashier)
        {
            targetPosition = cashier.transform.position;
            actor.AiController.FindPath(targetPosition);
            moveOver = actor.AiController.IsReached();

            currenTime += Time.fixedDeltaTime;
            if (currenTime >= waiTime)
            {
                moveOver = actor.AiController.IsReached();
                if (moveOver)
                {
                    currenTime = 0;
                    cashier.ResetGratuity();
                }
            }
        }
        else
        {
            currenTime += Time.deltaTime;
            if (currenTime >= waiTime)
            {
                currenTime = 0;
                ChangeState = true;
            }
        }
    }
}
