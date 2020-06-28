using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 提高货物可售卖的次数
/// </summary>
public class FacIncreaseGoodsSoldTimes : Facilities
{
    public FacIncreaseGoodsSoldTimes(int facilityId) : base(facilityId)
    {
    }

    public override void PutIntoUse(FacilitiesItemData data, float[] args)
    {
        base.PutIntoUse(data, args);
        FacModule.AddFacilityIncomeValue(facilityId, args[0]);
    }
}
