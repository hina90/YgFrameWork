using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Com_GuideGarbage : MonoBehaviour
{
    private SpriteRenderer spriteRender;

    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (Utils.CheckIsClickOnUI()) return;
        int count = transform.parent.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(transform.parent.GetChild(i).gameObject);
        }
        GuideManager.Instance.BroadcastGuideEvent(GameEvent.CLEAR_GARBAGE);
    }
}
