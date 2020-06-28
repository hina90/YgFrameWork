using Tool.Database;
using UnityEngine;

public interface IGuideTask
{
    void Execute();
    void Complete();
    void Dispose();
}

/// <summary>
/// 引导基类
/// </summary>
public class GuideTask : IGuideTask
{
    protected GuideConfigData m_GuideConfig;
    protected GameObject m_Obj;
    protected Transform m_Root;
    protected Transform m_Trans;
    protected MonoBehaviour mono;

    public GuideTask(Transform root, GuideConfigData guideConfig, MonoBehaviour mono)
    {
        this.mono = mono;
        m_GuideConfig = guideConfig;
        m_Root = root;
        if (string.IsNullOrEmpty(m_GuideConfig.guideWindow)) return;
        GameObject prefab = ResourceManager.Instance.GetResource(m_GuideConfig.guideWindow, ResouceType.Guide);
        m_Obj = Object.Instantiate(prefab, root);
        m_Obj.transform.SetSiblingIndex(0);
        m_Trans = m_Obj.transform;
    }

    public GuideTask(GuideConfigData guideConfig) { }

    public virtual void Execute() { }

    public virtual void Complete() { }

    public virtual void Dispose() { }
}
