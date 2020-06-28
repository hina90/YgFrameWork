using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class IncomeTipItem : MonoBehaviour
{
    private TextMeshPro textMesh;
    private MaskableGraphic graphic;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        graphic = GetComponent<MaskableGraphic>();
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(int value, Vector3 pos)
    {
        transform.localPosition = pos;
        textMesh.text = "+" + value.ToString();
        graphic.DOFade(1, 0);
        Move();
    }

    private void Move()
    {
        transform.DOLocalMoveY(transform.position.y + 0.5f, 1f);
        graphic.DOFade(0, 1f).OnComplete(() =>
        {
            IncomeTipManager.Instance.Recycle(gameObject);
        });
    }
}
