using System.Collections.Generic;

public class TaskBase
{
    public int taskID;//任务id
    public string taskDes;//任务描述
    public TaskStoreData taskStore;
    public List<TaskCondition> taskConditions = new List<TaskCondition>();//任务条件列表
    public List<TaskReward> taskRewards = new List<TaskReward>();//任务奖励列表
    /// <summary>
    /// 判断条件是否满足
    /// </summary>
    /// <param name="taskEvent"></param>
    public void Check(TaskType conditionType, int amount, int customerID = 0)
    {
        for (int i = 0; i < taskConditions.Count; i++)
        {
            if (taskConditions[i].conditionType == conditionType)
            {
                if (conditionType == TaskType.DESIGNATED_CUSTOMER)
                    if (taskConditions[i].customerID != customerID)
                        return;
                taskConditions[i].nowAmount += amount;
                if (taskConditions[i].nowAmount < 0)
                    taskConditions[i].nowAmount = 0;
                if (taskConditions[i].nowAmount >= taskConditions[i].targetAmount)
                {
                    taskConditions[i].nowAmount = taskConditions[i].targetAmount;
                    taskConditions[i].isFinish = true;
                }
                else
                    taskConditions[i].isFinish = false;

                //这里修改持久化数据里的进度
                taskStore.nowAmount = taskConditions[i].nowAmount;
            }
        }
        for (int i = 0; i < taskConditions.Count; i++)//遍历所有条件，看是否有一个条件不满足
        {
            if (!taskConditions[i].isFinish)//只要有一个条件不满足
            {
                //如果是条件不相同的不能设置
                if (conditionType == taskConditions[i].conditionType)
                    taskStore.taskState = TaskState.UNFINISH;
                return;
            }
        }
        //如果所有条件都满足
        if (taskStore.taskState == TaskState.UNFINISH)//只有任务是未完成的状态才能设置为未领取
        {
            taskStore.taskState = TaskState.UNCLAIMED;
            TaskManager.Instance.FinishTask();
        }
    }
}
