using Tool.Database;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public enum GuideType
{
    /// <summary>
    /// 按钮
    /// </summary>
    ClickButton = 1,
    /// <summary>
    /// 剧情对话
    /// </summary>
    Dialogue,
    /// <summary>
    /// 特殊任务
    /// </summary>
    SpecialTask,
    /// <summary>
    ///自动完成任务
    /// </summary>
    AutoFinish,
    /// <summary>
    /// 引导遮罩
    /// </summary>
    GuideMask,
    /// <summary>
    /// 弱引导手指指引
    /// </summary>
    WeakFinger,
}

public class UI_Guide : UIBase
{
    private int curGuideId;
    private GuideConfigData guideData;
    private Transform Img_Claw;

    public override void Init()
    {
        base.Init();
        Layer = LayerMenue.GUIDE;
        Img_Claw = Find<Transform>(gameObject, "Claw");
        AddGuideListener();
    }

    protected override void Enter()
    {
        base.Enter();
    }

    private void AddGuideListener()
    {
        GuideManager.Instance.AddGuideListener(GameEvent.ENTER_GUIDE, EnterGuide);
        GuideManager.Instance.AddGuideListener(GameEvent.FINISH_GUIDE, FinishGuide);
    }

    private void EnterGuide(params object[] args)
    {
        if (GuideManager.Instance.guideQueue.Count > 0) return;
        Show();
        transform.SetAsLastSibling();
        curGuideId = GuideManager.Instance.CurGuideId;
        guideData = ConfigDataManager.Instance.GetDatabase<GuideConfigDatabase>().GetDataByKey(curGuideId.ToString());
        GuideType guideType = (GuideType)guideData.type;
        GuideTask guideTask = default;
        switch (guideType)
        {
            case GuideType.ClickButton:
                guideTask = new GuideForceClick(transform, guideData, this);
                Invoke("ShowClaw", 1f);
                break;
            case GuideType.AutoFinish:
                guideTask = new GuideAutoFinish(transform, guideData, this);
                break;
            case GuideType.SpecialTask:
                guideTask = new GuideSpecialTask(transform, guideData, this);
                Invoke("ShowClaw", 1f);
                break;
            case GuideType.Dialogue:
                guideTask = new GuideDialogue(transform, guideData, this);
                break;
            case GuideType.GuideMask:
                guideTask = new GuideMask(transform, guideData, this);
                break;
            default:
                break;
        }
        guideTask.Execute();
        GuideManager.Instance.guideQueue.Enqueue(curGuideId);
    }

    private void FinishGuide(params object[] args)
    {
        Hide();
        GuideManager.Instance.guideQueue.Dequeue();
        GuideManager.Instance.FinishGuide();

        Img_Claw.gameObject.SetActive(false);
    }

    /// <summary>
    /// 显示猫爪
    /// </summary>
    private void ShowClaw()
    {
        if (guideData.showFinger == 0) return;
        Img_Claw.gameObject.SetActive(true);
        Img_Claw.localPosition = new Vector2(guideData.fingerPos[0], guideData.fingerPos[1]);
        Img_Claw.localRotation = Quaternion.Euler(new Vector3(0, 0, guideData.fingerAngle));
    }

    public override void Release()
    {
        base.Release();
    }

    private void OnDestroy()
    {
        GuideManager.Instance.RemoveGuideListener(GameEvent.ENTER_GUIDE, EnterGuide);
        GuideManager.Instance.RemoveGuideListener(GameEvent.FINISH_GUIDE, FinishGuide);
    }
}
