using System.Collections.Generic;

/// <summary>
/// 物品数据类
/// </summary>
[System.Serializable]
public class ItemStoreData
{
    /// <summary>
    /// 是否解锁
    /// </summary>
    public bool isUnluck;
    /// <summary>
    /// 是否购买
    /// </summary>
    public bool isPurchase;
    /// <summary>
    /// 小鱼干数量
    /// </summary>
    public int fishNum;
}

/// <summary>
/// 设施存储数据类
/// </summary>
[System.Serializable]
public class FacilitiesStoreData
{
    /// <summary>
    /// 设施Id
    /// </summary>
    public int facitiliesId;
    /// <summary>
    /// 当前使用的设施道具Id
    /// </summary>
    public int useItemId;
    /// <summary>
    /// 设施道具数据列表
    /// </summary>
    public Dictionary<int, ItemStoreData> itemStoreDatas;
}

/// <summary>
/// 菜单储存数据类
/// </summary>
[System.Serializable]
public class MenuStoreData
{
    /// <summary>
    /// 是否解锁
    /// </summary>
    public bool isUnluck;
    /// <summary>
    /// 是否学习
    /// </summary>
    public bool isStudy;
}
