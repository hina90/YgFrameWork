using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 队列
/// </summary>
public class QueueUp : MonoBehaviour
{
    private Queue<BaseActor> queue = new Queue<BaseActor>();

    private float currentTime = 0f;
    private const float WAITIME = 0.4f;

    /// <summary>
    /// 添加顾客
    /// </summary>
    /// <param name="id"></param>
    public void AddToQueueList(BaseActor actor)
    {
        if(!queue.Contains(actor))
            queue.Enqueue(actor);
    }
    /// <summary>
    /// 获取顾客
    /// </summary>
    public BaseActor RemoveFromQueueList()
    {
        if(Count() > 0)
        {
            return queue.Dequeue();
        }

        return null;
    }
    /// <summary>
    /// 获取排队点
    /// </summary>
    public Vector3 GetQueuePosition(BaseActor actor)
    {
        Vector3 pos = transform.position;
        BaseActor[] list = queue.ToArray();
        for (int i = list.Length - 1; i > 0; i--)
        {
            if (list[i] == actor)
            {
                pos = GetQueuePosition(i);
                break;
            }
        }
        return pos;
    }
    /// <summary>
    /// 获取排队坐标
    /// </summary>
    protected virtual Vector3 GetQueuePosition(int i)
    {
        return default;
    }
    /// <summary>
    /// 获取队列数量
    /// </summary>
    /// <returns></returns>
    public int Count()
    {
        return queue.Count;
    }
    /// <summary>
    /// 监听排队
    /// </summary>
    private void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= WAITIME)
        {
            currentTime = 0;
            QueueUpdate();
        }
    }

    protected virtual void QueueUpdate()
    {

    }
}
