using System.Collections;
using System.Collections.Generic;
using Tool.Database;
using UnityEngine;

/// <summary>
/// 提高做菜效率
/// </summary>
public class FacIncreaseCookRatio : Facilities
{
    private MenuConfigData menuData;

    public FacIncreaseCookRatio(int facilityId) : base(facilityId)
    {
    }

    public override void PutIntoUse(FacilitiesItemData data, float[] args)
    {
        base.PutIntoUse(data, args);
        FacModule.AddFacilityIncomeValue(data.facilitiesConfigData.id, args[0]);
    }
}
