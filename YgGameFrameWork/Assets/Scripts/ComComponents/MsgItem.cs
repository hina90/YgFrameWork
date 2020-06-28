using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class MsgItem : MonoBehaviour
{
    public float moveDis = 150;
    public float showTime = 1.5f;
    private Vector3 initPos;
    private MaskableGraphic[] graphics;
    private Text txtMsg;

    void Awake()
    {
        initPos = transform.localPosition;
        txtMsg = GetComponentInChildren<Text>();
        graphics = GetComponentsInChildren<MaskableGraphic>();
        foreach (var item in graphics)
        {
            item.color = new Color(item.color.r, item.color.g, item.color.b, 0);
        }
    }

    /// <summary>
    /// 显示Tip信息
    /// </summary>
    /// <param name="msg">信息内容</param>
    /// <param name="delayTime">延迟显示时间</param>
    /// <param name="colorHtml">文本颜色</param>
    public void Show(string msg, float delayTime, string colorHtml)
    {
        txtMsg.color = colorHtml.ParseHtmlColor();
        StartCoroutine(DelayMove(msg, delayTime));
    }

    /// <summary>
    /// 延迟显示协程
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="delayTime"></param>
    /// <returns></returns>
    IEnumerator DelayMove(string msg, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        txtMsg.text = msg;
        MoveUp();
    }

    /// <summary>
    /// 信息移动行为
    /// </summary>
    private void MoveUp()
    {
        foreach (var item in graphics)
        {
            item.color = new Color(item.color.r, item.color.g, item.color.b, 1);
        }
        transform.SetAsLastSibling();

        transform.DOLocalMoveY(initPos.y + moveDis, showTime).OnComplete(() =>
        {
            Dispose();
            TipManager.Instance.Recycle(gameObject);
        });

        foreach (var item in graphics)
        {
            item.DOFade(0, showTime).SetEase(Ease.InCubic);
        }
    }

    /// <summary>
    /// 回收释放
    /// </summary>
    private void Dispose()
    {
        txtMsg.text = null;
        transform.localPosition = initPos;
        transform.DOKill();
    }
}
