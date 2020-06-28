using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 咖啡队列
/// </summary>
public class QueueUpCoffee : QueueUp
{
    protected override void QueueUpdate()
    {
        if (!GameManager.Instance.IsCoffeeMakerBusy())
        {
            BaseActor actor = RemoveFromQueueList();
            if(null != actor)
            {
                Debug.Log("----------------------------CoffeeQueue  Over-------------------------");
                actor.AiController.SetTransition(Transition.QueueUpCoffeeOver);
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
        return new Vector3(transform.position.x - (i + 1) * 0.2f, transform.position.y - (i + 1) * 0.1f, 0);
    }
}
