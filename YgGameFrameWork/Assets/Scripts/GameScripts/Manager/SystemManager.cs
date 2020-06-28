public class SystemManager : Singleton<SystemManager>
{
    private SystemModule systemModule = GameModuleManager.Instance.GetModule<SystemModule>();
    public void Init()
    {
        
    }
    public bool GetSystemIsUnlock(int systemID)
    {
        if (systemModule.GetSystemStoreData(systemID).isUnlock)
            return true;
        return false;
    }
    public bool GetSystemIsOpen(int systemID)
    {
        if (systemModule.GetSystemStoreData(systemID).isOpen)
            return true;
        return false;
    }
    public SystemData GetSystemData(int systemID)
    {
        return systemModule.GetSystemData(systemID);
    }
    public void SaveData()
    {
        systemModule.SaveData();
    }
}
