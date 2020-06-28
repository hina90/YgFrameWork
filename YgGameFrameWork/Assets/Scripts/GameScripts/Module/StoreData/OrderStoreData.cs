using System;

[System.Serializable]
public class OrderStoreData
{
    /// <summary>
    /// 订单号
    /// </summary>
    public int orderID;
    /// <summary>
    /// 订单完成的实际时间
    /// </summary>
    public DateTime targetTime;
    /// <summary>
    /// 订单状态
    /// </summary>
    public TaskState taskState = TaskState.UNFINISH;
}
