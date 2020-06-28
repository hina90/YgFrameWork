using System.Collections;
using System.Collections.Generic;
using Tool.Database;
using UnityEngine;

/// <summary>
/// 客人每次消费后获取收入
/// </summary>
public class FacIncomeAfterConsume : Facilities
{
    public FacIncomeAfterConsume(int facilityId) : base(facilityId)
    {
    }

    public override void PutIntoUse(FacilitiesItemData data, float[] args)
    {
        base.PutIntoUse(data, args);
        FacModule.AddFacilityIncomeValue(data.facilitiesConfigData.id, args[0]);
    }
}
