using System;
using System.Collections.Generic;
using Tool.Database;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    /// <summary>
    /// 蓝色品质
    /// </summary>
    public static Color blue = new Color(54f / 225f, 153f / 225f, 222f / 225f);
    /// <summary>
    /// 紫色品质
    /// </summary>
    public static Color purple = new Color(135f / 225f, 108f / 225f, 223f / 225f);
    /// <summary>
    /// 橙色品质
    /// </summary>
    public static Color yellow = new Color(231f / 225f, 105f / 225f, 12f / 225f);

    public PlayerData playerData;
    /// <summary>
    ///临时存储
    /// </summary>
    public List<DigConfigData> tempList = new List<DigConfigData>();

    List<DigConfigData> digFraList = new List<DigConfigData>();
 
    public void Init()
    {
        //if (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified)
        //{
        //    Localization.language = "Chinese";
        //}
        //else if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
        //{
        //    Localization.language = "ChineseTraditional";
        //}
        //else
        //{
        //    Localization.language = "English";
        //}

        if (LoadGameData() == null)
        {
            playerData = new PlayerData();
            InitPlayerData();
            InitDataTest();
            //TestOffNew(); //关闭新手教程
        }
        else
        {
            playerData = LoadGameData();
        }
    }
    /// <summary>
    /// 初始化玩家数据
    /// </summary>
    private void InitPlayerData()
    {
        SDKManager.Instance.LogEvent(EventId.GameLogin,"InitPlayer","PlayerData");
        playerData.mIsGameBGMOpen = true;
        playerData.mIsGameSoundOpen = true;
        playerData.DicOwnExhibitIDs.Add(1002, 1);
        playerData.mOffLineTime = DateTime.Now;
        playerData.teachDict = new Dictionary<eTeachType, int>();
        LuckPhoneTime = DateTime.MinValue;
        SaveGameData();
    }
    /// <summary>
    /// 初始化测试数据
    /// </summary>
    private void InitDataTest()
    {
        playerData.Gold = 10000;
        playerData.Diamond = 500;
        playerData.DigCostItem = 15;
        SaveGameData();
    }

    /// <summary>
    /// 保存玩家数据
    /// </summary>
    public void SaveGameData()
    {
        if (playerData == null) return;
        ConfigManager.Instance.WriteFile(playerData, "gameData.data");
    }

    /// <summary>
    /// 获取玩家游戏数据
    /// </summary>
    private PlayerData LoadGameData()
    {
        return ConfigManager.Instance.ReadFile<PlayerData>("gameData.data");
    }

    /// <summary>
    /// 数字转换 K M
    /// </summary>
    /// <returns></returns>
    public string CountToKorM(Int64 count)
    {
        float unitaa = 1000000000000000000;
        float unitT =  1000000000000000;
        float unitB =  1000000000000;
        float unitG =  1000000000;
        float unitM =  1000000;
        float unitK =  1000;
        float nowUnit;
        string strunit;
        if(count >= unitaa)
        {
            nowUnit = unitaa;
            strunit = "aa";
        }else if (count >= unitT)
        {
            nowUnit = unitT;
            strunit = "T";
        }
        else if (count >= unitB)
        {
            nowUnit = unitB;
            strunit = "B";
        }
        else if(count >= unitG)
        {
            nowUnit = unitG;
            strunit = "G";
        }
        else if (count >= unitM)
        {
            nowUnit = unitM;
            strunit = "M";
        }
        else if (count >= unitK)
        {
            nowUnit = unitK;
            strunit = "K";
        }
        else
        {
            return count.ToString();
        }
        var value = count / nowUnit;
        if (value >= 100f)
            return value.ToString("f0") + strunit;
        else if (value >= 10f)
            return value.ToString("f1") + strunit;
        else
            return value.ToString("f2") + strunit;
    }
    public TimeSpan getDeltaTime(DateTime start, DateTime end)
    {
        if (start.Ticks >= end.Ticks)
            return new TimeSpan(0);

        var startSpan = new TimeSpan(start.Ticks);
        var endSpan = new TimeSpan(end.Ticks);
        return endSpan.Subtract(startSpan);
    }
    public string getDeltaTimeStr(DateTime start, DateTime end)
    {
        if (start.Ticks >= end.Ticks)
            return "00:00";

        TimeSpan ts = getDeltaTime(start, end);
        if(ts.TotalHours < 1)
            return string.Format("{0:D2}:{1:D2}", (Int32)ts.Minutes, (Int32)ts.Seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", (Int32)ts.TotalHours, (Int32)ts.Minutes, (Int32)ts.Seconds);
    }
    public bool GetTeach()
    {

        return true;

        //for (eTeachType i = eTeachType.lockstand; i < eTeachType.Max; i++)
        //{
        //    if (!playerData.teachDict.ContainsKey(i))
        //    {
        //        return false;
        //    }
        //}
        //return true;
    }
    public void AddTeach(eTeachType type)
    {
        if (!playerData.teachDict.ContainsKey(type))
        {
            playerData.teachDict.Add(type, 1);
        }
        playerData.teachDict[type] = 1;
        SaveGameData();
    }
    Dictionary<eTeachType, List<NewGameData>> newGameDict;
    public List<NewGameData> GetNewGameInfo(eTeachType type)
    {
        if (newGameDict==null)
        {
            newGameDict = new Dictionary<eTeachType, List<NewGameData>>();
            var temp = ConfigDataManager.Instance.GetDatabase<NewGameDatabase>().FindAll();
            foreach (var item in temp)
            {
                if (!newGameDict.ContainsKey((eTeachType)item.teachType))
                {
                    newGameDict.Add((eTeachType)item.teachType, new List<NewGameData>());
                }
                newGameDict[(eTeachType)(item.teachType)].Add(item);
            }
        }
        return newGameDict[type];
    }
    /// <summary>
    /// 手动关闭新手引导
    /// </summary>
    public void TestOffNew()
    {
        for (eTeachType i = 0; i < eTeachType.Max; i++)
        {
            if (!playerData.teachDict.ContainsKey(i))
            {
                playerData.teachDict.Add(i, 1);
            }
            playerData.teachDict[i] = 1;
            SaveGameData();
        }
    }
    private static GlobalConfigData mGlobalData = null;
    public static GlobalConfigData GlobalData
    {
        get {
            if(mGlobalData == null)
            {
                var globalList = ConfigDataManager.Instance.GetDatabase<GlobalConfigDatabase>().FindAll();
                if(globalList.Count != 1)
                {
                    Debug.LogError("GlobalConfigDatabase is not 1 line");
                    return null;
                }
                mGlobalData = globalList[0];
            }
            return mGlobalData;
        }
    }

    private DateTime mLuckPhoneTime;
    public DateTime LuckPhoneTime
    {
        set
        {
            UIManager.Instance.SendUIEvent(GameEvent.UPDATE_MAINGAME_EFFECT);
            mLuckPhoneTime = value;
            SaveGameData();
        }
        get
        {
            return mLuckPhoneTime;
        }
    }
    public bool IsInLuckPhoneTime()
    {
        if (mLuckPhoneTime == DateTime.MinValue)
            return false;
        return mLuckPhoneTime > DateTime.Now;
    }
  
}
