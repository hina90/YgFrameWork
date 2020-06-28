using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotionStoreBuyState : FSMState
{

    //状态索引
    private int stateIndex = 0;
    //花盆
    private ShelfEntity shelf;
    //偏移量
    private Vector3 offSetVec = new Vector3(0f, 0.7f, 0);
    protected float buyWaitime = 3.5f;           //购物时间
    protected float waitCurrentime = 0f;         //当前等待时间

    public NotionStoreBuyState()
    {
        waiTime = 2f;
        stateID = FSMStateID.NotionStoreBuy;
    }

    ///// <summary>
    ///// 条件转换
    ///// </summary>
    ///// <param name="aiController"></param>
    ///// <param name="actor"></param>
    public override void Reason(BaseActor actor)
    {
        if (ChangeState)
        {
            if (null != shelf)
            {
                shelf.HaveGuest = false;
                shelf = null;
                waitCurrentime = 0;
            }
            actor.AiController.SetTransition(Transition.NotionBuyOver, stateIndex);
        }
    }
    /// <summary>
    /// 行为
    /// </summary> 
    /// <param name="aiController"></param>
    /// <param name="actor"></param>
    public override void Act(BaseActor actor)
    {
        if (null == shelf)
            shelf = CheckGoodsShelf();
        if (null == shelf)
        {
            stateIndex = 0;
            ChangeState = true;
            return;
        }

        actor.AiController.FindPath(shelf.transform.position + offSetVec);
        moveOver = actor.AiController.IsReached();

        currenTime += Time.fixedDeltaTime;
        if (currenTime >= waiTime)
        {
            moveOver = actor.AiController.IsReached();
            if (moveOver)
            {
                waitCurrentime += Time.fixedDeltaTime;
                if (waitCurrentime >= buyWaitime)
                {
                    shelf.Sales();
                    stateIndex = 1;
                    waitCurrentime = 0;
                    ChangeState = true;
                }
            }
        }
    }
    /// <summary>
    /// 检测是否达到赏花条件
    /// </summary>
    private ShelfEntity CheckGoodsShelf()
    {
        ShelfEntity shelf = null;
        List<ShelfEntity> shelfList = FacilitiesManager.Instance.GetAllSellingShelf();
        if (shelfList != null && shelfList.Count > 0)
        {
            for (int i = 0; i < shelfList.Count; i++)
            {
                if (!shelfList[i].HaveGuest)
                {
                    shelf = shelfList[i];
                    shelf.HaveGuest = true;
                    break;
                }
            }
        }

        return shelf;
    }
}
