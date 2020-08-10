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
        //List<ExhibitsConfigData> exhibitsConfigs = ConfigDataManager.Instance.GetDatabase<ExhibitsConfigDatabase>().FindAll();
        //exhibitsConfigs.ForEach((config) =>
        //{
        //    exhibitsCache.Add(new ExhibitsData
        //    {
        //        exhibitsConfig = config,
        //        exhibitsStoreData = GetExhibitsStore(config.exhibitID)
        //    });
        //});
    }

    internal override void ReadData()
    {
        //base.ReadData();
        //exhibitsStoreData = ConfigManager.Instance.ReadFile<Dictionary<int, ExhibitsStoreData>>("ExhibitsStoreData.data");
        //if (exhibitsStoreData == null)
        //    exhibitsStoreData = new Dictionary<int, ExhibitsStoreData>();
    }
    internal override void SaveData()
    {
        base.SaveData();
     
        //ConfigManager.Instance.WriteFile(exhibitsStoreData, "ExhibitsStoreData.data");
    }
}
