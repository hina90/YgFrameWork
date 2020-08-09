using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UUIEventListener : MonoBehaviour,
                               IPointerClickHandler,
                               IPointerDownHandler,
                               IPointerEnterHandler,
                               IPointerExitHandler,
                               IPointerUpHandler,
                               ISelectHandler,
                               IUpdateSelectedHandler,
                               IDeselectHandler,
                               IBeginDragHandler,
                               IDragHandler,
                               IEndDragHandler,
                               IDropHandler,
                               IScrollHandler,
                               IMoveHandler
{
    public delegate void VoidDelegate(GameObject go);
    public delegate void VoidDragDelegate(GameObject go, bool isDrag, float detal_x, float detal_y);
    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;
    public VoidDelegate onDeSelect;
    public VoidDragDelegate onDrag;
    public VoidDelegate onDragEnd;
    public VoidDelegate onDragBegin;
    public VoidDelegate onDrop;
    public VoidDelegate onScroll;
    public VoidDelegate onMove;

    public void OnPointerClick(PointerEventData eventData) { if (onClick != null) onClick(gameObject); }
    public void OnPointerDown(PointerEventData eventData) { if (onDown != null) onDown(gameObject); }
    public void OnPointerEnter(PointerEventData eventData) { if (onEnter != null) onEnter(gameObject); }
    public void OnPointerExit(PointerEventData eventData) { if (onExit != null) onExit(gameObject); }
    public void OnPointerUp(PointerEventData eventData) { if (onUp != null) onUp(gameObject); }
    public void OnSelect(BaseEventData eventData) { if (onSelect != null) onSelect(gameObject); }
    public void OnUpdateSelected(BaseEventData eventData) { if (onUpdateSelect != null) onUpdateSelect(gameObject); }
    public void OnDeselect(BaseEventData eventData) { if (onDeSelect != null) onDeSelect(gameObject); }
    public void OnDrag(PointerEventData eventData) { if (onDrag != null) onDrag(gameObject, eventData.dragging, eventData.delta.x, eventData.delta.y); }
    public void OnEndDrag(PointerEventData eventData) { if (onDragEnd != null) onDragEnd(gameObject); }
    public void OnBeginDrag(PointerEventData eventData) { if (onDragBegin != null) onDragBegin(gameObject); }
    public void OnDrop(PointerEventData eventData) { if (onDrop != null) onDrop(gameObject); }
    public void OnScroll(PointerEventData eventData) { if (onScroll != null) onScroll(gameObject); }
    public void OnMove(AxisEventData eventData) { if (onMove != null) onMove(gameObject); }


    void OnPointerClick(GameObject go) { if (onClick != null) onClick(go); }
    void OnPointerDown(GameObject go) { if (onDown != null) onDown(go); }
    void OnPointerEnter(GameObject go) { if (onEnter != null) onEnter(go); }
    void OnPointerExit(GameObject go) { if (onExit != null) onExit(go); }
    void OnPointerUp(GameObject go) { if (onUp != null) onUp(go); }
    void OnSelect(GameObject go) { if (onSelect != null) onSelect(go); }
    void OnUpdateSelected(GameObject go) { if (onUpdateSelect != null) onUpdateSelect(go); }
    void OnDeselect(GameObject go) { if (onDeSelect != null) onDeSelect(go); }
    void OnDrag(GameObject go, bool isDrag, float detal_x, float detal_y) { if (onDrag != null) onDrag(go, isDrag, detal_x, detal_y); }
    void OnEndDrag(GameObject go) { if (onDragEnd != null) onDragEnd(go); }
    void OnDrop(GameObject go) { if (onDrop != null) onDrop(go); }
    void OnBeginDrag(GameObject go) { if (onDragBegin != null) onDragBegin(go); }
    void OnScroll(GameObject go) { if (onScroll != null) onScroll(go); }
    void OnMove(GameObject go) { if (onMove != null) onMove(go); }


    static public UUIEventListener Get(GameObject go)
    {
        UUIEventListener listener = go.GetComponent<UUIEventListener>();
        if (listener == null) listener = go.AddComponent<UUIEventListener>();

        return listener;
    }

    public void RegOnClick(VoidDelegate cb)
    {
        UUIEventListener.Get(gameObject).onClick = OnPointerClick;
        onClick = cb;
    }
    public void RegOnDown(VoidDelegate cb)
    {
        UUIEventListener.Get(gameObject).onDown = OnPointerDown;
        onDown = cb;
    }
    public void RegOnEnter(VoidDelegate cb)
    {
        UUIEventListener.Get(gameObject).onEnter = OnPointerEnter;
        onEnter = cb;
    }
    public void RegOnExit(VoidDelegate cb)
    {
        UUIEventListener.Get(gameObject).onExit = OnPointerExit;
        onExit = cb;
    }
    public void RegOnUp(VoidDelegate cb)
    {
        UUIEventListener.Get(gameObject).onUp = OnPointerUp;
        onUp = cb;
    }
    public void RegOnSelect(VoidDelegate cb)
    {
        UUIEventListener.Get(gameObject).onSelect = OnSelect;
        onSelect = cb;
    }
    public void RegOnUpdateSelected(VoidDelegate cb)
    {
        UUIEventListener.Get(gameObject).onUpdateSelect = OnUpdateSelected;
        onUpdateSelect = cb;
    }
    public void RegOnDeselect(VoidDelegate cb)
    {
        UUIEventListener.Get(gameObject).onDeSelect = OnDeselect;
        onDeSelect = cb;
    }
    public void RegOnDrag(VoidDragDelegate cb)
    {
        UUIEventListener.Get(gameObject).onDrag = OnDrag;
        onDrag = cb;
    }
    public void RegOnEndDrag(VoidDelegate cb)
    {
        UUIEventListener.Get(gameObject).onDragEnd = OnEndDrag;
        onDragEnd = cb;
    }
    public void RegOnDrop(VoidDelegate cb)
    {
        UUIEventListener.Get(gameObject).onDrop = OnDrop;
        onDrop = cb;
    }
    public void RegOnBeginDrag(VoidDelegate cb)
    {
        UUIEventListener.Get(gameObject).onDragBegin = OnBeginDrag;
        onDragBegin = cb;
    }

    public void RegOnScroll(VoidDelegate cb)
    {
        UUIEventListener.Get(gameObject).onScroll = OnScroll;
        onScroll = cb;
    }
    public void RegOnMove(VoidDelegate cb)
    {
        UUIEventListener.Get(gameObject).onMove = OnMove;
        onMove = cb;
    }
}
