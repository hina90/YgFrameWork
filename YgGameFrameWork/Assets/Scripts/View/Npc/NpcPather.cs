using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/// <summary>
/// 寻路器
/// </summary>
public class NpcPather:BaseBeheviour
{
    private Path path;
    private AILerp aiLerp;
    private Seeker seeker;
    private int currentWaypoint = 0;
    public float nextWaypointDistance = 0.2f;
    private bool reachedEndOfPath = false;
    public float repathRate = 0.5f;
    private float lastRepath = float.NegativeInfinity;

    private GameObject gameObject;

    /// <summary>
    /// 初始化结构函数
    /// </summary>
    protected void Initialize(GameObject gameObject)
    {
        this.gameObject = gameObject;
        seeker = this.gameObject.GetComponent<Seeker>();
        aiLerp = this.gameObject.GetComponent<AILerp>();
    }
    /// <summary>
    /// 获取路径点
    /// </summary>
    /// <param name="p"></param>
    void OnPathComplete(Path p)
    {
        p.Claim(this);
        if (!p.error)
        {
            if (path != null) path.Release(this);
            path = p;
            currentWaypoint = 0;
        }
        else
        {
            p.Release(this);
        }
    }

    /// <summary>
    /// 
    /// 寻找路径
    /// </summary>
    public void FindPath(int npcId, Vector3 position)
    {
        if (Time.unscaledTime > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.unscaledTime;
            var npcMgr = ManagementCenter.GetManager<NpcManager>();
            NpcView npcView = npcMgr.GetNpc(npcId) as NpcView;
            seeker.StartPath(npcView.gameObject.transform.position, position, OnPathComplete);
        }
    }
    /// <summary>
    /// 朝向
    /// </summary>
    /// <returns></returns>
    public bool FaceDirection()
    {
        reachedEndOfPath = false;
        if (null != path)
        {
            float distanceToWaypoint;
            while (true)
            {
                distanceToWaypoint = Vector3.Distance(gameObject.transform.position, path.vectorPath[currentWaypoint]);
                if (distanceToWaypoint < nextWaypointDistance)
                {
                    if (currentWaypoint + 1 < path.vectorPath.Count)
                    {
                        currentWaypoint++;
                    }
                    else
                    {
                        reachedEndOfPath = true;
                        break;
                    }
                }
                else break;
            }
            Vector2 direction = (path.vectorPath[currentWaypoint] - gameObject.transform.position).normalized;
            //actor.RunFaceDirection(direction, reachedEndOfPath);
        }

        return reachedEndOfPath;
    }
    /// <summary>
    /// 是否到达目标点
    /// <returns></returns>
    public bool IsReached()
    {
        return aiLerp.reachedEndOfPath;
    }
    /// <summary>
    /// 设置速度
    /// </summary>
    /// <param name="speed"></param>
    public float MoveSpeed
    {
        get { return aiLerp.speed; }
        set { aiLerp.speed = value; }
    }
}
