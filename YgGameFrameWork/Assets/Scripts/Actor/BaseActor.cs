using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Tool.Database;

/// <summary>
/// 角色基类
/// </summary>
public class BaseActor : MonoBehaviour
{
    /// <summary>
    /// 角色唯一ID
    /// </summary>
    public static int ID = 10100;
    /// <summary>
    /// 角色唯一ID
    /// </summary>
    public int ActorID { get; set; }
    /// <summary>
    /// 目标
    /// </summary>
    public Transform Target { get; set; }

    
    protected Vector3 offPos = new Vector3(0, 0, 0);


    /// <summary>
    /// 初始化AI
    /// </summary>
    protected virtual void InitAIController()
    {

    }
   
  
    protected virtual void Update()
    {

    }
    /// <summary>
    /// 释放
    /// </summary>
    public virtual void Release()
    {
    }
}
