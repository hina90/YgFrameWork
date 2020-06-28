using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 农场模块
/// </summary>
public class FarmModule : BaseModule
{
    private FarmStoreData farmData;

    /// <summary>
    /// 农场土地果实成熟剩余时间
    /// </summary>
    public Dictionary<int, int> LandGrowUpTimeDic
    {
        get { return farmData.landGrowUpTimeDic; }
    }

    internal override void Init(GameModuleManager moduleManager)
    {
        base.Init(moduleManager);
    }

    internal override void OnUpdate()
    {
        base.OnUpdate();
        ReadData();
    }

    /// <summary>
    /// 更新农场农作物成熟时间
    /// </summary>
    /// <param name="landId"></param>
    /// <param name="value"></param>
    internal void SetLandGrowUpTimeValue(int landId, int value)
    {
        if (!LandGrowUpTimeDic.ContainsKey(landId))
        {
            LandGrowUpTimeDic[landId] = 0;
        }
        LandGrowUpTimeDic[landId] = value;
        SaveData();
    }

    internal override void ReadData()
    {
        base.ReadData();
        farmData = ConfigManager.Instance.ReadFile<FarmStoreData>("farmData.data");
    }

    internal override void SaveData()
    {
        base.SaveData();
        ConfigManager.Instance.WriteFile(farmData, "farmData.data");
    }
}
