using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 顾客观赏后获得额外收入
/// </summary>
public class FacVisitIncome : Facilities
{
    //花园数据模块
    private GardenModule gardenModule;

    public FacVisitIncome(int facilityId) : base(facilityId)
    {
        gardenModule = GameModuleManager.Instance.GetModule<GardenModule>();
    }

    public override void PutIntoUse(FacilitiesItemData data, float[] args)
    {
        base.PutIntoUse(data, args);
        //累计客人观赏次数
        gardenModule.AddVisitCount(facilityId, (int)args[0]);
        //累计顾客参观额外收入
        FacModule.AddFacilityIncomeValue(facilityId, args[1]);
    }
}
