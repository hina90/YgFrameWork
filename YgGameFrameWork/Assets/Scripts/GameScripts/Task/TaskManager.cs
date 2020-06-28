using System;
using System.Collections.Generic;

public class TaskManager : UnitySingleton<TaskManager>
{
    #region 回调暂时不用
    // public event Callback GetEvent;//接受任务时,更新任务到任务面板等操作
    // public event Callback FinishEvent;//完成任务时,提示完成任务等操作
    // public event Callback RewardEvent;//得到奖励时,显示获取的物品等操作
    // public event Callback CancelEvent;//取消任务时,显示提示信息等操作
    #endregion
    public Dictionary<int, TaskBase> dictionary = new Dictionary<int, TaskBase>();//id,task
    public TaskModule taskModule;
    public DayTaskModule dayTaskModule;
    public AimTaskModule aimTaskModule;
    public AllDayTask allDayTask;
    private AimTask aimTask;
    private int notReceived = 0;    //用于计算已经完成且未领取的成就、日常、订单有几个
    public void Init()
    {
        taskModule = GameModuleManager.Instance.GetModule<TaskModule>();
        dayTaskModule = GameModuleManager.Instance.GetModule<DayTaskModule>();
        aimTaskModule = GameModuleManager.Instance.GetModule<AimTaskModule>();
        allDayTask = new AllDayTask();
        aimTask = new AimTask(aimTaskModule);

        for (int i = 0; i < taskModule.GetAllTask().Count; i++)
            GetTask(taskModule.GetAllTask()[i].taskID);
        for (int i = 0; i < dayTaskModule.GetAllDayTask().Count; i++)
            GetDayTask(dayTaskModule.GetAllDayTask()[i].taskID);
        if (notReceived > 0)
            UIManager.Instance.SendUIEvent(GameEvent.OPEN_TASKTIP);
    }
    public void SetReceivedTips()
    {
        taskModule.GetAllTaskData().ForEach((taskData) =>
        {
            if (taskData.taskStoreData.taskState == TaskState.UNCLAIMED)
                notReceived += 1;
        });
        dayTaskModule.GetAllDayTaskData().ForEach((dayTaskData) =>
        {
            if (dayTaskData.dayTaskStoreData.taskState == TaskState.UNCLAIMED)
                notReceived += 1;
        });
        if (notReceived > 0)
            UIManager.Instance.SendUIEvent(GameEvent.OPEN_TASKTIP);
    }
    /// <summary>
    /// 获取指定任务
    /// </summary>
    /// <param name="taskID"></param>
    public void GetTask(int taskID)
    {
        if (!dictionary.ContainsKey(taskID))
        {
            Task task = new Task(taskID);
            dictionary.Add(taskID, task);
            //GetEvent?.Invoke();//获取任务后执行的回调方法
        }
    }
    public void GetDayTask(int taskID)
    {
        if (!dictionary.ContainsKey(taskID))
        {
            DayTask task = new DayTask(taskID);
            dictionary.Add(taskID, task);
        }
    }
    /// <summary>
    /// 检查任务是否满足条件
    /// </summary>
    /// <param name="conditionID">条件ID</param>
    /// <param name="amount">每次增加的进度</param>
    public void CheckTask(TaskType conditionType, int amount, int customerID = 0)
    {
        foreach (KeyValuePair<int, TaskBase> item in dictionary)
        {
            item.Value.Check(conditionType, amount, customerID);
        }
        //保存数据
        taskModule.SaveData();
        if (dayTaskModule != null)
        {
            dayTaskModule.SaveData();
        }
    }
    /// <summary>
    /// 完成任务
    /// </summary>
    public void FinishTask()
    {
        //保存数据       
        taskModule.SaveData();
        dayTaskModule.SaveData();
        //FinishEvent?.Invoke();
        AddReceived();
    }
    /// <summary>
    /// 获取奖励
    /// </summary>
    /// <param name="taskID">任务ID</param>
    public void GetReward(int taskID)
    {
        if (dictionary.ContainsKey(taskID))
        {
            for (int i = 0; i < dictionary[taskID].taskRewards.Count; i++)
            {
                //RewardEvent?.Invoke();//获得奖励后执行的回调方法
            }
        }
        //保存数据       
        taskModule.SaveData();
        dayTaskModule.SaveData();
        //dictionary.Remove(taskID);
        SubReceived();
    }
    public void AddReceived()
    {
        notReceived += 1;
        UIManager.Instance.SendUIEvent(GameEvent.OPEN_TASKTIP);
    }
    public void SubReceived()
    {
        notReceived -= 1;
        if (notReceived <= 0)
        {
            UIManager.Instance.SendUIEvent(GameEvent.CLOSE_TASKTIP);
            notReceived = 0;
        }
    }
    /// <summary>
    /// 取消任务
    /// </summary>
    /// <param name="taskID">任务ID</param>
    public void CancelTask(int taskID)
    {
        if (dictionary.ContainsKey(taskID))
        {
            //CancelEvent?.Invoke();//删除任务后执行的回调方法
            dictionary.Remove(taskID);
        }
    }

    #region 引导任务

    /// <summary>
    /// 更新引导任务进度
    /// </summary>
    public void UpdateAimTaskProgress(AimTaskType aimTaskType, object arg)
    {
        aimTask.UpdateAimTaskProgress(aimTaskType, arg);
        UIManager.Instance.SendUIEvent(GameEvent.REFRESH_AIMTASK, aimTaskModule.AimTaskData);
    }

    /// <summary>
    /// 完成当前指引任务
    /// </summary>
    public void FinishAimTask(int aimTaskId)
    {
        //完成所有引导任务,返回
        if (aimTaskId == 1019)
        {
            aimTaskModule.IsFinishTask = true;
            UIManager.Instance.SendUIEvent(GameEvent.FINISH_AIMTASK);
            return;
        }

        aimTaskModule.CurTaskId = aimTaskId + 1;

        UIManager.Instance.SendUIEvent(GameEvent.REFRESH_AIMTASK, aimTaskModule.AimTaskData);
    }

    /// <summary>
    /// 是否已完成引导任务
    /// </summary>
    /// <returns></returns>
    public bool IsFinishAimTask()
    {
        return aimTaskModule.IsFinishTask;
    }

    #endregion
}
