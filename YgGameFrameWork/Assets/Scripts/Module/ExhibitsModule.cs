using System.Collections.Generic;
using Tool.Database;

public class ExhibitsModule : BaseModule
{
    /// <summary>
    /// 所有展品的列表
    /// </summary>
    private List<ExhibitsData> exhibitsCache;
    public List<ExhibitsData> ExhibitsCache { get { return exhibitsCache; } }
    /// <summary>
    /// 展品的持久化数据
    /// </summary>
    private Dictionary<int, ExhibitsStoreData> exhibitsStoreData;
    public Dictionary<int, ExhibitsStoreData> ExhibitsStoreData { get { return exhibitsStoreData; } }
    internal override void Init(GameModuleManager moduleManager)
    {
        base.Init(moduleManager);
        exhibitsCache = new List<ExhibitsData>();
        ReadData();
        ParseConfig();
    }
    private void ParseConfig()
    {
        List<ExhibitsConfigData> exhibitsConfigs = ConfigDataManager.Instance.GetDatabase<ExhibitsConfigDatabase>().FindAll();
        exhibitsConfigs.ForEach((config) =>
        {
            exhibitsCache.Add(new ExhibitsData
            {
                exhibitsConfig = config,
                exhibitsStoreData = GetExhibitsStore(config.exhibitID)
            });
        });
    }

    internal override void ReadData()
    {
        base.ReadData();
        exhibitsStoreData = ConfigManager.Instance.ReadFile<Dictionary<int, ExhibitsStoreData>>("ExhibitsStoreData.data");
        if (exhibitsStoreData == null)
            exhibitsStoreData = new Dictionary<int, ExhibitsStoreData>();
    }
    internal override void SaveData()
    {
        base.SaveData();
        if (GameDataManager.Instance.GetTeach())
        {
            ConfigManager.Instance.WriteFile(exhibitsStoreData, "ExhibitsStoreData.data");
        }
    }
    /// <summary>
    /// 返回一个展品的持久化数据
    /// </summary>
    public ExhibitsStoreData GetExhibitsStore(int exhibitID)
    {
        if (!exhibitsStoreData.TryGetValue(exhibitID, out ExhibitsStoreData storeData))
        {
            exhibitsStoreData[exhibitID] = new ExhibitsStoreData();
            storeData = exhibitsStoreData[exhibitID];
            storeData.standID = 0;
            storeData.level = 1;
            storeData.starLevel = 1;
            storeData.exp = 0;
            storeData.currentAddition = 1;

            string levelId = exhibitID.ToString() + storeData.level.ToString("0000");
            storeData.costBase = long.Parse(ConfigDataManager.Instance.GetDatabase<UpRewardConfigDatabase>().GetDataByKey(levelId).consume);
            storeData.currentCoin = long.Parse(ConfigDataManager.Instance.GetDatabase<UpRewardConfigDatabase>().GetDataByKey(levelId).income);
            storeData.provideMuseumExp = ConfigDataManager.Instance.GetDatabase<UpRewardConfigDatabase>().GetDataByKey(levelId).toMuseumExp;
        }
        return storeData;
    }
    /// <summary>
    /// 返回一个展品数据
    /// </summary>
    public ExhibitsData GetExhibitsData(int exhibitID)
    {
        ExhibitsData exhibits = new ExhibitsData();
        exhibitsCache.ForEach((config) =>
        {
            if (config.exhibitsConfig.exhibitID == exhibitID)
                exhibits = config;
        });
        return exhibits;
    }
    /// <summary>
    /// 返回满足条件的展品，即需要要品质对应且要拼接完成,且未被放置的
    /// </summary>
    internal List<ExhibitsData> MeetingConditionsExhibits(int size)
    {
        List<ExhibitsData> exhibits = new List<ExhibitsData>();
        exhibitsCache.ForEach((config) =>
        {
            //尺寸要相同，且未被放置，且已经解锁的展品
            if (config.exhibitsConfig.size == size && !config.exhibitsStoreData.isPlace)//&& config.exhibitsStoreData.isUnlock
                exhibits.Add(config);
        });
        return exhibits;
    }
}
