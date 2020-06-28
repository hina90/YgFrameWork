using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

/// <summary>
/// 特效数据类
/// </summary>
public class EffectVO
{
    private string sourceName;
    private string aniName;
    private bool loop;
    private int effectType;
    private Vector3 startVector;
    private Vector3 endVector;
    private Transform startTrans;
    private Transform endTrans;
    private Transform startTranName;
    private Transform endTranName;
    private Transform target;
    private float speed;
    private float lifeTime;
    private float scale;
    public bool isZero = false;                             //判断传入startVector没有

    private Callback<bool> arriveCallback;                  //到达目标后的回调函数

    public string SourceName { get; set; }
    public string AniName { get; set; }
    public bool Loop { get; set; }
    public int EffectType { get; set; }
    public Vector3 StartVector { get; set; }
    public Vector3 EndVector { get; set; }
    public Transform StartTrans { get; set; }
    public Transform EndTrans { get; set; }
    public string StartTranName { get; set; }
    public string EndTranName { get; set; }
    public Transform Target { get; set; }
    public float Speed { get; set; }
    public float LifeTime { get; set; }
    public float Scale { get; set; }
    public Callback<bool> ArriveCallback { get; set; }
    public bool IsZero { get; set; }
}
