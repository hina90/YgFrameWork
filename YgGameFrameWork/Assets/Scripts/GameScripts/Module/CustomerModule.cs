using System.Collections.Generic;
using Tool.Database;

public class CustomerModule : BaseModule
{
    ///所有顾客缓存
    private List<CustomerData> customerCache;
    /// <summary>
    /// 普通顾客缓存
    /// </summary>
    private List<CustomerData> ordinaryCustomer;
    /// <summary>
    /// 特殊顾客缓存
    /// </summary>
    private List<CustomerData> especialCustomer;
    //自然流客人数据
    private List<CustomerData> NormalConfigList;
    //手动宣传客人数据
    private List<CustomerData> ClickConfigList;
    //广告宣传客人数据
    private List<CustomerData> AdvertiseConfigList;

    #region 持久化模块
    private Dictionary<int, CustomerStoreData> customerStoreCache;
    /// <summary>
    /// 记录已经介绍过的顾客数据
    /// </summary>
    private List<int> customerHaveProfileList;
    #endregion

    internal override void Init(GameModuleManager moduleManager)
    {
        base.Init(moduleManager);
        customerCache = new List<CustomerData>();
        ordinaryCustomer = new List<CustomerData>();
        especialCustomer = new List<CustomerData>();
        ReadData();
        ParseConfig();
        Classification();
    }

    /// <summary>
    /// 解析配置表
    /// </summary>
    private void ParseConfig()
    {
        List<CustomerConfigData> customerConfigs = GetAllCustomer();
        customerConfigs.ForEach((configData) =>
        {
            customerCache.Add(new CustomerData()
            {
                customerConfig = configData,
                storeData = GetCustomerStoreData(configData.Id),
            });
        });
    }

    /// <summary>
    /// 获取所有顾客数据
    /// </summary>
    /// <returns></returns>
    internal List<CustomerData> GetAllCustomerData()
    {
        return customerCache;
    }
    /// <summary>
    /// 普通顾客
    /// </summary>
    /// <returns></returns>
    internal List<CustomerData> GetOrdinaryCustomerData()
    {
        return ordinaryCustomer;
    }
    /// <summary>
    /// 特殊顾客
    /// </summary>
    /// <returns></returns>
    internal List<CustomerData> GetEspecialCustomerData()
    {
        return especialCustomer;
    }
    /// <summary>
    /// 获取自然流量顾客
    /// </summary>
    /// <returns></returns>
    internal List<CustomerData> GetNormalConfigList()
    {
        return NormalConfigList;
    }
    /// <summary>
    /// 获取点击流量顾客
    /// </summary>
    /// <returns></returns>
    internal List<CustomerData> GetClickConfigList()
    {
        return ClickConfigList;
    }
    /// <summary>
    /// 获取广告流量顾客
    /// </summary>
    /// <returns></returns>
    internal List<CustomerData> GetAdvertiseConfigList()
    {
        return AdvertiseConfigList;
    }
    //分类
    private void Classification()
    {
        customerCache.ForEach((customerData) =>
        {
            if (customerData.customerConfig.customerType[0] == 1)
                ordinaryCustomer.Add(customerData);
            else
                especialCustomer.Add(customerData);
        });

        NormalConfigList = ordinaryCustomer.FindAll(temp => temp.customerConfig.promotionalConditions == 0);
        ClickConfigList = ordinaryCustomer.FindAll(temp => temp.customerConfig.promotionalConditions == 1);
        ClickConfigList.AddRange(NormalConfigList);
        AdvertiseConfigList = ordinaryCustomer.FindAll(temp => temp.customerConfig.promotionalConditions == 2);
        AdvertiseConfigList.AddRange(ClickConfigList);
    }
    /// <summary>
    /// 获取指定顾客持久化数据
    /// </summary>
    /// <param name="customerId">顾客Id</param>
    /// <returns></returns>
    private CustomerStoreData GetCustomerStoreData(int customerId)
    {
        if (!customerStoreCache.TryGetValue(customerId, out CustomerStoreData storeData))
        {
            customerStoreCache[customerId] = new CustomerStoreData();
            storeData = customerStoreCache[customerId];
        }
        return storeData;
    }

    public Dictionary<int, CustomerStoreData> CustomerStoreDataList()
    {
        return customerStoreCache;
    }

    /// <summary>
    /// 解锁客人
    /// </summary>
    /// <param name="customerId"></param>
    public void UnlockCustomer(int customerId)
    {
        if (GetCustomerStoreData(customerId).isActive)//如果是已经解锁过得顾客就直接返回
            return;
        TaskManager.Instance.CheckTask(TaskType.UNLOCK_CUSTOMER, 1);//解锁客人
        GetCustomerStoreData(customerId).isActive = true;
        SaveData();
        if (customerId == 1001 || customerId == 1002) return;
        TaskManager.Instance.UpdateAimTaskProgress(AimTaskType.UnlockCustomer, customerId);
    }
    /// <summary>
    /// 获取所有顾客配置数据列表
    /// </summary>
    /// <returns></returns>
    internal List<CustomerConfigData> GetAllCustomer()
    {
        return ConfigDataManager.Instance.GetDatabase<CustomerConfigDatabase>().FindAll();
    }
    internal int GetUnlockedCustomer()
    {
        int count = ConfigDataManager.Instance.GetDatabase<CustomerConfigDatabase>().FindAll().Count;
        List<CustomerData> customers = GetAllCustomerData();
        int index = 0;
        for (int i = 0; i < count; i++)
        {
            if (customers[i].storeData.isActive)
            {
                index += 1;
            }
        }
        return index;
    }

    /// <summary>
    /// 添加已介绍过的客人
    /// </summary>
    /// <param name="customerId"></param>
    internal void AddHaveProfileCustomer(int customerId)
    {
        if (customerHaveProfileList.Contains(customerId)) return;
        customerHaveProfileList.Add(customerId);
        SaveHaveProfileCustomerData();
    }

    /// <summary>
    /// 当前顾客是否需要显示简介面板
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    internal bool GetCustomerIsProfile(int customerId)
    {
        return customerHaveProfileList.Contains(customerId);
    }

    /// <summary>
    /// 读取本地储存数据
    /// </summary>
    internal override void ReadData()
    {
        base.ReadData();
        customerStoreCache = ConfigManager.Instance.ReadFile<Dictionary<int, CustomerStoreData>>("customerData.data");
        if (customerStoreCache == null)
        {
            customerStoreCache = new Dictionary<int, CustomerStoreData>();
        }
        customerHaveProfileList = ConfigManager.Instance.ReadFile<List<int>>("profileCustomerData.data");
        if (customerHaveProfileList == null)
        {
            customerHaveProfileList = new List<int>();
        }
    }

    /// <summary>
    /// 保存本地储存数据
    /// </summary>
    internal override void SaveData()
    {
        base.SaveData();
        ConfigManager.Instance.WriteFile(customerStoreCache, "customerData.data");
    }

    /// <summary>
    /// 保存顾客是否已唯一显示数据
    /// </summary>
    private void SaveHaveProfileCustomerData()
    {
        ConfigManager.Instance.WriteFile(customerHaveProfileList, "profileCustomerData.data");
    }
}
