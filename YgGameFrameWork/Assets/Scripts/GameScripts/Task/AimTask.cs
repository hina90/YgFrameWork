using System.Collections;
using System.Collections.Generic;
using Tool.Database;
using UnityEngine;

public enum AimTaskType
{
    BuyFacility,
    ClickBtn,
    LearnMenu,
    UnlockCustomer,
    UnlockArea,
    Planting,
    UpperShelf,
}

/// <summary>
/// 引导任务链接区域
/// </summary>Restra
public enum AimTaskAreaType
{
    RestaurantPanel = 1,
    KitchenPanel,
    StorePanel,
    GardenPanel,
    MenuPanel,
    CustomerPanel,
    GoodsPanel,
    RestaurantArea,
    GardenArea,
    StoreArea
}

public class AimTask : TaskBase
{
    private AimTaskModule aimTaskModule;
    //一级餐桌设施
    private readonly List<int> OneLevelTable = new List<int>() { 1005, 1009, 1013, 1017, 1021, 1025 };
    //一级炉灶设施
    private readonly List<int> OneLevelStove = new List<int>() { 1057, 1061, 1065, 1069, 1073, 1077 };
    //一级货架设施
    private readonly List<int> OneLevelShelf = new List<int>() { 1189, 1193, 1197, 1201, 1205, 1209 };

    public AimTask(AimTaskModule aimTaskModule)
    {
        this.aimTaskModule = aimTaskModule;
    }

    /// <summary>
    /// 更新引导任务进度
    /// </summary>
    public void UpdateAimTaskProgress(AimTaskType aimTaskType, object arg)
    {
        switch (aimTaskType)
        {
            case AimTaskType.BuyFacility:
                BuyFacility(arg);
                break;
            case AimTaskType.ClickBtn:
                BtnClick(arg);
                break;
            case AimTaskType.LearnMenu:
                LearnMenu(arg);
                break;
            case AimTaskType.UnlockCustomer:
                UnlockCustomer(arg);
                break;
            case AimTaskType.UnlockArea:
                UnlockArea(arg);
                break;
            case AimTaskType.Planting:
                Planting(arg);
                break;
            case AimTaskType.UpperShelf:
                UpperShelf(arg);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 购买新设施
    /// </summary>
    /// <param name="facilityId"></param>
    private void BuyFacility(object arg)
    {
        FacilitiesItemData facilitiesData = (FacilitiesItemData)arg;
        ///小费台任务
        if (facilitiesData.itemId == 1001)
        {
            aimTaskModule.UpdateTask(1001);
        }
        ///购买一级餐桌任务
        if (OneLevelTable.Contains(facilitiesData.itemId))
        {
            aimTaskModule.UpdateTask(1004);
        }
        ///购买一级炉灶任务
        if (OneLevelStove.Contains(facilitiesData.itemId))
        {
            aimTaskModule.UpdateTask(1006);
        }
        ///解锁餐厅所有一级设施任务
        if (facilitiesData.facilitiesConfigData.type == 1 && facilitiesData.itemConfigData.level == 1)
        {
            aimTaskModule.UpdateTask(1008);
        }
        ///解锁厨房所有一级设施任务
        if (facilitiesData.facilitiesConfigData.type == 2 && facilitiesData.itemConfigData.level == 1)
        {
            aimTaskModule.UpdateTask(1009);
        }
        ///购买许愿池任务
        if (facilitiesData.facilitiesConfigData.id == 10030)
        {
            aimTaskModule.UpdateTask(1012);
        }
        ///购买茉莉盆栽任务
        if (facilitiesData.itemId == 1125)
        {
            aimTaskModule.UpdateTask(1013);
        }
        ///解锁8个一级花园设施任务
        if (facilitiesData.facilitiesConfigData.type == 3 && facilitiesData.itemConfigData.level == 1)
        {
            aimTaskModule.UpdateTask(1015);
        }
        ///购买三个一级货架任务
        if (OneLevelShelf.Contains(facilitiesData.itemId))
        {
            aimTaskModule.UpdateTask(1017);
        }
        ///解锁8个一级便利店
        if (facilitiesData.facilitiesConfigData.type == 4 && facilitiesData.itemConfigData.level == 1)
        {
            aimTaskModule.UpdateTask(1019);
        }
    }

    /// <summary>
    /// 按钮点击任务
    /// </summary>
    private void BtnClick(object arg)
    {
        string btnName = (string)arg;
        switch (btnName)
        {
            case "BtnGainx2":
                aimTaskModule.UpdateTask(1002);
                break;
            case "BtnGain":
                aimTaskModule.UpdateTask(1002);
                break;
            case "WatchBtn":
                aimTaskModule.UpdateTask(1007);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 学习新菜谱
    /// </summary>
    private void LearnMenu(object arg)
    {
        int menuId = (int)arg;
        if (menuId == 2003)
        {
            aimTaskModule.UpdateTask(1003);
        }
        aimTaskModule.UpdateTask(1005);
    }

    /// <summary>
    /// 解锁新顾客
    /// </summary>
    private void UnlockCustomer(object arg)
    {
        int customerId = (int)arg;
        aimTaskModule.UpdateTask(1010);
    }

    /// <summary>
    /// 解锁新区域
    /// </summary>
    private void UnlockArea(object arg)
    {
        int areaId = (int)arg;
        switch (areaId)
        {
            case 1003:
                aimTaskModule.UpdateTask(1011);
                break;
            case 1004:
                aimTaskModule.UpdateTask(1016);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 种植
    /// </summary>
    private void Planting(object arg)
    {
        aimTaskModule.UpdateTask(1014);
    }

    /// <summary>
    /// 上货
    /// </summary>
    private void UpperShelf(object arg)
    {
        aimTaskModule.UpdateTask(1018);
    }
}
