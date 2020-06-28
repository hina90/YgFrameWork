using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OrderPrfabCall : CellBase
{
    private Text orderName;         //订单名字
    private Text explain;           //订单说明
    private Text goldNum;           //奖励鱼干数目
    private Text starNum;           //奖励评价数目
    private Text timeRemaining;     //剩余时间
    private Image timerBar;         //进度条
    private Button rewardBtn;       //按键
    private GameObject btnMask;
    private GameObject finishImg;
    private void Awake()
    {
        orderName = Find<Text>(gameObject, "OrderName");
        explain = Find<Text>(gameObject, "Explain");
        goldNum = Find<Text>(gameObject, "GoldNum");
        starNum = Find<Text>(gameObject, "StarNum");
        timeRemaining = Find<Text>(gameObject, "TimeRemaining");
        timerBar = Find<Image>(gameObject, "bar");
        rewardBtn = Find<Button>(gameObject, "ReceiveBtn");
        btnMask = Find(gameObject, "BtnMask");
        finishImg = Find(gameObject, "Finish");
    }
    void ScrollCellContent(OrderData orderData)
    {
        orderName.text = orderData.orderConfig.orderName;
        explain.text = orderData.orderConfig.orderDes;
        goldNum.text = orderData.orderConfig.rewardFish.ToString();
        starNum.text = orderData.orderConfig.rewardStar.ToString();
        rewardBtn.onClick.RemoveAllListeners();
        rewardBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            orderData.orderStoreData.taskState = TaskState.FINISH;
            UIManager.Instance.SendUIEvent(GameEvent.UPDATE_FISH, orderData.orderConfig.rewardFish);
            UIManager.Instance.SendUIEvent(GameEvent.UPDATE_STAR, orderData.orderConfig.rewardStar);
            OrderManager.Instance.ReleaseOrder(orderData.orderConfig.orderID);//注销该订单
            TaskManager.Instance.SubReceived();//减少任务计数
        });
        StartCoroutine(Timer(orderData));
    }
    private IEnumerator Timer(OrderData orderData)
    {
        while (true)
        {
            yield return 1;
            ShowTimerAndButton(orderData);
        }
    }
    private void ShowTimerAndButton(OrderData orderData)
    {
        TimeSpan timeSpan = TimeDifferenceManager.Instance.CountDown(orderData.orderStoreData.targetTime);
        if (timeSpan.Hours > 0)
            timeRemaining.text = $"{timeSpan.Hours}时{timeSpan.Minutes}分";
        else
            timeRemaining.text = $"{timeSpan.Minutes}分{timeSpan.Seconds}秒";
        timerBar.fillAmount = Convert.ToSingle((1f / orderData.orderConfig.orderCompletionTime) * (orderData.orderConfig.orderCompletionTime - timeSpan.TotalMinutes));

        if (orderData.orderStoreData.taskState == TaskState.UNCLAIMED)
        {
            btnMask.SetActive(false);
            timeRemaining.gameObject.SetActive(false);
        }
        else if (orderData.orderStoreData.taskState == TaskState.UNFINISH)
        {
            rewardBtn.gameObject.SetActive(true);
            btnMask.SetActive(true);
            finishImg.SetActive(false);
            timeRemaining.gameObject.SetActive(true);
        }
        else
        {
            btnMask.SetActive(false);
            timeRemaining.gameObject.SetActive(false);
            rewardBtn.gameObject.SetActive(false);
            finishImg.SetActive(true);
        }
    }
}
