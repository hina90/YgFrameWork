using UnityEngine;
using UnityEngine.UI;

public class AllDayTaskCall : CellBase
{
    /// <summary>
    /// 已经完成任务图标
    /// </summary>
    private Transform complete;
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
    /// 进度条
    /// </summary>
    private Image progressBar;
    private bool label = false;
    private void Start()
    {
        complete = Find(gameObject, "AllTaskComplete").transform;
        progressInfo = Find<Text>(gameObject, "ProgressInfo");
        taskBtn = Find<Button>(gameObject, "TaskBtn");
        taskBtnImg = Find<Image>(gameObject, "Image");
        progressBar = Find<Image>(gameObject, "Bar");
        taskBtn.onClick.AddListener(() =>
        {
            Reward();
            complete.gameObject.SetActive(true);
            taskBtn.gameObject.SetActive(false);
            label = true;
        });
    }
    private void Update()
    {
        if (label)
            return;
        ProgressInfo(progressInfo, progressBar, TaskManager.Instance.allDayTask.taskCondition.nowAmount, TaskManager.Instance.dayTaskModule.GetAllDayTask().Count);
        Finish(taskBtnImg, taskBtn, TaskManager.Instance.allDayTask.taskStore.taskState);
        WhetherReward(complete, taskBtn.transform, TaskManager.Instance.allDayTask.taskStore.taskState);
        if (TaskManager.Instance.allDayTask.taskStore.taskState == TaskState.FINISH)
            label = true;
    }
    public void Reward()
    {
        TaskManager.Instance.allDayTask.Reward();
    }
}
