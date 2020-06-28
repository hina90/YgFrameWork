using Tool.Database;
using UnityEngine;

/// <summary>
/// 设施道具数据
/// </summary>
public class FacilitiesItemData
{
    /// <summary>
    /// 设施配置数据
    /// </summary>
    public ItemConfigData itemConfigData;
    /// <summary>
    /// 设施持久化数据
    /// </summary>
    public ItemStoreData storeData;
    /// <summary>
    /// 设备类型Id
    /// </summary>
    public FacilitiesConfigData facilitiesConfigData;
    /// <summary>
    /// 设备物品Id
    /// </summary>
    public int itemId;
    /// <summary>
    /// 设备位置坐标
    /// </summary>
    public string pos;
    /// <summary>
    /// 设备实例物体
    /// </summary>
    public GameObject gameObject;
}

/// <summary>
/// 菜单菜品数据
/// </summary>
public class MenuData
{
    /// <summary>
    /// 菜品配置数据
    /// </summary>
    public MenuConfigData configData;
    /// <summary>
    /// 菜品持久化数据
    /// </summary>
    public MenuStoreData storeData;
}
