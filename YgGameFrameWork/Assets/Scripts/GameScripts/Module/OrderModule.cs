using System.Collections.Generic;
using Tool.Database;

public class OrderModule : BaseModule
{
    private Dictionary<int, OrderData> orderCache;                  //订单只有注册了才会存放在列表中
    private Dictionary<int, OrderStoreData> orderStoreData;         //持久化数据
    internal override void Init(GameModuleManager moduleManager)
    {
        base.Init(moduleManager);
        orderCache = new Dictionary<int, OrderData>();
        ReadData();
        ParseConfig();
    }
    /// <summary>
    /// 解析数据表
    /// </summary>
    private void ParseConfig()
    {
        foreach (KeyValuePair<int, OrderStoreData> item in orderStoreData)
        {
            orderCache[item.Value.orderID] = new OrderData
            {
                orderConfig = ConfigDataManager.Instance.GetDatabase<OrderConfigDatabase>().GetDataByKey(item.Value.orderID.ToString()),
                orderStoreData = item.Value
            };
        }
    }
    /// <summary>
    /// 这边用作显示作用，将字典转换为线性表
    /// </summary>
    /// <returns></returns>
    public List<OrderData> GetListOrderData()
    {
        List<OrderData> orderDatas = new List<OrderData>();
        foreach (KeyValuePair<int, OrderData> item in orderCache)
        {
            orderDatas.Add(item.Value);
        }
        orderDatas.Sort(new OrderComparer());
        return orderDatas;
    }
    /// <summary>
    /// 注册订单
    /// </summary>
    public void RegisterOrder(int orderID)
    {
        if (!orderCache.TryGetValue(orderID, out OrderData orderData))
        {
            orderStoreData[orderID] = new OrderStoreData();
            orderCache[orderID] = new OrderData
            {
                orderConfig = ConfigDataManager.Instance.GetDatabase<OrderConfigDatabase>().GetDataByKey(orderID.ToString()),
                orderStoreData = orderStoreData[orderID]
            };
            orderCache[orderID].orderStoreData.targetTime = TimeDifferenceManager.Instance.TargetTime(orderCache[orderID].orderConfig.orderCompletionTime);
            orderCache[orderID].orderStoreData.orderID = orderID;
        }
    }
    public OrderData GetOrderData(int orderID)
    {
        if (!orderCache.ContainsKey(orderID))
            return null;
        return orderCache[orderID];
    }
    /// <summary>
    /// 订单完成后就要删除字典中的订单数据
    /// </summary>
    /// <param name="orderID"></param>
    public void ReleaseData(int orderID)
    {
        try
        {
            orderCache.Remove(orderID);
            orderStoreData.Remove(orderID);
        }
        catch (System.IndexOutOfRangeException)
        {
            TDDebug.DebugLogError("没有这个键");
            throw;
        }
    }
    /// <summary>
    /// 读取保存的订单列表和订单持久化数据
    /// </summary>
    internal override void ReadData()
    {
        base.ReadData();
        orderStoreData = ConfigManager.Instance.ReadFile<Dictionary<int, OrderStoreData>>("orderStoreData.data");
        if (orderStoreData == null)
            orderStoreData = new Dictionary<int, OrderStoreData>();
    }
    /// <summary>
    /// 保存订单列表和订单持久化数据
    /// </summary>
    internal override void SaveData()
    {
        base.SaveData();
        ConfigManager.Instance.WriteFile(orderStoreData, "orderStoreData.data");
    }
}
public class OrderComparer : IComparer<OrderData>
{
    public int Compare(OrderData x, OrderData y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        if (x.orderStoreData.targetTime.CompareTo(y.orderStoreData.targetTime) > 0) return 1;
        if (x.orderStoreData.targetTime.CompareTo(y.orderStoreData.targetTime) < 0) return -1;
        return 0;
    }
}
