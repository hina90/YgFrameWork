using System.Collections;
using Tool.Database;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 按钮强制点击引导
/// </summary>
public class GuideForceClick : GuideTask
{
    enum BtnMaskType { Circle = 1, Rect }

    private Button btn;
    private CircleGuidanceController circleController;
    private RectGuidanceController rectController;

    private const string dialogueItem = "GuideDialogItem";

    public GuideForceClick(Transform root, GuideConfigData guideData, MonoBehaviour mono) : base(root, guideData, mono)
    {
        circleController = root.Find("Circle").GetComponent<CircleGuidanceController>();
        rectController = root.Find("Rect").GetComponent<RectGuidanceController>();

        if (m_GuideConfig.needCondition == 1)
        {
            GuideManager.Instance.AddGuideListener(GameEvent.AUTO_FINISH, Complete);
        }
    }

    public override void Execute()
    {
        base.Execute();
        if (string.IsNullOrEmpty(m_GuideConfig.btnName)) return;
        btn = GuideManager.Instance.GetButton(m_GuideConfig.btnName);
        if (btn == null) return;
        switch ((BtnMaskType)m_GuideConfig.btnMaskType)
        {
            case BtnMaskType.Circle:
                circleController.gameObject.SetActive(true);
                circleController.Target = btn.GetComponent<Image>();
                break;
            case BtnMaskType.Rect:
                rectController.gameObject.SetActive(true);
                rectController.Target = btn.GetComponent<Image>();
                break;
            default:
                break;
        }
        if (m_GuideConfig.needCondition == 1) return;
        btn.onClick.AddListener(Complete);
    }

    /// <summary>
    /// 完成指定条件结束当前引导
    /// </summary>
    /// <param name="arg"></param>
    private void Complete(object[] arg)
    {
        Complete();
        if (m_GuideConfig.Id == 1006)
        {
            GameManager.Instance.NormalGuest();
        }
    }

    public override void Complete()
    {
        base.Complete();
        GuideManager.Instance.RemoveBtn(m_GuideConfig.btnName);
        circleController.gameObject.SetActive(false);
        rectController.gameObject.SetActive(false);

        if (m_GuideConfig.needCondition == 1)
        {
            GuideManager.Instance.RemoveGuideListener(GameEvent.AUTO_FINISH, Complete);
        }
        else
        {
            btn.onClick.RemoveListener(Complete);
        }

        GuideManager.Instance.BroadcastGuideEvent(GameEvent.FINISH_GUIDE);
        Dispose();
    }

    public override void Dispose()
    {
        base.Dispose();
        if (m_Obj == null) return;
        Object.Destroy(m_Obj);
    }
}
