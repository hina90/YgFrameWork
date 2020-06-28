using System;
using System.Collections;
using System.Collections.Generic;
using Tool.Database;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 按钮弱引导点击
/// </summary>
public class GuideWeakClick : GuideTask
{
    private readonly string clawPrefab = "Claw";
    private readonly string tipPrefab = "GuideTip";
    private GameObject clawObj;
    private GameObject guideTipObj;
    private UIBase parentUI;

    public GuideWeakClick(GuideConfigData guideConfig) : base(guideConfig)
    {
        parentUI = UIManager.Instance.GetResUI(guideConfig.ui);
        Transform parentTrans = parentUI.transform;
        if (guideConfig.showFinger == 0) return;
        clawObj = ResourceManager.Instance.GetResourceInstantiate(clawPrefab, parentTrans, ResouceType.PrefabItem);
        GuideManager.Instance.AddGuideListener(GameEvent.FINISH_WEAKGUIDE, Complete);
        clawObj.transform.localPosition = new Vector2(guideConfig.fingerPos[0], guideConfig.fingerPos[1]);
        clawObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, guideConfig.fingerAngle));

        if (guideConfig.waitTime != 0)
        {
            parentUI.StartCoroutine(DelayCall(guideConfig.waitTime));
        }

        if (guideConfig.dialogue.Length <= 0) return;
        GameObject tipPrefabObj = ResourceManager.Instance.GetResource(tipPrefab, ResouceType.PrefabItem);
        guideTipObj = UnityEngine.Object.Instantiate(tipPrefabObj, parentTrans);
        guideTipObj.transform.Find("content").GetComponent<Text>().text = guideConfig.dialogue[0];
    }

    IEnumerator DelayCall(float time)
    {
        yield return new WaitForSeconds(time);
        Complete();
    }

    /// <summary>
    /// 完成当前弱引导步骤
    /// </summary>
    /// <param name="arg"></param>
    private void Complete(object[] arg)
    {
        Complete();
    }

    public override void Complete()
    {
        base.Complete();
        GuideManager.Instance.FinishWeakGuide();
        Dispose();
    }

    public override void Dispose()
    {
        base.Dispose();
        if (clawObj != null)
        {
            UnityEngine.Object.Destroy(clawObj);
            clawObj = null;
        }
        if (guideTipObj != null)
        {
            UnityEngine.Object.Destroy(guideTipObj);
            guideTipObj = null;
        }
    }
}
