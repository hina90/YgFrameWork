using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Tool.Database;
using UnityEngine;

///// <summary>
///// 设施类型
///// </summary>
//public enum FacilitiesType
//{
//    Restaurant = 1,         //餐厅
//    Kitchen,                //厨房
//    Garden,                 //花园
//    Hotel,                  //酒店
//    ConvenienceStore,       //便利店
//    AmusementPark           //游乐园
//}

public interface IFacilities
{
    void PutIntoUse(FacilitiesItemData data, float[] args);
}

/// <summary>
/// 设施基类
/// </summary>
public class Facilities : IFacilities
{
    private FacilityModule facilityModule;
    protected FacilityModule FacModule
    {
        get
        {
            if (facilityModule == null)
            {
                facilityModule = GameModuleManager.Instance.GetModule<FacilityModule>();
            }
            return facilityModule;
        }
    }
    private PlayerModule playerModule;
    protected PlayerModule PlayerModule
    {
        get
        {
            if (playerModule == null)
            {
                playerModule = GameModuleManager.Instance.GetModule<PlayerModule>();
            }
            return playerModule;
        }
    }

    protected FacilitiesItemData data;
    protected float[] args;
    protected int facilityId;

    public Facilities(int facilityId)
    {
        this.facilityId = facilityId;
    }

    /// <summary>
    /// 设施加成逻辑
    /// </summary>
    public virtual void PutIntoUse(FacilitiesItemData data, float[] args)
    {
        this.data = data;
        this.args = args;
    }
}
