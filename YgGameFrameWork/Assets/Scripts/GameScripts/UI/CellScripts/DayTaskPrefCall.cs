using UnityEngine;
using UnityEngine.UI;

public class DayTaskPrefCall : CellBase
{
    public DayTask dayTask;
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
    void ScrollCellContent(DayTaskData dayTaskData)
    {
        dayTask = TaskManager.Instance.dictionary[dayTaskData.dayTaskConfig.taskID] as DayTask;
        taskInfo.text = dayTaskData.dayTaskConfig.des;//添加任务描述
        for (int i = 0; i < dayTask.taskConditions.Count; i++)
        {
            ProgressInfo(progressInfo, progressBar, dayTaskData.dayTaskStoreData.nowAmount, dayTask.taskConditions[i].targetAmount);    //设置进度
        }
        for (int i = 0; i < dayTask.taskRewards.Count; i++)
        {
            RewardInfo(rewardIcon, rewardNum, dayTask.taskRewards[i].id, dayTask.taskRewards[i].amount);                         //设置奖励信息
        }
        Finish(taskBtnImg, taskBtn, dayTaskData.dayTaskStoreData.taskState);                                                        //设置图标状态
        WhetherReward(complete, taskBtn.transform, dayTaskData.dayTaskStoreData.taskState);
    }
    public void Reward()
    {
        dayTask.Reward();
    }
}
