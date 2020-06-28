using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消费状态
/// </summary>
public class ConsumptionState : FSMState
{
    private ConsumeEntity currentConsumption;   //当前消费设施

    private float waitCurTime = 0;
    protected float consumptionTime = 2.5f;     //消费时间
    protected Vector3 offSetVec = new Vector3(0f, 1f, 0f);


    /// <summary>
    /// 消费完后的条件转换
    /// </summary>
    /// <param name="aiController"></param>
    /// <param name="actor"></param>
    public override void Reason(BaseActor actor)
    {
        if (ChangeState)
        {
            currentConsumption.HaveGuest = false;
            ChangeTransition(actor);
            waitCurTime = 0;
        }
    }
    /// <summary>
    /// 改变状态
    /// </summary>
    protected virtual void ChangeTransition(BaseActor actor)
    {
        
    }
    /// <summary>
    /// 消费行为
    /// </summary>
    /// <param name="aiController"></param>
    /// <param name="actor"></param>
    public override void Act(BaseActor actor)
    {
        if (currentConsumption == null)
            currentConsumption = GetFreeConsumptionFacility();
        if (currentConsumption == null)
            return;

        currentConsumption.HaveGuest = true;

        actor.AiController.FindPath(currentConsumption.transform.position + offSetVec);
        moveOver = actor.AiController.IsReached();

        currenTime += Time.fixedDeltaTime;
        if (currenTime >= waiTime)
        {
            moveOver = actor.AiController.IsReached();
            if (moveOver)
            {
                waitCurTime += Time.fixedDeltaTime;
                if (waitCurTime >= consumptionTime)
                {
                    currentConsumption.ConsumeIncome();
                    ChangeState = true;
                    waitCurTime = 0;
                }
            }
        }
    }
    /// <summary>
    /// 获取消费设施
    /// </summary>
    /// <returns></returns>
    protected virtual ConsumeEntity GetFreeConsumptionFacility()
    {
        return null;
    }
}
