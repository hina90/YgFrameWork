using System.Collections;
using System.Collections.Generic;
using Tool.Database;
using UnityEngine;

/// <summary>
/// 增加小费收入上限
/// </summary>
public class FacIncreaseToplimit : Facilities
{
    public FacIncreaseToplimit(int facilityId) : base(facilityId)
    {
    }

    public override void PutIntoUse(FacilitiesItemData data, float[] args)
    {
        base.PutIntoUse(data, args);
        FacModule.MaxGratuity += (int)args[0];
    }
}
