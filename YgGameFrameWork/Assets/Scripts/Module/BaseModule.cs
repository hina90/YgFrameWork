
public class BaseModule
{
    private GameModuleManager moduleManager;
    protected T GetModule<T>() where T : BaseModule, new() => moduleManager.GetModule<T>();

    internal virtual void Init(GameModuleManager moduleManager)
    {
        this.moduleManager = moduleManager;
        ReadData();
    }
    internal virtual void OnUpdate(float deltalTime) { }
    internal virtual void ReadData() { }
    internal virtual void SaveData() { }
    internal virtual void ResetData() { }
}
