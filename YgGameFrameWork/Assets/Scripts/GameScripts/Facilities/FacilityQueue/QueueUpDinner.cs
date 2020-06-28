using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 用餐列队
/// </summary>
public class QueueUpDinner : QueueUp
{
    protected override void QueueUpdate()
    {
        if (!GameManager.Instance.IsDinnerDeskBusy())
        {
            BaseActor actor = RemoveFromQueueList();
            if (null != actor)
            {
                actor.AiController.SetTransition(Transition.QueueUpDinnerOver);
            }
        }
    }
    /// <summary>
    /// 获取用餐排队坐标
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    protected override Vector3 GetQueuePosition(int i)
    {
        return new Vector3(transform.position.x + (i + 1) * 0.03f, transform.position.y + i * 0.3f, 0);
    }
}
