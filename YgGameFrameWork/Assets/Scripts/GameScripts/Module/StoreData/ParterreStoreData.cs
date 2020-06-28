using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParterreStoreData
{
    /// <summary>
    /// 花圃状态
    /// </summary>
    public ParterreStatus parterreStatus;
    /// <summary>
    /// 花圃当前状态剩余时间
    /// </summary>
    public int remainTime;
    /// <summary>
    /// 花品种子Id
    /// </summary>
    public int flowerId;
    /// <summary>
    /// 顾客可观赏次数
    /// </summary>
    public int visitTimes;
    /// <summary>
    /// 剩余顾客观赏次数
    /// </summary>
    public int remainVisitTimes;
}
