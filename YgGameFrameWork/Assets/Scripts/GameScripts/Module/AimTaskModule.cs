using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool.Database;

/// <summary>
/// 指引任务模块
/// </summary>
public class AimTaskModule : BaseModule
{
    private List<AimTaskConfigData> aimTaskList;
    private AimTaskStoreData aimTaskStoreData;
    private AimTaskData aimTaskData;

    public AimTaskData AimTaskData
    {
        get { return aimTaskData; }
    }

    /// <summary>
    /// 是否完成引导任务
    /// </summary>
    public bool IsFinishTask
    {
        get { return aimTaskStoreData.isFinish; }
        set { aimTaskStoreData.isFinish = value; }
    }

    /// <summary>
    /// 当前进行中的任务Id
    /// </summary>
    public int CurTaskId
    {
        get { return aimTaskData.taskId; }
        set
        {
            aimTaskStoreData.curTaskId = value;
            aimTaskData = new AimTaskData(aimTaskStoreData.aimTaskDic[value]);
            SaveData();
        }
    }

    internal override void Init(GameModuleManager moduleManager)
    {
        base.Init(moduleManager);
        aimTaskList = new List<AimTaskConfigData>();
        ParseConfig();
        ReadData();
    }

    /// <summary>
    /// 解析数据表
    /// </summary>
    private void ParseConfig()
    {
        aimTaskList = ConfigDataManager.Instance.GetDatabase<AimTaskConfigDatabase>().FindAll();
    }

    /// <summary>
    /// 刷新指定任务进度
    /// </summary>
    public void UpdateTask(int taskId)
    {
        if (taskId < CurTaskId) return;
        AimTaskConfigData taskConfig = ConfigDataManager.Instance.GetDatabase<AimTaskConfigDatabase>().GetDataByKey(taskId.ToString());
        AimTaskUnit aimTaskUnit = aimTaskStoreData.aimTaskDic[taskId];
        if (taskConfig.targetValue == 0)
        {
            aimTaskUnit.isFinish = true;
        }
        else
        {
            aimTaskUnit.taskProgressValue += 1;
            if (aimTaskUnit.taskProgressValue >= taskConfig.targetValue)
            {
                aimTaskUnit.isFinish = true;
            }
        }
        if (taskId == CurTaskId)
        {
            aimTaskData.isFinish = aimTaskUnit.isFinish;
            aimTaskData.taskProgressValue = aimTaskUnit.taskProgressValue;
        }
        SaveData();
    }

    internal override void ReadData()
    {
        base.ReadData();
        aimTaskStoreData = ConfigManager.Instance.ReadFile<AimTaskStoreData>("aimTaskData.data");
        if (aimTaskStoreData == null)
        {
            aimTaskStoreData = new AimTaskStoreData()
            {
                curTaskId = 1001,
                aimTaskDic = new Dictionary<int, AimTaskUnit>()
            };
            //初始化引导任务
            aimTaskList.ForEach(item =>
            {
                aimTaskStoreData.aimTaskDic[item.taskId] = new AimTaskUnit()
                {
                    taskId = item.taskId,
                    taskProgressValue = 0,
                    isFinish = false
                };
            });
        }
        aimTaskData = new AimTaskData(aimTaskStoreData.aimTaskDic[aimTaskStoreData.curTaskId]);
    }

    internal override void SaveData()
    {
        base.SaveData();
        ConfigManager.Instance.WriteFile(aimTaskStoreData, "aimTaskData.data");
    }
}
