using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomerSpanModule : BaseModule
{
    /// <summary>
    /// 客人晚餐消费数据
    /// </summary>
    private GuestHaveDinnerStoreData storeData;

    internal override void Init(GameModuleManager moduleManager)
    {
        base.Init(moduleManager);
        storeData = new GuestHaveDinnerStoreData();
        ReadData();
    }
    /// <summary>
    /// 获取顾客吃晚餐次数
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public int GetHaveDinnerNumber(int id)
    {
        int time = 0;
        if(storeData.haveDinnerTimeDic.ContainsKey(id))
        {
            time = storeData.haveDinnerTimeDic[id];
        }

        return time;
    }
    /// <summary>
    /// 设置吃饭次数
    /// </summary>
    /// <param name="id"></param>
    public void SetHaveDinnerNumber(int id)
    {
        if (!storeData.haveDinnerTimeDic.ContainsKey(id))
            storeData.haveDinnerTimeDic[id] = 0;

        storeData.haveDinnerTimeDic[id]++;
    }
    /// <summary>
    /// 重置所有顾客吃饭数据
    /// </summary>
    internal override void ResetData()
    {
        foreach (int kv in storeData.haveDinnerTimeDic.Keys)
        {
            storeData.haveDinnerTimeDic[kv] = 0;
        }
    }
    /// <summary>
    /// 读取本地储存数据
    /// </summary>
    internal override void ReadData()
    {
        base.ReadData();
        storeData = ConfigManager.Instance.ReadFile<GuestHaveDinnerStoreData>("GuestHaveDinnerStore.data");
        if (storeData == null)
            storeData = new GuestHaveDinnerStoreData();
    }
    /// <summary>
    /// 保存本地数据
    /// </summary>
    internal override void SaveData()
    {
        base.SaveData();
        ConfigManager.Instance.WriteFile(storeData, "GuestHaveDinnerStore.data");
    }
}

