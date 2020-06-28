using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 杂货店随机移动状态
/// </summary>
public class NotionStoreRandomMoveState : RandomWalkState
{
    private const int MOVE_TIMES = 2;                    //移动次数
    private int currentMoveTime = 0;                     //当前移动次数

    public NotionStoreRandomMoveState()
    {
        waiTime = 2f;
        stateID = FSMStateID.NotionStoreRandomMove;
    }
    ///// <summary>
    ///// 随机移动条件转换
    ///// </summary>
    ///// <param name="aiController"></param>
    ///// <param name="actor"></param>
    public override void Reason(BaseActor actor)
    {
        if (ChangeState)
        {
            actor.AiController.SetTransition(Transition.NotionRandomMoveOver, 0);
        }
    }
    /// <summary>
    /// 随机移动行为
    /// </summary>
    /// <param name="aiController"></param>
    /// <param name="actor"></param>
    public override void Act(BaseActor actor)
    {
        RandomMove(actor);
    }
    /// <summary>
    /// 移动玩后行为
    /// </summary>
    /// <param name="actor"></param>
    protected override void ActOverHandle(BaseActor actor)
    {
        if (currentMoveTime >= MOVE_TIMES)
        {
            actor.AiController.SetTransition(Transition.NotionRandomMoveOver, 1);
            return;
        }
        Debug.Log(CheckGoodsShelf());
        if (CheckGoodsShelf())
        {
            ChangeState = true;
        }
        else
        {
            currentMoveTime++;
            RandomMove(actor);
        }
    }
    /// <summary>
    /// 获取随机移动点位置
    /// </summary>
    /// <returns></returns>
    protected override Transform GetRandomMovePoint()
    {
        return GameManager.Instance.GetNotionStoreRandomMovePoint();
    }
    /// <summary>
    /// 检测是否可以购物
    /// </summary>
    private bool CheckGoodsShelf()
    {
        bool canBuy = false;
        List<ShelfEntity> goodsShelfList = FacilitiesManager.Instance.GetAllSellingShelf();
        if (goodsShelfList != null)
        {
            for (int i = 0; i < goodsShelfList.Count; i++)
            {
                if (!goodsShelfList[i].HaveGuest)
                {
                    canBuy = true;
                    break;
                }
            }
        }

        return canBuy;
    }
}
