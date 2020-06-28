using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 花园随机移动状态
/// </summary>
public class GardenRandomMoveState : RandomWalkState
{
    private const int MOVE_TIMES = 2;                    //移动次数
    private int currentMoveTime = 0;                     //当前移动次数

    public GardenRandomMoveState()
    {
        waiTime = 2f;
        stateID = FSMStateID.GardenRandomMove;
    }
    ///// <summary>
    ///// 随机移动条件转换
    ///// </summary>
    ///// <param name="aiController"></param>
    ///// <param name="actor"></param>
    public override void Reason(BaseActor actor)
    {
        if(ChangeState)
        {
            //Debug.Log("-----------------------便利店随机移动转换到  购买状态------------------------");
            actor.AiController.SetTransition(Transition.GardenRandomMoveOver, 0);
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

    protected override void ActOverHandle(BaseActor actor)
    {
        if(currentMoveTime >= MOVE_TIMES)
        {
            actor.AiController.SetTransition(Transition.GardenRandomMoveOver, 1);
            return;
        }
        if(CheckAdmireFlowers())
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
        return GameManager.Instance.GetGardenRandomMovePoint();
    }
    /// <summary>
    /// 检测是否达到赏花条件
    /// </summary>
    private bool CheckAdmireFlowers()
    {
        bool isFlower = false;
        List<ParterreEntity>  flowerList = FacilitiesManager.Instance.GetAllShowingParterre();
        if(flowerList != null)
        {
            for(int i = 0; i < flowerList.Count; i++)
            {
                if (flowerList[i].IsInSight && !flowerList[i].HaveGuest)
                {
                    isFlower = true;
                    break;
                }
            }
        }

        return isFlower;
    }
}
