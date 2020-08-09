using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
/// <summary>
/// AI操控类
/// </summary>
public class AIController : AdvancedFSM
{
    private Path path;
    private AILerp aiLerp;
    private Seeker seeker;
    private Rigidbody2D rb;
    protected BaseActor actor;
    private int currentWaypoint = 0;
    public float nextWaypointDistance = 0.2f;
    private bool reachedEndOfPath = false;

    public float repathRate = 0.5f;
    private float lastRepath = float.NegativeInfinity;

    /// <summary>
    /// 初始化数据
    /// </summary>
    public override void Initialize(BaseActor actor)
    {
        this.actor = actor;
        
        ConstructFSM();
        ConstructCom();
    }
    /// <summary>
    /// 初始化结构函数
    /// </summary>
    private void ConstructCom()
    { 
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        aiLerp = GetComponent<AILerp>();
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
    public void FindPath(Transform trans)
    {
        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;
            seeker.StartPath(rb.position, trans.position, OnPathComplete);
        }
        //if (path == null)
        //    return false;

        //reachedEndOfPath = false;
        //float distanceToWaypoint;
        //while (true)
        //{
        //    distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        //    if (distanceToWaypoint < nextWaypointDistance)
        //    {
        //        if (currentWaypoint + 1 < path.vectorPath.Count)
        //        {
        //            currentWaypoint++;
        //        }
        //        else
        //        {
        //            reachedEndOfPath = true;
        //            break;
        //        }
        //    }
        //    else break;
        //}

        //Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        //actor.RunFaceDirection(direction, reachedEndOfPath, trans.name);

        //return reachedEndOfPath;
    }
    public bool FaceDirection(Transform trans)
    {
        reachedEndOfPath = false;
        if (null != path)
        {
            float distanceToWaypoint;
            while (true)
            {
                distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
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
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            //actor.RunFaceDirection(direction, reachedEndOfPath, trans.name);//设置朝向
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
    /// <summary>
    /// 停止寻路
    /// </summary>
    public void SetFindPathEnable(bool enable)
    {
        aiLerp.canMove = enable;
    }
    /// <summary>
    /// 构造FSM
    /// 注册AI各种状态 
    /// </summary>
    protected virtual void ConstructFSM()
    {
        
    }
    /// <summary>
    /// FSM基类中Update调用
    /// </summary>
    public override void FSMUpdate()
    {
        //确认当前状态发生的转换
        if (CurrentState == null)
            return;
        CurrentState.Reason(actor);
    }
    /// <summary> 
    /// FSM基类中FixedUpdate中调用
    /// </summary>
    private float currentTime = 0;
    public override void FSMFixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        //确认当前状态发生的行为
        if (currentTime >= 0.3f)
        {
            if (CurrentState == null)
                return;
            CurrentState.Act(actor);
        }
    }
    /// <summary>
    /// 此方法在每个状态类的Reason方法中被调用
    /// </summary>
    /// <param name="t"></param>
    //public void SetTransition(Transition t, int stateIndex = 0)
    //{
    //    PerformTransition(t, stateIndex);
    //}
    /// <summary>
    /// 状态转换方法
    /// </summary>
    /// <param name="stateId"></param>
    public void SetTransition(BaseActor actor, FSMStateID stateId)
    {
        PerformTransition(actor, stateId);
    }
    /// <summary>
    /// 销毁
    /// </summary>
    public void Release()
    {
        actor = null;
        fsmStates.Clear();
    }
}
