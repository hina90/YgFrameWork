using System.Collections;
using System.Collections.Generic;
using Tool.Database;
using UnityEngine;

/// <summary>
/// 特殊任务引导
/// </summary>
public class GuideSpecialTask : GuideTask
{
    private Transform parentTrans;

    public GuideSpecialTask(Transform root, GuideConfigData guideConfig, MonoBehaviour mono) : base(root, guideConfig, mono)
    {
        parentTrans = GameObject.Find(((FacilitiesType)guideConfig.area).ToString()).transform;
    }

    public override void Execute()
    {
        base.Execute();
        ResourceManager.Instance.GetResourceInstantiate(m_GuideConfig.guideResources,parentTrans,ResouceType.Guide);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
