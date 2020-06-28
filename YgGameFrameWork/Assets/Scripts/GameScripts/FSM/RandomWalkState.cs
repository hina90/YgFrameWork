using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 随机移动状态
/// </summary>
public class RandomWalkState : FSMState
{
    //随机移动点
    protected Transform moveTrans;
    protected float randomWaitime = 0;
    protected float waitCurrentime = 0;

    ///// <summary>
    ///// 随机移动条件转换
    ///// </summary>
    ///// <param name="aiController"></param>
    ///// <param name="actor"></param>
    public override void Reason(BaseActor actor)
    {

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
    /// 随机移动
    /// </summary>
    /// <param name="actor"></param>
    protected void RandomMove(BaseActor actor) 
    {
        if (null == moveTrans)
        {
            randomWaitime = Random.Range(0.8f, 1.5f);
            moveTrans = GetRandomMovePoint();
            targetPosition = moveTrans.position;
        }

        actor.AiController.FindPath(targetPosition);
        moveOver = actor.AiController.IsReached();

        currenTime += Time.fixedDeltaTime;
        if (currenTime >= waiTime)
        {
            moveOver = actor.AiController.IsReached();
            if (moveOver)
            {
                waitCurrentime += Time.fixedDeltaTime;
                if (waitCurrentime >= randomWaitime)
                {
                    currenTime = 0;
                    waitCurrentime = 0;
                    moveTrans = null;
                    ActOverHandle(actor);
                }
            }
        }
    }
    /// <summary>
    /// 行动完成后行为
    /// </summary>
    protected virtual void ActOverHandle(BaseActor actor)
    {

    }

    protected virtual Transform GetRandomMovePoint()
    {
        return GameManager.Instance.GetRandomMovePoint();
    }
}
