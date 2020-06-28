using System.Collections.Generic;
using Tool.Database;

public class TaskModule : BaseModule
{
    /// <summary>
    /// 所有成就任务缓存
    /// </summary>
    private List<TaskData> taskCache;
    private Dictionary<int, TaskStoreData> taskStoreCache;

    internal override void Init(GameModuleManager moduleManager)
    {
        base.Init(moduleManager);
        taskCache = new List<TaskData>();
        ReadData();
        ParseConfig();
    }
    /// <summary>
    /// 解析配置表
    /// </summary>
    private void ParseConfig()
    {
        List<TaskConfigData> taskConfigs = GetAllTask();
        taskConfigs.ForEach((configData) =>
        {
            taskCache.Add(new TaskData()
            {
                taskConfig = configData,
                taskStoreData = GetTaskStoreData(configData.taskID)
            });
        });
    }
    /// <summary>
    /// 获取所有任务数据
    /// </summary>
    /// <returns></returns>
    internal List<TaskData> GetAllTaskData()
    {
        taskCache.Sort(new TaskComparer());
        return taskCache;
    }
    /// <summary>
    /// 获取指定任务持久化数据
    /// </summary>
    /// <param name="customerId">顾客Id</param>
    /// <returns></returns>
    private TaskStoreData GetTaskStoreData(int taskId)
    {
        if (!taskStoreCache.TryGetValue(taskId, out TaskStoreData storeData))
        {
            taskStoreCache[taskId] = new TaskStoreData();
            storeData = taskStoreCache[taskId];
        }
        return storeData;
    }
    /// <summary>
    /// 获取指定任务数据
    /// </summary>
    public TaskData GetTaskData(int taskID)
    {
        if (!taskStoreCache.TryGetValue(taskID, out TaskStoreData taskStore))
        {
            taskStoreCache[taskID] = new TaskStoreData();
        }
        taskStore = taskStoreCache[taskID];
        TaskData taskData = new TaskData()
        {
            taskConfig = ConfigDataManager.Instance.GetDatabase<TaskConfigDatabase>().GetDataByKey(taskID.ToString()),
            taskStoreData = taskStore,
        };
        return taskData;
    }
    /// <summary>
    /// 获取所有任务配置数据列表
    /// </summary>
    /// <returns></returns>
    internal List<TaskConfigData> GetAllTask()
    {
        return ConfigDataManager.Instance.GetDatabase<TaskConfigDatabase>().FindAll();
    }

    /// <summary>
    /// 读取本地储存数据
    /// </summary>
    internal override void ReadData()
    {
        base.ReadData();
        taskStoreCache = ConfigManager.Instance.ReadFile<Dictionary<int, TaskStoreData>>("taskData.data");
        if (taskStoreCache == null)
            taskStoreCache = new Dictionary<int, TaskStoreData>();
    }
    /// <summary>
    /// 保存本地数据
    /// </summary>
    internal override void SaveData()
    {
        base.SaveData();
        ConfigManager.Instance.WriteFile(taskStoreCache, "taskData.data");
    }
}

public class TaskComparer : IComparer<TaskData>
{
    public int Compare(TaskData x, TaskData y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        if ((int)x.taskStoreData.taskState > (int)y.taskStoreData.taskState) return 1;
        if ((int)x.taskStoreData.taskState < (int)y.taskStoreData.taskState) return -1;
        if (((float)x.taskStoreData.nowAmount / x.taskConfig.conArgument) > ((float)y.taskStoreData.nowAmount / y.taskConfig.conArgument)) return -1;
        if (((float)x.taskStoreData.nowAmount / x.taskConfig.conArgument) < ((float)y.taskStoreData.nowAmount / y.taskConfig.conArgument)) return 1;
        return 0;
    }
}