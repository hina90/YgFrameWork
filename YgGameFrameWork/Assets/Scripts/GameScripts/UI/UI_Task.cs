using System.Collections.Generic;
using SG;
using UnityEngine;
using UnityEngine.UI;

public class UI_Task : UIBase
{
    private Button exitBtn;
    private Button bgReturn;
    private Text timeText;
    private Toggle achievementBtn;
    private Toggle dayTaskBtn;
    private Toggle orderBtn;
    private GameObject orderIsNull;
    private GameObject[] pans;
    private TaskModule taskModule;
    private DayTaskModule dayTaskModule;
    private LoopVerticalScrollRect achievementContent;
    private InitOnStart achievementCount;
    private LoopVerticalScrollRect dayTaskContent;
    private InitOnStart dayTaskCount;
    private LoopVerticalScrollRect orderContent;
    private InitOnStart orderCount;
    public override void Init()
    {
        Layer = LayerMenue.UI;
        taskModule = GameModuleManager.Instance.GetModule<TaskModule>();
        dayTaskModule = GameModuleManager.Instance.GetModule<DayTaskModule>();
        PlayAnimation(Find(gameObject, "Scale"));

        //成就任务页面数据
        achievementContent = Find<LoopVerticalScrollRect>(gameObject, "AchievementPan");
        achievementCount = Find<InitOnStart>(gameObject, "AchievementPan");

        //每日任务页面数据
        dayTaskContent = Find<LoopVerticalScrollRect>(gameObject, "DayTaskPan");
        dayTaskCount = Find<InitOnStart>(gameObject, "DayTaskPan");

        //订单任务页面数据
        orderContent = Find<LoopVerticalScrollRect>(gameObject, "OrderPan");
        orderCount = Find<InitOnStart>(gameObject, "OrderPan");
    }
    protected override void Enter()
    {
        exitBtn = Find<Button>(gameObject, "ExitBtn");
        bgReturn = Find<Button>(gameObject, "bg");
        achievementBtn = Find<Toggle>(gameObject, "AchievementBtn");
        dayTaskBtn = Find<Toggle>(gameObject, "DayTaskBtn");
        orderBtn = Find<Toggle>(gameObject, "OrderBtn");
        timeText = Find<Text>(gameObject, "TimeText");
        orderIsNull = Find(gameObject, "IfNull");
        pans = new GameObject[]
        {
            Find(gameObject,"AchievementBg"),
            Find(gameObject,"DayTaskBg"),
            Find(gameObject,"OrderBg")
        };

        ListenerEvent();
        achievementBtn.isOn = true;
    }
    private bool playAudio = false;
    private void ListenerEvent()
    {
        exitBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            UIManager.Instance.BackUI(Layer);
        });
        bgReturn.onClick.AddListener(() =>
        {
            UIManager.Instance.BackUI(Layer);
        });
        achievementBtn.onValueChanged.AddListener((bool fyx) =>
        {
            if (!fyx)
                return;
            if (playAudio)
                AudioManager.Instance.PlayUIAudio("button_1");
            pans[0].SetActive(true);
            pans[1].SetActive(false);
            pans[2].SetActive(false);
            achievementCount.totalCount = taskModule.GetAllTaskData().Count;
            achievementContent.objectsToFill = taskModule.GetAllTaskData().ToArray();
            achievementContent.RefillCells();
            playAudio = true;
        });
        dayTaskBtn.onValueChanged.AddListener((bool fyx) =>
        {
            if (!fyx)
                return;
            pans[0].SetActive(false);
            pans[1].SetActive(true);
            pans[2].SetActive(false);
            AudioManager.Instance.PlayUIAudio("button_1");
            dayTaskCount.totalCount = dayTaskModule.GetAllDayTaskData().Count;
            dayTaskContent.objectsToFill = dayTaskModule.GetAllDayTaskData().ToArray();
            dayTaskContent.RefillCells();
        });
        orderBtn.onValueChanged.AddListener((bool fyx) =>
        {
            if (!fyx)
                return;
            pans[0].SetActive(false);
            pans[1].SetActive(false);
            pans[2].SetActive(true);
            AudioManager.Instance.PlayUIAudio("button_1");
            orderCount.totalCount = OrderManager.Instance.GetOrderDatas().Count;
            orderContent.objectsToFill = OrderManager.Instance.GetOrderDatas().ToArray();
            orderCount.Start();
            if (orderCount.totalCount > 0)
                orderIsNull.SetActive(false);
            else
                orderIsNull.SetActive(true);
        });
    }
    public override void MainUpdate()
    {
        timeText.text = TimeDifferenceManager.Instance.ToZeroTime();
    }
    public override void Release()
    {
        CachePoolManager.Instance.DestroyPool("TaskPref");
        CachePoolManager.Instance.DestroyPool("DayTaskPref");
        CachePoolManager.Instance.DestroyPool("OrderPref");
        base.Release();
    }
}
