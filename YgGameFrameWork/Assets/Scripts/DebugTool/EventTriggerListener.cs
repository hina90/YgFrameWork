using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
public class EventTriggerListener : UnityEngine.EventSystems.EventTrigger
{
    public delegate void PointDelegate(PointerEventData go);
    public delegate void BaseDelegate(BaseEventData go);
    public PointDelegate onClick;
    public PointDelegate onDBClick;
    public PointDelegate onDown;
    public PointDelegate onEnter;
    public PointDelegate onExit;
    public PointDelegate onUp;
    public BaseDelegate onSelect;
    public BaseDelegate onUpdateSelect;
    public PointDelegate onDrag;
    public PointDelegate onEndDrag;
    public float clickTime = 0;
    static public EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null) listener = go.AddComponent<EventTriggerListener>();
        return listener;
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        bool isDoubleClick = false;
        if (clickTime != 0)
        {
            if (Time.realtimeSinceStartup - clickTime <= 0.2f)
            {
                isDoubleClick = true;
                clickTime = 0;
            }
            else
            {
                clickTime = Time.realtimeSinceStartup;
            }
        }
        else
        {
            clickTime = Time.realtimeSinceStartup;
        }

        if (onDBClick != null && isDoubleClick)
        {
            onDBClick(eventData);
        }
        else if (onClick != null)
        {
            onClick(eventData);
        }
        
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null) onDown(eventData);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null) onEnter(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null) onExit(eventData);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null) onUp(eventData);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(eventData);
    }
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null) onUpdateSelect(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null) onDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag != null) onEndDrag(eventData);
    }
}