using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 固定时间额外收入
/// </summary>
public class ExtraIncomeEntity : FacilityEntity
{
    private float curTime = 0;

    private void Update()
    {
        curTime += Time.deltaTime;
        if (curTime >= facilitiesData.itemConfigData.funParam_2[0])
        {
            CreateDriedFish((int)facilityModule.FacilityAddValueCache[facilitiesData.facilitiesConfigData.id]);
            curTime = 0;
        }
    }
}
