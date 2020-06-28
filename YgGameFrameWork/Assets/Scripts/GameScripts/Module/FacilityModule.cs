using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacilityModule : BaseModule
{
    private FacilityStoreData facilityData;

    /// <summary>
    /// 设施增益效果值
    /// </summary>
    public Dictionary<int, float> FacilityAddValueCache
    {
        get { return facilityData.facilityAddValueCache; }
    }

    /// <summary>
    /// 小费上限
    /// </summary>
    public int MaxGratuity
    {
        get { return facilityData.maxGratuity; }
        set
        {
            facilityData.maxGratuity = Mathf.Max(0, value);
            SaveData();
        }
    }
    /// <summary>
    /// 当前小费
    /// </summary>
    public int CurGratuity
    {
        get { return facilityData.curGratuity; }
        set
        {
            facilityData.curGratuity = Mathf.Max(0, value);
            SaveData();
        }
    }
    /// <summary>
    /// 每分钟小费收入
    /// </summary>
    public int GratuityPerMinute
    {
        get { return facilityData.gratuityPerMinute; }
        set
        {
            facilityData.gratuityPerMinute = Mathf.Max(0, value);
            SaveData();
        }
    }

    /// <summary>
    /// 初始化设施数据
    /// </summary>
    /// <param name="moduleManager"></param>
    internal override void Init(GameModuleManager moduleManager)
    {
        base.Init(moduleManager);
        ReadData();
    }

    /// <summary>
    /// 购买新设施获取增益收入值
    /// </summary>
    /// <param name="facilityId"></param>
    /// <param name="addValue"></param>
    internal void AddFacilityIncomeValue(int facilityId, float addValue)
    {
        if (!FacilityAddValueCache.ContainsKey(facilityId))
        {
            FacilityAddValueCache[facilityId] = 0;
        }
        FacilityAddValueCache[facilityId] += addValue;
        SaveData();
    }

    /// <summary>
    /// 读取储存数据
    /// </summary>
    internal override void ReadData()
    {
        facilityData = ConfigManager.Instance.ReadFile<FacilityStoreData>("facilityValue.data");
        if (facilityData == null)
        {
            facilityData = new FacilityStoreData
            {
                facilityAddValueCache = new Dictionary<int, float>()
            };
            CurGratuity = 1000;
        }
        int fishCount = TimeDifferenceManager.Instance.OfflineSeconds / 60;
        if (fishCount > 0)
        {
            //离线收银台收益
            CurGratuity += fishCount * GratuityPerMinute;
            CurGratuity = Mathf.Clamp(CurGratuity + GratuityPerMinute, 0, MaxGratuity);
        }
    }

    /// <summary>
    /// 保存设施数据
    /// </summary>
    internal override void SaveData()
    {
        ConfigManager.Instance.WriteFile(facilityData, "facilityValue.data");
    }
}
