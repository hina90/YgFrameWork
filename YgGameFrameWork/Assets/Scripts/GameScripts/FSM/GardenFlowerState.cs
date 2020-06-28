using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 赏花状态
/// </summary>
public class GardenFlowerState : FSMState
{
    //状态索引
    private int stateIndex = 0;
    //花盆
    private ParterreEntity flower; 
    //偏移量
    private Vector3 offSetVec = new Vector3(0f, 0.7f, 0);
    protected float flowerWaitime = 3.5f;           //赏花时间
    protected float waitCurrentime = 0f;            //当前等待时间

    public GardenFlowerState()
    {
        waiTime = 2f;
        stateID = FSMStateID.GardenFlowers;
    }
    ///// <summary>
    ///// 条件转换
    ///// </summary>
    ///// <param name="aiController"></param>
    ///// <param name="actor"></param>
    public override void Reason(BaseActor actor)
    {
        if(ChangeState)
        {
            if(null != flower)
            {
                flower.HaveGuest = false;
                flower = null;
                waitCurrentime = 0;
            }
            actor.AiController.SetTransition(Transition.GardenFlowersOver, stateIndex);
        }
    }
    /// <summary>
    /// 行为
    /// </summary>
    /// <param name="aiController"></param>
    /// <param name="actor"></param>
    public override void Act(BaseActor actor)
    {
        if(null == flower)
            flower = CheckAdmireFlowers();
        if (null == flower)
        {
            stateIndex = 0;
            ChangeState = true;
            return;
        }
        
        actor.AiController.FindPath(flower.transform.position + offSetVec);
        moveOver = actor.AiController.IsReached();

        currenTime += Time.fixedDeltaTime;
        if (currenTime >= waiTime)
        {
            moveOver = actor.AiController.IsReached();
            if (moveOver)
            {
                waitCurrentime += Time.fixedDeltaTime;
                if (waitCurrentime >= flowerWaitime)
                {
                    flower.Ornamental();
                    waitCurrentime = 0;
                    ChangeState = true;
                    if (GameManager.Instance.CheckAreaUnLock(1004))              //便利店开启则去便利店
                        stateIndex = 1;
                    else
                        stateIndex = 2;                                          //离开
                }
            }
        }
    }

    /// <summary>
    /// 检测是否达到赏花条件
    /// </summary>
    private ParterreEntity CheckAdmireFlowers()
    {
        ParterreEntity flower = null;
        List<ParterreEntity> flowerList = FacilitiesManager.Instance.GetAllShowingParterre();
        if (flowerList != null && flowerList.Count > 0)
        {
            for (int i = 0; i < flowerList.Count; i++)
            {
                if (flowerList[i].IsInSight && !flowerList[i].HaveGuest)
                {
                    flower = flowerList[i];
                    flower.HaveGuest = true;
                    break;
                }
            }
        }

        return flower;
    }
}
