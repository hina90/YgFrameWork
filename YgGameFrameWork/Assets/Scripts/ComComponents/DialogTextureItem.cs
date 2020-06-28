using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 图片类型消息
/// </summary>
public class DialogTextureItem
{
    private Image m_ImgHead;
    private Image m_ImgMsg;
    private LayoutElement m_LayElement;

    public DialogTextureItem(Transform transform)
    {
        m_ImgHead = transform.Find("Img_Head").GetComponent<Image>();
        m_ImgMsg = transform.Find("Msg").GetComponent<Image>();
        m_LayElement = transform.GetComponent<LayoutElement>();
    }

    internal void BindData(string texName)
    {
        m_ImgMsg.overrideSprite = ResourceManager.Instance.GetSpriteResource(texName, ResouceType.Icon);
        m_ImgMsg.SetNativeSize();
        m_LayElement.preferredHeight = Mathf.Max(50, m_ImgMsg.preferredHeight);
    }
}
