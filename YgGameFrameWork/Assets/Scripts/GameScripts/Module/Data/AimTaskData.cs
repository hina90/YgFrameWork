using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool.Database;

public class AimTaskData
{
    /// <summary>
    /// 引导任务的Id
    /// </summary>
    public int taskId;
    /// <summary>
    /// 任务配置数据
    /// </summary>
    public AimTaskConfigData taskConfig;
    /// <summary>
    /// 当前任务进度
    /// </summary>
    public int taskProgressValue;
    /// <summary>
    /// 是否已完成当前任务
    /// </summary>
    public bool isFinish;

    public AimTaskData(AimTaskUnit aimTaskUnit)
    {
        taskId = aimTaskUnit.taskId;
        taskProgressValue = aimTaskUnit.taskProgressValue;
        isFinish = aimTaskUnit.isFinish;
        taskConfig = ConfigDataManager.Instance.GetDatabase<AimTaskConfigDatabase>().GetDataByKey(taskId.ToString());
    }
}
