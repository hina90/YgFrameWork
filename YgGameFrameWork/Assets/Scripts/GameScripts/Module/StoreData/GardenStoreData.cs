using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GardenStoreData
{
    /// <summary>
    /// 许愿池许愿次数
    /// </summary>
    public int wishTimes;
    /// <summary>
    /// 许愿池剩余许愿次数
    /// </summary>
    public int remainWishTimes;
    /// <summary>
    /// 许愿池物品掉落个数
    /// </summary>
    public int wishRewardNum;
    /// <summary>
    /// 许愿池剩余重置时间
    /// </summary>
    public int remainResetTime;
    /// <summary>
    /// 许愿池下一次可许愿时间
    /// </summary>
    public DateTime nextWishTime;
    /// <summary>
    /// 花园花朵信息
    /// </summary>
    public Dictionary<int, ParterreStoreData> parterreDataDic;
    /// <summary>
    /// 许愿间隔时间
    /// </summary>
    public int[] wishIntervalTime;
    /// <summary>
    /// 变异花朵剩余CD冷却时间
    /// </summary>
    public int mutationCDTime;
    /// <summary>
    /// 成长效率加成
    /// </summary>
    public float growthRateBonus;
    /// <summary>
    /// 等待变异的花圃Id
    /// </summary>
    public int waitVariationId;
}
