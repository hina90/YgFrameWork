using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using Tool.Database;
using UnityEngine;
using UnityEngine.UI;

public class GuideDialogue : GuideTask
{
    private GameObject dialogue;
    private Text content;
    private SkeletonGraphic petAni;
    private Button btnHide;
    private bool isShow;
    private GameObject guideResourceObj;
    private const float dialogueShowTime = 2.6f;

    public GuideDialogue(Transform root, GuideConfigData guideConfig, MonoBehaviour mono) : base(root, guideConfig, mono)
    {
        dialogue = m_Trans.Find("dialogue").gameObject;
        content = m_Trans.Find("dialogue/Content").GetComponent<Text>();
        petAni = m_Trans.Find("dialogue/Pet").GetComponent<SkeletonGraphic>();
        btnHide = m_Trans.Find("dialogue/Bg").GetComponent<Button>();
        if (!string.IsNullOrEmpty(guideConfig.guideResources))
        {
            guideResourceObj = ResourceManager.Instance.GetResourceInstantiate(m_GuideConfig.guideResources, root, ResouceType.Guide);
            guideResourceObj.transform.SetAsFirstSibling();
        }

        //btnHide.onClick.AddListener(() =>
        //{
        //    if (!isShow) return;
        //    FinishDialogue();
        //});

        if (m_GuideConfig.Id == 1024)
        {
            GuideManager.Instance.forbidTouch = true;
        }
    }

    public override void Execute()
    {
        base.Execute();
        StartGuideDialogue();
    }

    public override void Complete()
    {
        base.Complete();
        GuideManager.Instance.BroadcastGuideEvent(GameEvent.FINISH_GUIDE);
    }

    public override void Dispose()
    {
        base.Dispose();
        if (m_Obj == null) return;
        UnityEngine.Object.Destroy(m_Obj);
        if (guideResourceObj == null) return;
        UnityEngine.Object.Destroy(guideResourceObj);
    }

    /// <summary>
    /// 开始剧情对话
    /// </summary>
    private void StartGuideDialogue()
    {
        if (string.IsNullOrEmpty(m_GuideConfig.dialogue[0])) return;
        ShowDialogue();
    }

    /// <summary>
    /// 显示对话面板
    /// </summary>
    private void ShowDialogue()
    {
        ShowBubble();
        dialogue.transform.DOLocalMoveY(-530, 1f).OnComplete(() =>
        {
            petAni.AnimationState.SetAnimation(0, m_GuideConfig.anim, true);
            isShow = true;
        });
        int length = m_GuideConfig.dialogue.Length;
        if (length == 2)
        {
            mono.StartCoroutine(DelayToggleDialogue(dialogueShowTime));
        }
        float time = dialogueShowTime * length;
        mono.StartCoroutine(DelayCall(time));

    }

    /// <summary>
    /// 延迟回调
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator DelayCall(float time)
    {
        yield return new WaitForSeconds(time);
        FinishDialogue();
    }

    /// <summary>
    /// 延迟切换对话内容
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator DelayToggleDialogue(float time)
    {
        yield return new WaitForSeconds(time);
        content.text = m_GuideConfig.dialogue[1];
    }

    /// <summary>
    /// 显示对话内容
    /// </summary>
    private void ShowBubble()
    {
        dialogue.SetActive(true);
        content.text = m_GuideConfig.dialogue[0];
    }

    /// <summary>
    /// 完成对话
    /// </summary>
    private void FinishDialogue()
    {
        dialogue.transform.DOLocalMoveY(-1229, 0.5f).OnComplete(() =>
        {
            isShow = false;
            btnHide.onClick.RemoveAllListeners();
            Complete();
            Dispose();
        });
    }
}
