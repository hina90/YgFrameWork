using UnityEngine.UI;
using Tool.Database;
using TMPro;

public class UI_OrderTip : UIBase
{
    private Text orderName;
    private Text explain;
    private Text fishNum;
    private Text starNum;
    private TextMeshProUGUI stateText;
    private Button receiveBtn;
    private Button acceptBtn;
    private Button bgReturn;
    private OrderConfigData orderConfig;
    private int orderID;
    private bool acceptOrReceive;//接受为true,领取为false
    public override void Init()
    {
        Layer = LayerMenue.TIPS;
        orderID = (int)param[0];
        acceptOrReceive = (bool)param[1];
        orderConfig = ConfigDataManager.Instance.GetDatabase<OrderConfigDatabase>().GetDataByKey(orderID.ToString());

        orderName = Find<Text>(gameObject, "OrderName");
        explain = Find<Text>(gameObject, "Explain");
        fishNum = Find<Text>(gameObject, "GoldNum");
        starNum = Find<Text>(gameObject, "StarNum");
        stateText = Find<TextMeshProUGUI>(gameObject, "StateText");
        receiveBtn = Find<Button>(gameObject, "ReceiveBtn");
        acceptBtn = Find<Button>(gameObject, "AcceptBtn");
        bgReturn = Find<Button>(gameObject, "bg");
        bgReturn.enabled = false;

        PlayAnimation(Find(gameObject, "OrderTip"));
    }
    protected override void Enter()
    {
        orderName.text = orderConfig.orderName;
        explain.text = orderConfig.orderDes;
        fishNum.text = orderConfig.rewardFish.ToString();
        starNum.text = orderConfig.rewardStar.ToString();
        if (acceptOrReceive)
        {
            bgReturn.enabled = true;
            acceptBtn.gameObject.SetActive(true);
            stateText.text = $"<sprite=3>{(orderConfig.orderCompletionTime / 60f).ToString("0.#")}小时后登录领取!";
        }
        else
        {
            receiveBtn.gameObject.SetActive(true);
            stateText.text = "<sprite=8>准时完成!";
        }
        RegisterEvent();
    }
    private void RegisterEvent()
    {
        //接受订单
        acceptBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            OrderManager.Instance.RegisterOrder(orderID);
            UIManager.Instance.BackUI(Layer);
        });
        //领取订单奖励
        receiveBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayUIAudio("button_1");
            UIManager.Instance.SendUIEvent(GameEvent.UPDATE_FISH, orderConfig.rewardFish);
            UIManager.Instance.SendUIEvent(GameEvent.UPDATE_STAR, orderConfig.rewardStar);
            OrderManager.Instance.ReleaseOrder(orderConfig.orderID);//注销该订单
            TaskManager.Instance.SubReceived();
            UIManager.Instance.BackUI(Layer);
        });
        bgReturn.onClick.AddListener(() =>
        {
            UIManager.Instance.BackUI(Layer);
        });
    }
}
