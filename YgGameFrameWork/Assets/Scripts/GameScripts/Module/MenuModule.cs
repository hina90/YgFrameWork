using System.Collections;
using System.Collections.Generic;
using Tool.Database;
using UnityEngine;


/// <summary>
/// 设施类型
/// </summary>
public enum FacilitiesType
{
    Restaurant = 1,         //餐厅
    Kitchen,                //厨房
    Garden,                 //花园
    Store,                  //便利店
    Farm,                    //农场
    Cafe,                   //咖啡厅
}

public enum MenuType
{
    /// <summary>
    /// 现做
    /// </summary>
    Make = 1,
    /// <summary>
    /// 自助
    /// </summary>
    Self_help,
}

/// <summary>
/// 菜单模块
/// </summary>
public class MenuModule : BaseModule
{
    ///所有设施缓存
    private Dictionary<int, List<FacilitiesConfigData>> facilitiesCache;
    ///所有菜单缓存
    private Dictionary<int, List<MenuData>> menuCache;

    #region 持久化模块
    private Dictionary<int, FacilitiesStoreData> facilitiesStoreCache;
    private Dictionary<int, MenuStoreData> menuStoreCache;

    #endregion

    internal override void Init(GameModuleManager moduleManager)
    {
        base.Init(moduleManager);
        facilitiesCache = new Dictionary<int, List<FacilitiesConfigData>>();
        menuCache = new Dictionary<int, List<MenuData>>();
        ReadData();
        ParseConfig();
    }

    /// <summary>
    /// 解析数据表
    /// </summary>
    private void ParseConfig()
    {
        try
        {
            ///缓存设施表
            List<FacilitiesConfigData> facilitiesDatas = GetAllFacilities();
            for (int i = 0; i < facilitiesDatas.Count; i++)
            {
                if (!facilitiesCache.ContainsKey(facilitiesDatas[i].type))
                {
                    facilitiesCache[facilitiesDatas[i].type] = new List<FacilitiesConfigData>();
                }
                facilitiesCache[facilitiesDatas[i].type].Add(facilitiesDatas[i]);
            }

            ///缓存菜单表
            List<MenuConfigData> menuDatas = GetAllMenu();
            for (int i = 0; i < menuDatas.Count; i++)
            {
                if (!menuCache.ContainsKey(menuDatas[i].type))
                {
                    menuCache[menuDatas[i].type] = new List<MenuData>();
                }
                menuCache[menuDatas[i].type].Add(GetMenuData(menuDatas[i]));
            }
        }
        catch (System.Exception e)
        {
            TDDebug.DebugLogError(e.StackTrace);
        }
    }

    /// <summary>
    /// 获取指定区域类型的所有设施
    /// </summary>
    /// <param name="type">区域类型</param>
    /// <returns></returns>
    internal List<FacilitiesConfigData> GetFacilitiesListByType(FacilitiesType type)
    {
        int typeIndex = (int)type;
        if (facilitiesCache.ContainsKey(typeIndex))
        {
            return facilitiesCache[typeIndex];
        }
        else
        {
            TDDebug.DebugLogError("无该类型设施，请检查设备配置表!");
            return null;
        }
    }

    /// <summary>
    /// 获取指定类型的菜单列表
    /// </summary>
    /// <param name="menuType">菜单类型</param>
    /// <returns></returns>
    internal List<MenuData> GetMenuListByType(MenuType menuType)
    {
        int typeIndex = (int)menuType;
        if (menuCache.ContainsKey(typeIndex))
        {
            return menuCache[typeIndex];
        }
        else
        {
            TDDebug.DebugLogError("无该类型菜单，请检查菜单配置表!");
            return null;
        }
    }

    /// <summary>
    /// 获取具体设施数据
    /// </summary>
    /// <param name="facilitiesConfig">设施组Id</param>
    /// <param name="itemId">指定设施道具Id</param>
    /// <returns></returns>
    public FacilitiesItemData GetFacilitiesItemData(int facilitiesId, int itemId)
    {
        FacilitiesConfigData facilitiesConfig = ConfigDataManager.Instance.GetDatabase<FacilitiesConfigDatabase>().GetDataByKey(facilitiesId.ToString());
        return GetFacilitiesItemData(facilitiesConfig, itemId);
    }

    /// <summary>
    /// 获取具体设施数据
    /// </summary>
    /// <param name="facilitiesConfig">设施配置数据</param>
    /// <param name="itemId">指定设施Id</param>
    /// <returns></returns>
    public FacilitiesItemData GetFacilitiesItemData(FacilitiesConfigData facilitiesConfig, int itemId)
    {
        try
        {
            if (!facilitiesStoreCache.TryGetValue(facilitiesConfig.id, out FacilitiesStoreData storeData))
            {
                facilitiesStoreCache[facilitiesConfig.id] = new FacilitiesStoreData()
                {
                    facitiliesId = facilitiesConfig.id,
                    itemStoreDatas = new Dictionary<int, ItemStoreData>()
                    {
                        [facilitiesConfig.item_1] = new ItemStoreData(),
                        [facilitiesConfig.item_2] = new ItemStoreData(),
                        [facilitiesConfig.item_3] = new ItemStoreData(),
                        [facilitiesConfig.item_4] = new ItemStoreData(),
                    },
                };
                storeData = facilitiesStoreCache[facilitiesConfig.id];
            }
            FacilitiesItemData facilitiesItemData = new FacilitiesItemData()
            {
                itemConfigData = ConfigDataManager.Instance.GetDatabase<ItemConfigDatabase>().GetDataByKey(itemId.ToString()),
                storeData = storeData.itemStoreDatas[itemId],
                itemId = itemId,
                facilitiesConfigData = facilitiesConfig,
                pos = facilitiesConfig.buildPos,
            };
            return facilitiesItemData;
        }
        catch (System.Exception e)
        {
            //TDDebug.DebugLogError(e.StackTrace);
            return null;
        }
    }
    public FacilitiesItemData GetFacilitiesItemData(int itemId)
    {
        ItemConfigData itemConfigData = ConfigDataManager.Instance.GetDatabase<ItemConfigDatabase>().GetDataByKey(itemId.ToString());
        return GetFacilitiesItemData(itemConfigData.facId, itemConfigData.Id);
    }
    /// <summary>
    /// 获取指定菜单数据
    /// </summary>
    /// <param name="menuId">菜单Id</param>
    /// <returns></returns>
    public MenuData GetMenuData(int menuId)
    {
        if (!menuStoreCache.TryGetValue(menuId, out MenuStoreData menuStore))
        {
            menuStoreCache[menuId] = new MenuStoreData();
        }
        menuStore = menuStoreCache[menuId];
        MenuData menuData = new MenuData()
        {
            configData = ConfigDataManager.Instance.GetDatabase<MenuConfigDatabase>().GetDataByKey(menuId.ToString()),
            storeData = menuStore,
        };
        return menuData;
    }

    /// <summary>
    /// 获取指定菜单数据
    /// </summary>
    /// <param name="menuId">菜单配置数据</param>
    /// <returns></returns>
    public MenuData GetMenuData(MenuConfigData menuConfigData)
    {
        if (!menuStoreCache.TryGetValue(menuConfigData.Id, out MenuStoreData menuStore))
        {
            menuStoreCache[menuConfigData.Id] = new MenuStoreData();
            if (menuConfigData.unluckValue[0] == 0)
            {
                menuStoreCache[menuConfigData.Id].isStudy = true;
            }
        }
        menuStore = menuStoreCache[menuConfigData.Id];
        MenuData menuData = new MenuData()
        {
            configData = menuConfigData,
            storeData = menuStore,
        };
        return menuData;
    }

    /// <summary>
    /// 获取所有投入使用的设施数据
    /// </summary>
    internal List<FacilitiesItemData> GetAllUseFacilityItem()
    {
        List<FacilitiesItemData> facItemList = new List<FacilitiesItemData>();
        foreach (var fac in facilitiesStoreCache)
        {
            if (fac.Value.useItemId == 0) continue;
            facItemList.Add(GetFacilitiesItemData(fac.Key, fac.Value.useItemId));
        }
        return facItemList;
    }

    /// <summary>
    /// 检测当前道具是否正在使用
    /// </summary>
    /// <param name="itemId">设施道具Id</param>
    internal bool CheckIsUseFacilityItem(int facilityId, int itemId)
    {
        return facilitiesStoreCache[facilityId].useItemId == itemId;
    }

    /// <summary>
    /// 使用指定设施
    /// </summary>
    /// <param name="itemId">设施道具Id</param>
    internal void SetUseFacilityItem(int facilityId, int itemId)
    {
        facilitiesStoreCache[facilityId].useItemId = itemId;
        SaveFacilitiesData();
    }

    /// <summary>
    /// 获取所有设施组配置数据
    /// </summary>
    /// <returns></returns>
    internal List<FacilitiesConfigData> GetAllFacilities()
    {
        return ConfigDataManager.Instance.GetDatabase<FacilitiesConfigDatabase>().FindAll();
    }
    /// <summary>
    /// 获取所有菜单配置数据
    /// </summary>
    /// <returns></returns>
    internal List<MenuConfigData> GetAllMenu()
    {
        return ConfigDataManager.Instance.GetDatabase<MenuConfigDatabase>().FindAll();
    }

    /// <summary>
    /// 读取本地储存数据
    /// </summary>
    internal override void ReadData()
    {
        base.ReadData();
        facilitiesStoreCache = ConfigManager.Instance.ReadFile<Dictionary<int, FacilitiesStoreData>>("facilitiesData.data");
        menuStoreCache = ConfigManager.Instance.ReadFile<Dictionary<int, MenuStoreData>>("menuData.data");

        if (facilitiesStoreCache == null) facilitiesStoreCache = new Dictionary<int, FacilitiesStoreData>();
        if (menuStoreCache == null) menuStoreCache = new Dictionary<int, MenuStoreData>();
    }

    /// <summary>
    /// 保存所有菜单数据
    /// </summary>
    internal override void SaveData()
    {
        base.SaveData();
        SaveFacilitiesData();
        SaveMenuData();
    }

    /// <summary>
    /// 保存设施数据
    /// </summary>
    internal void SaveFacilitiesData()
    {
        ConfigManager.Instance.WriteFile(facilitiesStoreCache, "facilitiesData.data");
    }

    /// <summary>
    /// 保存菜单数据
    /// </summary>
    internal void SaveMenuData()
    {
        ConfigManager.Instance.WriteFile(menuStoreCache, "menuData.data");
    }
}
