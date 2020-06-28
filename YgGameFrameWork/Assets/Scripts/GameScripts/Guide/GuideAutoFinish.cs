using System.Collections;
using Tool.Database;
using UnityEngine;

/// <summary>
/// 自动完成引导类
/// </summary>
public class GuideAutoFinish : GuideTask
{
    public GuideAutoFinish(Transform root, GuideConfigData guideData, MonoBehaviour mono) : base(root, guideData, mono)
    {
        GuideManager.Instance.AddGuideListener(GameEvent.AUTO_FINISH, Complete);
    }


    public override void Execute()
    {
        base.Execute();
        if (m_GuideConfig.waitTime != 0)
        {
            mono.StartCoroutine(DelayCallComplete());
        }
    }

    IEnumerator DelayCallComplete()
    {
        yield return new WaitForSeconds(m_GuideConfig.waitTime);
        Complete();
    }

    private void Complete(object[] arg)
    {
        Complete();
    }

    public override void Complete()
    {
        base.Complete();
        GuideManager.Instance.BroadcastGuideEvent(GameEvent.FINISH_GUIDE);
        Dispose();
    }

    public override void Dispose()
    {
        base.Dispose();
        GuideManager.Instance.RemoveGuideListener(GameEvent.AUTO_FINISH, Complete);
        if (m_Obj == null) return;
        Object.Destroy(m_Obj);
    }
}
