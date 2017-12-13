using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTrigger : UnityEngine.EventSystems.EventTrigger
{
    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;
    public Action<PointerEventData> onDrag;
    public Action<PointerEventData> onEndDrag;
    public Action<PointerEventData> mDown;
    public Action<PointerEventData> mUP;
    public Action<PointerEventData> onPointerEnter;
    public Action<PointerEventData> onPointerExit;

    static public EventTrigger Get(GameObject go)
    {
        EventTrigger listener = go.GetComponent<EventTrigger>();
        if (listener == null) listener = go.AddComponent<EventTrigger>();
        return listener;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null) onDrag(eventData);
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag != null) onEndDrag(eventData);
        base.OnEndDrag(eventData);
    }


    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null) onClick(gameObject);
        base.OnPointerClick(eventData);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (mDown != null) mDown(eventData);
        if (onDown != null) onDown(gameObject);
        base.OnPointerDown(eventData);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onPointerEnter != null) onPointerEnter(eventData);
        if (onEnter != null) onEnter(gameObject);
        base.OnPointerEnter(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onPointerExit != null) onPointerExit(eventData);
        if (onExit != null) onExit(gameObject);
        base.OnPointerExit(eventData);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (mUP != null) mUP(eventData);
        if (onUp != null) onUp(gameObject);
        base.OnPointerUp(eventData);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(gameObject);
        base.OnSelect(eventData);
    }
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null) onUpdateSelect(gameObject);
        base.OnUpdateSelected(eventData);
    }
}