using Tool.Database;

public class Task : TaskBase
{
    private TaskConfigData task;

    /// <summary>
    /// 根据id读取任务表初始化
    /// </summary>
    /// <param name="taskID"></param>
    public Task(int taskID)
    {
        task = TaskManager.Instance.taskModule.GetTaskData(taskID).taskConfig;//获取单条任务数据
        taskStore = TaskManager.Instance.taskModule.GetTaskData(taskID).taskStoreData;//获取单条任务的持久化数据

        this.taskID = taskID;//添加任务ID
        taskDes = task.des;//添加任务描述

        TaskCondition taskCondition = new TaskCondition((TaskType)task.condition, taskStore.nowAmount, task.conArgument);
        taskConditions.Add(taskCondition);//添加任务条件

        TaskReward taskReward = new TaskReward(task.reward, task.rewardArgument);
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
        if (task.reward == 1)
            UIManager.Instance.SendUIEvent(GameEvent.UPDATE_FISH, task.rewardArgument);//发送奖励
        if (task.reward == 2)
            UIManager.Instance.SendUIEvent(GameEvent.UPDATE_STAR, task.rewardArgument);//发送奖励
        TaskManager.Instance.GetReward(task.taskID);
    }
    /// <summary>
    /// 取消任务
    /// </summary>
    public void Cancel()
    {
        TaskManager.Instance.CancelTask(task.taskID);
    }
}
