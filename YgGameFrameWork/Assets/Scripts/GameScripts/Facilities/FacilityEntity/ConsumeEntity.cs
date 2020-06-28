using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消费设施实体
/// </summary>
public class ConsumeEntity : FacilityEntity
{
    /// <summary>
    /// 当前使用设施的客人Id
    /// </summary>
    public int UseGuestID { get; set; }
    /// <summary>
    /// 是否有客人
    /// </summary>
    public bool HaveGuest { get; set; }
    /// <summary>
    /// 顾客消费收入
    /// </summary>
    public void ConsumeIncome()
    {
        int value = (int)facilityModule.FacilityAddValueCache[facilitiesData.facilitiesConfigData.id];
        CreateDriedFish(value);
    }

    public override void ClickEffect()
    {
#if UNITY_EDITOR
        ConsumeIncome();
#endif
    }
}
