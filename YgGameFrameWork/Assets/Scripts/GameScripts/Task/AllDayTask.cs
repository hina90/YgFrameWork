using System.Collections.Generic;

public class AllDayTask
{
    public TaskCondition taskCondition;//任务条件列表
    public TaskReward taskReward;//任务奖励列表
    public TaskStoreData taskStore;//持久化数据
    public AllDayTask()
    {
        taskCondition = new TaskCondition(TaskType.ALL_TASK, 0, 5);
        taskReward = new TaskReward(2, 25);
        ReadData();
    }
    private void SaveData()
    {
        ConfigManager.Instance.WriteFile(taskStore, "allDayTaskData.data");
    }
    private void ReadData()
    {
        if (TimeDifferenceManager.Instance.WhetherTheSameDay)
        {
            TDDebug.DebugLog("AllDayTask中：同一天登陆");
            taskStore = ConfigManager.Instance.ReadFile<TaskStoreData>("allDayTaskData.data");
        }
        if (taskStore == null)
            taskStore = new TaskStoreData();
        List<DayTaskData> dayTask = TaskManager.Instance.dayTaskModule.GetAllDayTaskData();
        dayTask.ForEach((dayTaskData) =>
        {
            if (dayTaskData.dayTaskStoreData.taskState == TaskState.FINISH)
            {
                Check(TaskType.ALL_TASK);
            }
        });
        SaveData();
    }
    public void Check(TaskType conditionType)
    {
        if (taskCondition.conditionType == conditionType)
        {
            taskCondition.nowAmount += 1;
            if (taskCondition.nowAmount < 0)
                taskCondition.nowAmount = 0;
            if (taskCondition.nowAmount >= taskCondition.targetAmount)
            {
                taskCondition.nowAmount = taskCondition.targetAmount;
                taskCondition.isFinish = true;
            }
            else
                taskCondition.isFinish = false;
        }
        if (!taskCondition.isFinish)
        {
            if (taskStore.taskState == TaskState.UNFINISH)
                taskStore.taskState = TaskState.UNFINISH;
            return;
        }
        //如果条件都满足
        if (taskStore.taskState == TaskState.UNFINISH)//只有任务是未完成的状态才能设置为未领取
        {
            taskStore.taskState = TaskState.UNCLAIMED;
            TaskManager.Instance.FinishTask();
            SaveData();
        }
    }
    /// <summary>
    /// 获取奖励
    /// </summary>
    public void Reward()
    {
        //获得奖励
        if (taskStore.taskState != TaskState.UNCLAIMED)//只有状态是未领取状态才可以领取奖励
            return;
        taskStore.taskState = TaskState.FINISH;
        UIManager.Instance.SendUIEvent(GameEvent.UPDATE_STAR, taskReward.amount);//发送奖励
        TaskManager.Instance.GetReward(0);
        SaveData();
    }
}
