using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 对话消息类型
/// </summary>
public enum DialogType
{
    Text,
    Texture,
}

/// <summary>
/// 文本类型消息
/// </summary>
public class DialogItem
{
    private Image m_ImgHead;
    private Text m_Msg;
    private LayoutElement m_LayElement;
    private ContentSizeFitter m_SizeFitter;
    public DialogType dialogType;

    public DialogItem(Transform transform)
    {
        m_ImgHead = transform.Find("Img_Head").GetComponent<Image>();
        m_Msg = transform.Find("Msg/Text").GetComponent<Text>();
        m_LayElement = transform.GetComponent<LayoutElement>();
        m_SizeFitter = m_Msg.GetComponent<ContentSizeFitter>();
    }

    public void BindData(string content)
    {
        m_Msg.text = content;
        if (m_Msg.preferredWidth < 170)
        {
            m_SizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
        m_LayElement.preferredHeight = Mathf.Max(50, m_Msg.preferredHeight + 10);
    }
}
