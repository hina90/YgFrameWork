using UnityEngine;
using UnityEngine.UI;

public class TaskPrefbCall : CellBase
{
    public Task task;
    /// <summary>
    /// 已经完成任务图标
    /// </summary>
    private Transform complete;
    /// <summary>
    /// 任务描述
    /// </summary>
    private Text taskInfo;
    /// <summary>
    /// 奖励参数
    /// </summary>
    private Text rewardNum;
    /// <summary>
    /// 进度数据
    /// </summary>
    private Text progressInfo;
    /// <summary>
    /// 按钮
    /// </summary>
    private Button taskBtn;
    /// <summary>
    /// 按钮上的文字图片
    /// </summary>
    private Image taskBtnImg;
    /// <summary>
    /// 奖励物品的图标
    /// </summary>
    private Image rewardIcon;
    /// <summary>
    /// 进度条
    /// </summary>
    private Image progressBar;
    private void Awake()
    {
        complete = Find(gameObject, "CompleteTask").transform;
        taskInfo = Find<Text>(gameObject, "TaskInfo");
        taskBtn = Find<Button>(gameObject, "TaskBtn");
        taskBtnImg = Find<Image>(gameObject, "taskBtnImg");
        rewardIcon = Find<Image>(gameObject, "RewardIcon");
        rewardNum = Find<Text>(gameObject, "RewardNum");
        progressBar = Find<Image>(gameObject, "Bar");
        progressInfo = Find<Text>(gameObject, "ProgressInfo");
        taskBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            Reward();
            complete.gameObject.SetActive(true);
            taskBtn.gameObject.SetActive(false);
        });
    }
    private void ScrollCellContent(TaskData taskData)
    {
        task = TaskManager.Instance.dictionary[taskData.taskConfig.taskID] as Task;
        taskInfo.text = taskData.taskConfig.des;//添加任务描述

        for (int i = 0; i < task.taskConditions.Count; i++)
        {
            ProgressInfo(progressInfo, progressBar, taskData.taskStoreData.nowAmount, task.taskConditions[i].targetAmount);    //设置进度
        }
        for (int i = 0; i < task.taskRewards.Count; i++)
        {
            RewardInfo(rewardIcon, rewardNum, task.taskRewards[i].id, task.taskRewards[i].amount);                         //设置奖励信息
        }
        Finish(taskBtnImg, taskBtn, taskData.taskStoreData.taskState);                                                        //设置图标状态
        WhetherReward(complete, taskBtn.transform, taskData.taskStoreData.taskState);
    }
    /// <summary>
    /// 获得奖励
    /// </summary>
    public void Reward()
    {
        task.Reward();
    }
    /// <summary>
    /// 取消任务，暂时用不到
    /// </summary>
    public void Cancel()
    {
        task.Cancel();
    }
}
