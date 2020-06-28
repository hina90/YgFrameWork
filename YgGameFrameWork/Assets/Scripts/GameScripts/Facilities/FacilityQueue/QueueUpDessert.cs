using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 甜品台队列
/// </summary>
public class QueueUpDessert : QueueUp
{
    protected override void QueueUpdate()
    {
        if (!GameManager.Instance.IsDessertTableBusy())
        {
            BaseActor actor = RemoveFromQueueList();
            if (null != actor)
            {
                actor.AiController.SetTransition(Transition.QueueUpDessertOver);
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
        return new Vector3(transform.position.x + (i + 1) * 0.2f, transform.position.y + (i + 1) * 0.1f, 0);
    }
}
