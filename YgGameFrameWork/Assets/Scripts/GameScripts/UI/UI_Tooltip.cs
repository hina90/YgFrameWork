using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tooltip : UIBase
{
    private Text m_TxtContent;
    private Button m_BtnSure, m_BtnCancel, m_BtnCenter;
    private GameObject m_Tip1, m_Tip2;

    private Callback_1<bool> sureCallBack, cancelCallBack;
    private Callback okCallBack;

    public override void Init()
    {
        Layer = LayerMenue.MSGBOX;

        m_TxtContent = Find<Text>(gameObject, "TxtContent");
        m_BtnSure = Find<Button>(gameObject, "Btn_Sure");
        m_BtnCancel = Find<Button>(gameObject, "Btn_Cancel");
        m_BtnCenter = Find<Button>(gameObject, "Btn_Ok");
        m_Tip1 = Find(gameObject, "Tips1");
        m_Tip2 = Find(gameObject, "Tips2");

        RegisterBtnEvent();
    }

    protected override void Enter()
    {
        base.Enter();
        m_Tip1.SetActive(false);
        m_Tip2.SetActive(false);
        if (param.Length <= 0) return;
        ToolTipData tipData = (ToolTipData)param[0];
        m_TxtContent.text = tipData.Content;
        sureCallBack = tipData.SureCallBack;
        cancelCallBack = tipData.CancelCallBack;
        okCallBack = tipData.OkCallBack;
        switch (tipData.BtnType)
        {
            case ButtonType.One:
                m_Tip2.SetActive(true);
                break;
            case ButtonType.Two:
                m_Tip1.SetActive(true);
                break;
            default:
                break;
        }
    }

    public override void Release()
    {
        base.Release();
    }

    private void RegisterBtnEvent()
    {
        m_BtnSure.onClick.AddListener(() =>
        {
            bool isOk = false;
            if (sureCallBack != null)
            {
                isOk = sureCallBack();
            }
            if (isOk) UIManager.Instance.CloseUI(Layer);
        });

        m_BtnCancel.onClick.AddListener(() =>
        {
            cancelCallBack?.Invoke();
            UIManager.Instance.CloseUI(Layer);
        });
        m_BtnCenter.onClick.AddListener(() =>
        {
            okCallBack?.Invoke();
            UIManager.Instance.CloseUI(Layer);
        });
    }
}

/// <summary>
/// 提示框结构体
/// </summary>
public struct ToolTipData
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title;
    /// <summary>
    /// 内容
    /// </summary>
    public string Content;
    /// <summary>
    /// 确认按钮回调
    /// </summary>
    public Callback_1<bool> SureCallBack;
    /// <summary>
    /// 取消按钮回调
    /// </summary>
    public Callback_1<bool> CancelCallBack;
    /// <summary>
    /// 中间确认按钮回调
    /// </summary>
    public Callback OkCallBack;
    /// <summary>
    /// 按钮显示类型
    /// </summary>
    public ButtonType BtnType;
    /// <summary>
    /// 左侧按钮标题
    /// </summary>
    public string LeftBtnTitle;
    /// <summary>
    /// 中间按钮标题
    /// </summary>
    public string CenterBtnTitle;
    /// <summary>
    /// 右侧按钮标题
    /// </summary>
    public string RightBtnTitle;
}

public enum ButtonType
{
    Two,
    One,
}
