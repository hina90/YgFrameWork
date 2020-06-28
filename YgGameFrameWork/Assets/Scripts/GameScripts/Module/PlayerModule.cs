using System;
using UnityEngine;

public class PlayerModule : BaseModule
{
    private PlayerStoreData playerData;
    /// <summary>
    /// 星级
    /// </summary>
    public int Star
    {
        get { return playerData.star; }
        set
        {
            playerData.star = Mathf.Max(0, value);
            //TDDebug.DebugLogFormat("当前评价星级:{0}", playerData.star);
            SaveData();
        }
    }
    /// <summary>
    /// 鱼干
    /// </summary>
    public int Fish
    {
        get { return playerData.fish; }
        set
        {
            playerData.fish = Mathf.Max(0, value);
            //TDDebug.DebugLogFormat("当前小鱼干数量:{0}", playerData.fish);
            SaveData();
        }
    }
    /// <summary>
    /// 打理天数
    /// </summary>
    public int SurvivalDays
    {
        get { return playerData.survivalDays; }
        set
        {
            playerData.survivalDays = value;
            SaveData();
        }
    }
    /// <summary>
    /// 是否是第一次登录游戏
    /// </summary>
    public bool MoneyIsOne
    {
        get { return playerData.moneyIsOne; }
        set
        {
            playerData.moneyIsOne = value;
            SaveData();
        }
    }
    /// <summary>
    /// 上线离线时间
    /// </summary>
    public DateTime OfflineTime
    {
        get { return playerData.offlineTime; }
        set
        {
            playerData.offlineTime = value;
            SaveData();
        }
    }
    /// <summary>
    /// 登录时间
    /// </summary>
    public DateTime LoadingTime
    {
        get { return playerData.loadingTime; }
        set
        {
            playerData.loadingTime = value;
            SaveData();
        }
    }
    public DateTime GardenUnlockTime { get { return playerData.gardenUnlockTime; } }
    /// <summary>
    /// 音乐音量
    /// </summary>
    /// <value></value>
    public float BGMVolume
    {
        get { return playerData.bgmVolume; }
        set
        {
            playerData.bgmVolume = value;
            SaveData();
        }
    }
    /// <summary>
    /// 音效音量
    /// </summary>
    /// <value></value>
    public float AudioVolume
    {
        get { return playerData.audioVolume; }
        set
        {
            playerData.audioVolume = value;
            SaveData();
        }
    }

    /// <summary>
    /// 免费2倍小费台
    /// </summary>
    public int FreeCashier
    {
        get { return playerData.freeCashier; }
        set
        {
            playerData.freeCashier = value;
            SaveData();
        }
    }

    internal override void Init(GameModuleManager moduleManager)
    {
        base.Init(moduleManager);
        ReadData();
    }

    internal override void ReadData()
    {
        playerData = ConfigManager.Instance.ReadFile<PlayerStoreData>("playerData.data");
        if (playerData == null)
        {
            playerData = new PlayerStoreData();
            Fish = 1000;
            Star = 0;
            SurvivalDays = 1;
            MoneyIsOne = false;
            OfflineTime = TimeDifferenceManager.Instance.GetWebTime();
            LoadingTime = TimeDifferenceManager.Instance.GetWebTime();
            playerData.gardenUnlockTime = TimeDifferenceManager.Instance.TargetTime(5);
            BGMVolume = 1;
            AudioVolume = 1;
        }
        TimeDifferenceManager.Instance.Init();
    }

    internal override void SaveData()
    {
        ConfigManager.Instance.WriteFile(playerData, "playerData.data");
    }
}
