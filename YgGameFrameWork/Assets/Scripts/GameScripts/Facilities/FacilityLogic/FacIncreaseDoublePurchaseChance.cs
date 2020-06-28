using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 提高顾客加倍购买的概率
/// </summary>
public class FacIncreaseDoublePurchaseChance : Facilities
{
    private StoreModule storeModule;
    public FacIncreaseDoublePurchaseChance(int facilityId) : base(facilityId)
    {
        storeModule = GameModuleManager.Instance.GetModule<StoreModule>();
    }

    public override void PutIntoUse(FacilitiesItemData data, float[] args)
    {
        base.PutIntoUse(data, args);
        //刷新加倍购买倍数
        storeModule.Times = (int)args[0];
        //累计加倍购买概率
        storeModule.DoublePurchaseChance += args[1];
    }
}
