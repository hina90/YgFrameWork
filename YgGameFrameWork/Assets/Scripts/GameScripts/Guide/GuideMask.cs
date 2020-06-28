using System;
using System.Collections;
using System.Collections.Generic;
using Tool.Database;
using UnityEngine;
using UnityEngine.UI;

public class GuideMask : GuideTask
{
    private RectGuidanceController rectController;
    private Image imgMask;

    public GuideMask(Transform root, GuideConfigData guideConfig, MonoBehaviour mono) : base(root, guideConfig, mono)
    {
        rectController = root.Find("Rect").GetComponent<RectGuidanceController>();
        imgMask = m_Trans.Find("ImgMask").GetComponent<Image>();
        GuideManager.Instance.AddGuideListener(GameEvent.SHOW_MASK, Complete);
    }

    public override void Execute()
    {
        base.Execute();
        rectController.gameObject.SetActive(true);
        rectController.Target = imgMask;
    }

    private void Complete(object[] arg)
    {
        Complete();
    }

    public override void Complete()
    {
        base.Complete();
        rectController.gameObject.SetActive(false);
        GuideManager.Instance.RemoveGuideListener(GameEvent.SHOW_MASK, Complete);
        GuideManager.Instance.BroadcastGuideEvent(GameEvent.FINISH_GUIDE);
        Dispose();
    }

    public override void Dispose()
    {
        base.Dispose();
        if (m_Obj == null) return;
        UnityEngine.Object.Destroy(m_Obj);
    }
}
