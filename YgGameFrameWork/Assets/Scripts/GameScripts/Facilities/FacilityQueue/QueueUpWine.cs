using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 酒柜队列
/// </summary>
public class QueueUpWine : QueueUp
{
    protected override void QueueUpdate()
    {
        if (!GameManager.Instance.IsWineCabinetBusy())
        {
            BaseActor actor = RemoveFromQueueList();
            if(null != actor)
            {
                actor.AiController.SetTransition(Transition.QueueUpWineOver);
            }
        }
    }
    /// <summary>
    /// 获取排队坐标
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    protected override Vector3 GetQueuePosition(int i)
    {
        return new Vector3(transform.position.x, transform.position.y - (i + 1) * 0.3f, 0);
    }
}
