using Tool.Database;

public class DayTask : TaskBase
{
    private DayTaskConfigData dayTask;

    /// <summary>
    /// 根据id读取任务表初始化
    /// </summary>
    /// <param name="taskID"></param>
    public DayTask(int taskID)
    {
        dayTask = TaskManager.Instance.dayTaskModule.GetDayTaskData(taskID).dayTaskConfig;//获取单条任务数据
        taskStore = TaskManager.Instance.dayTaskModule.GetDayTaskData(taskID).dayTaskStoreData;//获取单条任务的持久化数据

        this.taskID = taskID;//添加任务ID
        taskDes = dayTask.des;//添加任务描述

        if (dayTask.conArgument.Length == 1)
        {
            TaskCondition taskCondition = new TaskCondition((TaskType)dayTask.condition, taskStore.nowAmount, dayTask.conArgument[0]);
            taskConditions.Add(taskCondition);//添加任务条件
        }
        else
        {
            TaskCondition taskCondition = new TaskCondition((TaskType)dayTask.condition, taskStore.nowAmount, dayTask.conArgument[0], dayTask.conArgument[1]);
            taskConditions.Add(taskCondition);//添加任务条件
        }


        TaskReward taskReward = new TaskReward(dayTask.reward, dayTask.rewardArgument);
        taskRewards.Add(taskReward);//添加奖励条件
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
        if (dayTask.reward == 1)
            UIManager.Instance.SendUIEvent(GameEvent.UPDATE_FISH, dayTask.rewardArgument);//发送奖励
        if (dayTask.reward == 2)
            UIManager.Instance.SendUIEvent(GameEvent.UPDATE_STAR, dayTask.rewardArgument);//发送奖励
        TaskManager.Instance.allDayTask.Check(TaskType.ALL_TASK);//每完成一个每日任务，就发送一次消息
        TaskManager.Instance.GetReward(dayTask.taskID);
    }
    /// <summary>
    /// 取消任务
    /// </summary>
    public void Cancel()
    {
        TaskManager.Instance.CancelTask(dayTask.taskID);
    }
}
