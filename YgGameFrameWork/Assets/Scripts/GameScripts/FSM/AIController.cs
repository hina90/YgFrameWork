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
    private BaseActor actor;
    public float speed = 0;
    private int currentWaypoint = 0;
    public float nextWaypointDistance = 0.3f;
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
    /// 寻找路径
    /// </summary>
    public bool FindPath(Vector3 position)
    {
        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;
            seeker.StartPath(rb.position, position, OnPathComplete);
        }
        if (path == null)
            return false;

        reachedEndOfPath = false;
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
        //Vector2 force = direction * speed * Time.deltaTime;
        //rb.AddForce(force);

        if (direction.x >= 0.01f)
            actor.FaceDirection(new Vector3(-1, 1, 1));
        else if(direction.x <= -0.01f)
            actor.FaceDirection(new Vector3(1, 1, 1));

        return reachedEndOfPath;
    }
    /// <summary>
    /// 是否到达目标点
    /// </summary>
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
        CurrentState.Reason(actor);
    }
    /// <summary>
    /// FSM基类中FixedUpdate中调用
    /// </summary>
    private float currentTime = 0;
    public override void FSMFixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        if (currentTime >= 0.2f)
            //确认当前状态发生的行为
            CurrentState.Act(actor);
    }
    /// <summary>
    /// 此方法在每个状态类的Reason方法中被调用
    /// </summary>
    /// <param name="t"></param>
    public void SetTransition(Transition t, int stateIndex = 0)
    {
        PerformTransition(t, stateIndex);
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
