using System.Collections.Generic;
using Tool.Database;
using UnityEngine;

public class DayTaskModule : BaseModule
{
    /// <summary>
    /// 所有每日任务缓存
    /// </summary>
    private List<DayTaskData> dayTaskCache;
    private Dictionary<int, TaskStoreData> dayTaskStoreCache;
    private List<int> dayTaskID;
    private List<int> rangeID;//可选取的范围ID
    private PlayerModule playerModule;
    internal override void Init(GameModuleManager moduleManager)
    {
        base.Init(moduleManager);
        dayTaskCache = new List<DayTaskData>();
        playerModule = moduleManager.GetModule<PlayerModule>();
        rangeID = new List<int>();
        GetRangeTaskID();
        ReadDayTaskID();
        ReadData();
        ParseConfig();
    }
    internal void SetInit()
    {
        GetRangeTaskID();
        ReadDayTaskID();
        ReadData();
        ParseConfig();
        SaveData();
    }
    /// <summary>
    /// 解析配置表
    /// </summary>
    private void ParseConfig()
    {
        List<DayTaskConfigData> taskConfigs = GetAllDayTask();
        taskConfigs.ForEach((configData) =>
        {
            dayTaskCache.Add(new DayTaskData()
            {
                dayTaskConfig = configData,
                dayTaskStoreData = GetDayTaskStoreData(configData.taskID)
            });
        });
    }
    /// <summary>
    /// 获取可选任务范围的taskID
    /// </summary>
    private void GetRangeTaskID()
    {
        List<DayTaskConfigData> taskConfigs = ConfigDataManager.Instance.GetDatabase<DayTaskConfigDatabase>().FindAll();
        taskConfigs.ForEach((taskData) =>
        {
            if (playerModule.Star >= taskData.accessConditions[0] && playerModule.Star < taskData.accessConditions[1])
            {
                rangeID.Add(taskData.taskID);
            }
        });
    }
    /// <summary>
    /// 获取所有任务数据
    /// </summary>
    /// <returns></returns>
    internal List<DayTaskData> GetAllDayTaskData()
    {
        dayTaskCache.Sort(new DayTaskComparer());
        return dayTaskCache;
    }
    /// <summary>
    /// 每日随机五个任务
    /// </summary>
    /// <returns></returns>
    internal List<DayTaskConfigData> GetAllDayTask()
    {
        List<DayTaskConfigData> dayTaskConfigDatas = new List<DayTaskConfigData>();
        dayTaskID.ForEach((config) =>
        {
            dayTaskConfigDatas.Add(ConfigDataManager.Instance.GetDatabase<DayTaskConfigDatabase>().GetDataByKey(config.ToString()));
        });
        return dayTaskConfigDatas;
    }
    /// <summary>
    /// 获取指定任务数据
    /// </summary>
    public DayTaskData GetDayTaskData(int taskID)
    {
        if (!dayTaskStoreCache.TryGetValue(taskID, out TaskStoreData taskStore))
        {
            dayTaskStoreCache[taskID] = new TaskStoreData();
        }
        taskStore = dayTaskStoreCache[taskID];
        DayTaskData taskData = new DayTaskData()
        {
            dayTaskConfig = ConfigDataManager.Instance.GetDatabase<DayTaskConfigDatabase>().GetDataByKey(taskID.ToString()),
            dayTaskStoreData = taskStore,
        };
        return taskData;
    }
    /// <summary>
    /// 获取指定任务持久化数据
    /// </summary>
    /// <param name="customerId">顾客Id</param>
    /// <returns></returns>
    private TaskStoreData GetDayTaskStoreData(int taskId)
    {
        if (!dayTaskStoreCache.TryGetValue(taskId, out TaskStoreData storeData))
        {
            dayTaskStoreCache[taskId] = new TaskStoreData();
            storeData = dayTaskStoreCache[taskId];
        }
        return storeData;
    }
    /// <summary>
    /// 读取本地储存数据
    /// </summary>
    internal override void ReadData()
    {
        base.ReadData();
        //如果不是同一天就不读取储存的数据
        if (TimeDifferenceManager.Instance.WhetherTheSameDay)
            dayTaskStoreCache = ConfigManager.Instance.ReadFile<Dictionary<int, TaskStoreData>>("dayTaskData.data");
        if (dayTaskStoreCache == null)
            dayTaskStoreCache = new Dictionary<int, TaskStoreData>();
    }
    /// <summary>
    /// 保存本地数据
    /// </summary>
    internal override void SaveData()
    {
        base.SaveData();
        ConfigManager.Instance.WriteFile(dayTaskStoreCache, "dayTaskData.data");
    }
    private void ReadDayTaskID()
    {
        if (TimeDifferenceManager.Instance.WhetherTheSameDay)
        {
            dayTaskID = ConfigManager.Instance.ReadFile<List<int>>("dayTaskID.data");
            if (dayTaskID == null)
                RandomTaskID();//如果是同一天登陆就读取之前的任务ID
        }
        else
            RandomTaskID();//随机两个任务ID
    }
    private void SaveDayTaskID()
    {
        ConfigManager.Instance.WriteFile(dayTaskID, "dayTaskID.data");
    }
    private void RandomTaskID()
    {
        dayTaskID = new List<int>();
        if (rangeID.Count < 5)
            return;
        for (int i = 0; i < 5; i++)//每日随机五个任务
        {
            int index = Random.Range(0, rangeID.Count - 1);
            dayTaskID.Add(rangeID[index]);
            rangeID.RemoveAt(index);
        }
        SaveDayTaskID();
    }
}
public class DayTaskComparer : IComparer<DayTaskData>
{
    public int Compare(DayTaskData x, DayTaskData y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        if ((int)x.dayTaskStoreData.taskState > (int)y.dayTaskStoreData.taskState) return 1;
        if ((int)x.dayTaskStoreData.taskState < (int)y.dayTaskStoreData.taskState) return -1;
        return 0;
    }
}
