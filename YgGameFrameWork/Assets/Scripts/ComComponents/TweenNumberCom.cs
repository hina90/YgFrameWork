using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// 伤害数字组件
/// </summary>
public class TweenNumberCom : MonoBehaviour
{
    private Text showText;

    private void Awake()
    {
        showText = GetComponent<Text>();
    }
    /// <summary>
    /// 跳动
    /// </summary>
    public void TweenJump(int content)
    {
        showText.text = content.ToString();
        Sequence mySequence = DOTween.Sequence();
        Tweener move = transform.DOMoveY(transform.position.y + 50, 0.2f);
        Tween scale1 = transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.1f);
        Tween scale2 = transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.1f);
        mySequence.Insert(0, move)
            .Insert(0.3f, scale1)
            .Insert(0.3f, scale2)
            .OnComplete(()=>
            {
                Release();
            });
    }
    /// <summary>
    /// 释放 
    /// </summary>
    public void Release()
    {
        Destroy(gameObject);
    }
}
