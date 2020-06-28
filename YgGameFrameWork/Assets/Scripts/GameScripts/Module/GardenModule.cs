using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool.Database;
using System;

/// <summary>
/// 花园模块
/// </summary>
public class GardenModule : BaseModule
{
    private GardenStoreData gardenData;
    private readonly int[] intervalTimes = new int[] { 10, 30, 60, 120 };
    private const int time = 50400;

    /// <summary>
    /// 花园花朵信息列表(key:花圃设施Id，value:花圃信息)
    /// </summary>
    public Dictionary<int, ParterreStoreData> ParterreDataDic
    {
        get
        {
            return gardenData.parterreDataDic;
        }
    }

    /// <summary>
    /// 许愿池许愿次数
    /// </summary>
    public int WishTimes
    {
        get { return gardenData.wishTimes; }
        set
        {
            gardenData.wishTimes = value;
            SaveData();
        }
    }

    /// <summary>
    /// 许愿池剩余许愿次数
    /// </summary>
    public int RemainWishTimes
    {
        get { return gardenData.remainWishTimes; }
        set
        {
            gardenData.remainWishTimes = value;
            SaveData();
        }
    }

    /// <summary>
    /// 许愿池物品掉落个数
    /// </summary>
    public int WishRewardNum
    {
        get { return gardenData.wishRewardNum; }
        set
        {
            gardenData.wishRewardNum = value;
            SaveData();
        }
    }

    /// <summary>
    /// 许愿池剩余重置时间
    /// </summary>
    public int RemainResetTime
    {
        get { return gardenData.remainResetTime; }
        set
        {
            gardenData.remainResetTime = value;
        }
    }
    /// <summary>
    /// 许愿池下一次许愿时间
    /// </summary>
    public DateTime NextWishTime
    {
        get { return gardenData.nextWishTime; }
        set
        {
            gardenData.nextWishTime = value;
            SaveData();
        }
    }

    /// <summary>
    /// 许愿间隔时间
    /// </summary>
    public int[] WishIntervalTime
    {
        get { return gardenData.wishIntervalTime; }
        set
        {
            gardenData.wishIntervalTime = value;
        }
    }

    /// <summary>
    /// 变异花朵剩余CD冷却时间
    /// </summary>
    public int MutationCDTime
    {
        get { return gardenData.mutationCDTime; }
        set
        {
            gardenData.mutationCDTime = value;
        }
    }

    /// <summary>
    /// 花园花品成长效率加成
    /// </summary>
    public float GrowthRateBonus
    {
        get { return gardenData.growthRateBonus; }
        set
        {
            gardenData.growthRateBonus = value;
            SaveData();
        }
    }

    /// <summary>
    /// 等待变异的花圃Id
    /// </summary>
    public int WaitVariationId
    {
        get { return gardenData.waitVariationId; }
        set
        {
            gardenData.waitVariationId = value;
            SaveData();
        }
    }

    internal override void Init(GameModuleManager moduleManager)
    {
        base.Init(moduleManager);
        ReadData();
        TimerManager.Instance.CreateTimer("gardenTimer", 0, 1, () =>
        {
            SaveData();
        });
    }

    /// <summary>
    /// 更新花园花朵信息
    /// </summary>
    /// <param name="parterreId"></param>
    /// <param name="value"></param>
    internal void SetParterreDataValue(int parterreId, ParterreStoreData value)
    {
        if (ParterreDataDic.ContainsKey(parterreId)) return;
        ParterreDataDic[parterreId] = value;
        SaveData();
    }

    /// <summary>
    /// 增加客人观赏的次数
    /// </summary>
    /// <param name="parterreId">花圃设施Id</param>
    /// <param name="count">当前观赏增加值</param>
    internal void AddVisitCount(int parterreId, int count)
    {
        if (!ParterreDataDic.TryGetValue(parterreId, out ParterreStoreData storeData))
        {
            storeData = new ParterreStoreData()
            {
                parterreStatus = ParterreStatus.Idle,
                visitTimes = 2,
                remainVisitTimes = 2,
            };
            ParterreDataDic[parterreId] = storeData;
        }
        storeData.visitTimes += count;
        storeData.remainVisitTimes += count;
        SaveData();
    }

    /// <summary>
    /// 许愿池计时器
    /// </summary>
    private float curTime;
    internal override void OnUpdate()
    {
        //许愿池重置时间刷新
        if (RemainResetTime > 0)
        {
            curTime += Time.deltaTime;
            if (curTime >= 1)
            {
                RemainResetTime--;
                curTime = 0;
                //重置许愿次数
                if (RemainResetTime == 0)
                {
                    RemainWishTimes = WishTimes;
                }
                UIManager.Instance.SendUIEvent(GameEvent.UPDATE_WISHPOOL_RESETTIME, RemainResetTime);
            }
        }
        //许愿池下一次许愿间隔时间
        for (int i = 0; i < WishIntervalTime.Length; i++)
        {
            if (WishIntervalTime[i] > 0)
            {
                curTime += Time.deltaTime;
                if (curTime >= 1)
                {
                    WishIntervalTime[i]--;
                    //发送对应许愿间隔时间，完成冷却
                    UIManager.Instance.SendUIEvent(GameEvent.UPDATE_WISHPOOL_INTERVALTTIME, i);
                    curTime = 0;
                }
            }
        }

        MutationCDTimerCount();
    }

    /// <summary>
    /// 许愿池重置CD冷却开始
    /// </summary>
    internal void CDTimerCount()
    {
        RemainResetTime = time;
        NextWishTime = TimeDifferenceManager.Instance.TargetTime(time / 60f);
    }

    /// <summary>
    /// 许愿间隔时间开始CD
    /// </summary>
    internal void IntervalTimerCount(int index)
    {
        WishIntervalTime[index] = intervalTimes[index];
    }

    /// <summary>
    /// 变异花朵内置冷却倒计时
    /// </summary>
    private float mutateCdTime;
    internal void MutationCDTimerCount()
    {
        if (MutationCDTime > 0)
        {
            mutateCdTime += Time.deltaTime;
            if (mutateCdTime >= 1)
            {
                MutationCDTime--;
                mutateCdTime = 0;
                //重置变异花朵冷却时间
                if (MutationCDTime <= 0)
                {
                    MutationCDTime = 0;
                }
            }
        }
    }

    internal override void ReadData()
    {
        base.ReadData();
        gardenData = ConfigManager.Instance.ReadFile<GardenStoreData>("gardenData.data");
        if (gardenData == null)
        {
            gardenData = new GardenStoreData()
            {
                parterreDataDic = new Dictionary<int, ParterreStoreData>(),
                wishIntervalTime = new int[4],
            };
        }

        if (RemainWishTimes < WishTimes && RemainWishTimes > 0)
        {
            IntervalTimerCount(WishTimes - RemainWishTimes - 1);
        }
        RemainResetTime = (int)TimeDifferenceManager.Instance.CountDown(NextWishTime).TotalSeconds;
        if (RemainResetTime <= 0)
        {
            RemainResetTime = 0;
            RemainWishTimes = WishTimes;
            return;
        }
        //TimeDifferenceManager.Instance
        RemainResetTime -= TimeDifferenceManager.Instance.OfflineSeconds;
    }

    internal override void SaveData()
    {
        base.SaveData();
        ConfigManager.Instance.WriteFile(gardenData, "gardenData.data");
    }
    internal GardenConfigData GardenConfig(int GardenID)
    {
        return ConfigDataManager.Instance.GetDatabase<GardenConfigDatabase>().GetDataByKey(GardenID.ToString());
    }
}
