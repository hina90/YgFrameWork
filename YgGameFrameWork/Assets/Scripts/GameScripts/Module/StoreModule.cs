using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool.Database;

/// <summary>
/// 便利店模块
/// </summary>
public class StoreModule : BaseModule
{
    private StoreData storeData;
    private List<StoreConfigData> goodsDatas;
    public List<StoreConfigData> GoodsDatas { get => goodsDatas; set => goodsDatas = value; }

    /// <summary>
    /// 货架货物信息数据
    /// </summary>
    public Dictionary<int, ShelfData> ShelfDic
    {
        get { return storeData.shelfDic; }
    }

    /// <summary>
    /// 便利店上货效率
    /// </summary>
    public float LoadRate
    {
        get { return storeData.loadRate; }
        set
        {
            storeData.loadRate = value;
        }
    }

    /// <summary>
    /// 便利店顾客加倍购买概率
    /// </summary>
    public float DoublePurchaseChance
    {
        get { return storeData.doublePurchaseChance; }
        set
        {
            storeData.doublePurchaseChance = value;
        }
    }

    /// <summary>
    /// 加倍购买的倍数
    /// </summary>
    public int Times
    {
        get { return storeData.times; }
        set
        {
            storeData.times = value;
        }
    }

    internal override void Init(GameModuleManager moduleManager)
    {
        base.Init(moduleManager);
        ParseConfig();
        ReadData();
        TimerManager.Instance.CreateTimer("storeTimer", 0, 1, () =>
        {
            SaveData();
        });
    }

    /// <summary>
    /// 解析配置表数据
    /// </summary>
    private void ParseConfig()
    {
        goodsDatas = ConfigDataManager.Instance.GetDatabase<StoreConfigDatabase>().FindAll();
    }

    internal void AddGoodsData(int facilityId, ShelfData shelfData)
    {
        if (!ShelfDic.ContainsKey(facilityId))
        {
            ShelfDic[facilityId] = shelfData;
            SaveData();
        }
    }

    internal override void OnUpdate()
    {
        base.OnUpdate();
    }

    internal override void ReadData()
    {
        base.ReadData();
        storeData = ConfigManager.Instance.ReadFile<StoreData>("storeData.data");
        if (storeData == null)
        {
            storeData = new StoreData()
            {
                shelfDic = new Dictionary<int, ShelfData>()
            };
        }
    }

    internal override void SaveData()
    {
        base.SaveData();
        ConfigManager.Instance.WriteFile(storeData, "storeData.data");
    }
}
