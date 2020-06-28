using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FacilityStoreData
{
    /// <summary>
    /// 小费上限
    /// </summary>
    public int maxGratuity;
    /// <summary>
    /// 当前小费
    /// </summary>
    public int curGratuity;
    /// <summary>
    /// 每分钟小费收入
    /// </summary>
    public int gratuityPerMinute;
    /// <summary>
    /// 设施增益效果值
    /// </summary>
    public Dictionary<int, float> facilityAddValueCache;
}
