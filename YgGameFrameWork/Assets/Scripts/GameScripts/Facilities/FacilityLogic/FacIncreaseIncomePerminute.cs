using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 增加每分钟小费收入N点
/// </summary>
public class FacIncreaseIncomePerminute : Facilities
{
    public FacIncreaseIncomePerminute(int facilityId) : base(facilityId)
    {
    }

    public override void PutIntoUse(FacilitiesItemData data, float[] args)
    {
        base.PutIntoUse(data, args);
        FacModule.GratuityPerMinute += (int)args[0];
    }
}
