using System.Collections.Generic;
using Tool.Database;

public class SystemModule : BaseModule
{
    private Dictionary<int, SystemData> systemCache;
    private Dictionary<int, SystemStoreData> systemStoreData;
    private PlayerModule playerModule;
    internal override void Init(GameModuleManager moduleManager)
    {
        base.Init(moduleManager);
        systemCache = new Dictionary<int, SystemData>();
        playerModule = moduleManager.GetModule<PlayerModule>();
        ReadData();
        ParseConfig();
    }

    private void ParseConfig()
    {
        List<SystemConfigData> systemConfigs = GetAllSystem();
        systemConfigs.ForEach((configData) =>
        {
            systemCache[configData.Id] = new SystemData
            {
                systemConfig = configData,
                systemStoreData = GetSystemStoreData(configData.Id)
            };
        });
        if (systemCache[1001].systemStoreData.isOpen == false)
            SetSystemStoreData();
    }
    private void SetSystemStoreData()
    {
        foreach (KeyValuePair<int, SystemData> item in systemCache)
        {
            if (item.Key != 1005 && item.Key != 1006)
                item.Value.systemStoreData.isOpen = true;
            if (playerModule.Star >= item.Value.systemConfig.star)
                item.Value.systemStoreData.isUnlock = true;
            TDDebug.DebugLog($"ID{item.Key}名字为{item.Value.systemConfig.name},系统是否开放{item.Value.systemStoreData.isOpen},系统是否解锁{item.Value.systemStoreData.isUnlock}");
        }
    }
    public SystemStoreData GetSystemStoreData(int systemId)
    {
        if (!systemStoreData.TryGetValue(systemId, out SystemStoreData storeData))
        {
            systemStoreData[systemId] = new SystemStoreData();
            storeData = systemStoreData[systemId];
        }
        return storeData;
    }
    public SystemData GetSystemData(int systemID)
    {
        if (!systemCache.TryGetValue(systemID, out SystemData systemData))
        {
            ParseConfig();
        }
        return systemData;
    }
    private List<SystemConfigData> GetAllSystem()
    {
        return ConfigDataManager.Instance.GetDatabase<SystemConfigDatabase>().FindAll();
    }
    internal override void ReadData()
    {
        base.ReadData();
        systemStoreData = ConfigManager.Instance.ReadFile<Dictionary<int, SystemStoreData>>("systemStoreData.data");
        if (systemStoreData == null)
            systemStoreData = new Dictionary<int, SystemStoreData>();
    }
    internal override void SaveData()
    {
        base.SaveData();
        ConfigManager.Instance.WriteFile(systemStoreData, "systemStoreData.data");
    }
}
