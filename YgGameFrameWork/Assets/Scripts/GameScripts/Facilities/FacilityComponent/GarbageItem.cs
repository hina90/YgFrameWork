using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 垃圾
/// </summary>
public class GarbageItem : MonoBehaviour
{
    private SpriteRenderer spriteRender;

    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (Utils.CheckIsClickOnUI()) return;
        GameManager.Instance.RemoveGarbageFromList(this);
        GuideManager.Instance.BroadcastGuideEvent(GameEvent.CLEAR_GARBAGE);
    }

    public void ShowGarbageItem(Sprite sp)
    {
        spriteRender.sprite = sp;
    }
}
