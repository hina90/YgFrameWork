using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyButton : Button
{
    private Transform[] childTrans;

    protected override void Start()
    {
        base.Start();
        childTrans = GetComponentsInChildren<Transform>();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (transition != Transition.SpriteSwap || !interactable) return;
        for (int i = 0; i < childTrans.Length; i++)
        {
            if (childTrans[i].name == name) continue;
            childTrans[i].localScale = Vector3.one * 0.8f;
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (transition != Transition.SpriteSwap || !interactable) return;
        for (int i = 0; i < childTrans.Length; i++)
        {
            if (childTrans[i].name == name) continue;
            childTrans[i].localScale = Vector3.one;
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (transition != Transition.SpriteSwap || !interactable) return;
        for (int i = 0; i < childTrans.Length; i++)
        {
            if (childTrans[i].name == name) continue;
            childTrans[i].localScale = Vector3.one;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        childTrans = null;
    }
}
