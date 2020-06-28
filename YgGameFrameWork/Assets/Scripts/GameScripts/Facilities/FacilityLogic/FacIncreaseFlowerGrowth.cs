using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 促进花类成长
/// </summary>
public class FacIncreaseFlowerGrowth : Facilities
{
    private GardenModule gardenModule;
    public FacIncreaseFlowerGrowth(int facilityId) : base(facilityId)
    {
        gardenModule = GameModuleManager.Instance.GetModule<GardenModule>();
    }

    public override void PutIntoUse(FacilitiesItemData data, float[] args)
    {
        base.PutIntoUse(data, args);
        gardenModule.GrowthRateBonus += args[0];
    }
}
