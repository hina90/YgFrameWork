using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Tool.Database;

/// <summary>
/// 角色基类
/// </summary>
public class BaseActor : SpineActor
{
    /// <summary>
    /// 最大消费次数
    /// </summary>
    public const int MAX_CONSUPTION = 2;
    public const int MAX_RANDOMMOVE = 3;
    /// <summary>
    /// 角色唯一ID
    /// </summary>
    public static int ID = 10100;
    /// <summary>
    /// 角色唯一ID
    /// </summary>
    public int ActorID { get; set; }
    /// <summary>
    /// 消费次数
    /// </summary>
    public int ConsumptionNumber { get; set; }
    /// <summary>
    /// 随机移动次数
    /// </summary>
    public int RandomMoveNumber { get; set; }
    /// <summary>
    /// 大胃王点餐次数
    /// </summary>
    public int TakeDinnerRepeatNumber { get; set; }
    /// <summary>
    /// AI控制器
    /// </summary>
    public AIController AiController { get; set; }
    /// <summary>
    /// 开始操作
    /// </summary>
    public bool StartOperation { get; set; }
    /// <summary>
    /// 配置档数据
    /// </summary>
    public CustomerConfigData ConfigData { get; set; }

    /// <summary>
    /// 初始化角色数据
    /// </summary>
    public override void Init()
    {
        base.Init();
        StartOperation = false;
        ActorID = BaseActor.ID++;

        InitAIController();
        ConsumptionNumber = 0;
        AiController.MoveSpeed = 0.87f;

        gameObject.AddComponent<SortLayerCom>();
    }
    /// <summary>
    /// 初始化AI
    /// </summary>
    protected virtual void InitAIController()
    {
        
    }
    /// <summary>
    /// 进入触发
    /// </summary>
    private void OnTriggerStay2D(Collider2D collision)
    {
        //GameManager.Instance.AddToSortList(transform);
    }
    /// <summary>
    /// 退出触发
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        //GameManager.Instance.RemoveFromSortList(transform);
    }
    /// <summary>
    /// 设置朝向
    /// </summary>
    public void FaceDirection(Vector3 vec)
    {
        transform.localScale = vec;
    }

    protected virtual void Update()
    {
        
    }
    /// <summary>
    /// 释放
    /// </summary>
    public virtual void Release()
    {
        AiController.Release();
        AiController = null;
    }
}
