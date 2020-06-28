using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 客人吃饭次数数据储存
/// </summary>
/// 
[System.Serializable]
public class GuestHaveDinnerStoreData 
{
    public Dictionary<int, int> haveDinnerTimeDic = new Dictionary<int, int>();
}
