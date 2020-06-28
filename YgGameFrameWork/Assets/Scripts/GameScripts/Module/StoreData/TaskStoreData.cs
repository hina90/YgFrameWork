using System.Collections.Generic;

public enum TaskState
{
    /// <summary>
    /// 完成未领取
    /// </summary>
    UNCLAIMED = 1,
    /// <summary>
    /// 未完成
    /// </summary>
    UNFINISH = 2,
    /// <summary>
    /// 以完成且已领取奖励
    /// </summary>
    FINISH = 3,
}

[System.Serializable]
public class TaskStoreData
{
    /// <summary>
    /// 当前进度
    /// </summary>
    public int nowAmount;
    /// <summary>
    /// 任务状态
    /// </summary>
    public TaskState taskState = TaskState.UNFINISH;
}

[System.Serializable]
public class AimTaskStoreData
{
    public bool isFinish;
    public int curTaskId;
    public Dictionary<int, AimTaskUnit> aimTaskDic;
}

[System.Serializable]
public class AimTaskUnit
{
    /// <summary>
    /// 引导任务的Id
    /// </summary>
    public int taskId;
    /// <summary>
    /// 当前任务进度
    /// </summary>
    public int taskProgressValue;
    /// <summary>
    /// 是否已完成当前任务
    /// </summary>
    public bool isFinish;
}
