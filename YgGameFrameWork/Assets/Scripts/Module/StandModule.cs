using System.Collections.Generic;
using Tool.Database;

public class StandModule : BaseModule
{
    private int standNum = 5;//展台的个数
    //private Dictionary<int, StandData> _StandCache;
    //public Dictionary<int, StandData> StandCache { get { return _StandCache; } }
    internal override void Init(GameModuleManager moduleManager)
    {
        base.Init(moduleManager);
        //_StandCache = new Dictionary<int, StandData>();
        ReadData();
        ParseConfig();
    }
    /// <summary>
    /// 解析数据表
    /// </summary>
    private void ParseConfig()
    {
        //for (int i = 0; i < standNum; i++)
        //{
        //    GetStandData(i + 1);
        //}
        //List<StandConfigData> standConfigDatas = ConfigDataManager.Instance.GetDatabase<StandConfigDatabase>().FindAll();
        //standConfigDatas.ForEach((config) => {
        //    GetStandData(config.id);
        //});
    }
    /// <summary>
    /// 获取展台的数据
    /// </summary>
    /// <param name="standID"></param>
    /// <returns></returns>
    //public StandData GetStandData(int standID)
    //{
        
    //    return null;
    //}
  
    internal override void ReadData()
    {
        base.ReadData();
        //_StandStoreData = ConfigManager.Instance.ReadFile<Dictionary<int, StandStoreData>>("StandStoreData.data");
        //if (_StandStoreData == null)
        //    _StandStoreData = new Dictionary<int, StandStoreData>();
    }
    internal override void SaveData()
    {
        base.SaveData();
        //if (GameDataManager.Instance.GetTeach())
        //{
        //    ConfigManager.Instance.WriteFile(_StandStoreData, "StandStoreData.data");
        //}
    }
}
