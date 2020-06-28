using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 提高便利店上货的效率
/// </summary>
public class FacIncreaseLoadRate : Facilities
{
    private StoreModule storeModule;
    public FacIncreaseLoadRate(int facilityId) : base(facilityId)
    {
        storeModule = GameModuleManager.Instance.GetModule<StoreModule>();
    }

    public override void PutIntoUse(FacilitiesItemData data, float[] args)
    {
        base.PutIntoUse(data, args);
        storeModule.LoadRate += args[0];
    }
}
