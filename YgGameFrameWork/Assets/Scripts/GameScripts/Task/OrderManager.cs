using System.Collections.Generic;

public class OrderManager : UnitySingleton<OrderManager>
{
    private OrderModule orderModule;
    private bool justOnLine = true;
    public void Init()
    {
        orderModule = GameModuleManager.Instance.GetModule<OrderModule>();
        TimerManager.Instance.CreateTimer("OrderTimer", 2, 1, CheckOrder);//不断检测是否有订单完成
    }
    public void CreateOrder(int orderID)
    {
        if (GetOrder(orderID))
            return;
        //打开订单提示页面
        UIManager.Instance.OpenUI<UI_OrderTip>(orderID, true);
    }
    public void RegisterOrder(int orderID)
    {
        if (GetOrder(orderID))
            return;
        orderModule.RegisterOrder(orderID);
        SaveData();
    }
    /// <summary>
    /// 判断是否已经有了这个订单，若没有再添加
    /// </summary>
    public bool GetOrder(int orderID)
    {
        if (orderModule.GetOrderData(orderID) == null)
            return false;
        return true;
    }
    public void ReleaseOrder(int orderID)
    {
        orderModule.ReleaseData(orderID);
        SaveData();
    }
    public List<OrderData> GetOrderDatas()
    {
        return orderModule.GetListOrderData();
    }
    public OrderData GetOrderData(int orderID)
    {
        return orderModule.GetOrderData(orderID);
    }
    private void SaveData()
    {
        orderModule.SaveData();
    }
    //检测是否有订单完成
    public void CheckOrder()
    {
        List<OrderData> orderDatas = GetOrderDatas();
        orderDatas.ForEach((orderData) =>
        {
            if (TimeDifferenceManager.Instance.CompareTime(orderData.orderStoreData.targetTime))
            {
                if (orderData.orderStoreData.taskState == TaskState.UNFINISH)
                {
                    TaskManager.Instance.AddReceived();
                    orderData.orderStoreData.taskState = TaskState.UNCLAIMED;
                    SaveData();
                }
                //只有刚上线才会打开订单完成领取任务奖品的显示页面
                if (justOnLine)
                {
                    UIManager.Instance.OpenUI<UI_OrderTip>(orderData.orderConfig.orderID, false);
                }
            }
        });
        justOnLine = false;
    }
}
