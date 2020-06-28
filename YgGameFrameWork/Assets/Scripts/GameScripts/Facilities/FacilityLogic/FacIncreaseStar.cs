using System.Collections;
using System.Collections.Generic;
using Tool.Database;
using UnityEngine;

/// <summary>
/// 增加星级
/// </summary>
public class FacIncreaseStar : Facilities
{
    public FacIncreaseStar(int facilityId) : base(facilityId)
    {
    }

    public override void PutIntoUse(FacilitiesItemData data, float[] args)
    {
        //增加星级
        base.PutIntoUse(data, args);
        UIManager.Instance.SendUIEvent(GameEvent.UPDATE_STAR, (int)args[0]);
    }
}
