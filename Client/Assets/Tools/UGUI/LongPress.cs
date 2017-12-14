
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LongPress : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler
{
    Action OnPress;
    Action EndPress;
    Action OnClick;
    bool IsDown = false;
    bool IsOnPress = false;
    float ClickTime;
    float PressTime = .5f;
    public void Init(Action p, Action e, Action c)
    {
        OnPress = p;
        EndPress = e;
        OnClick = c;
    }
    void update()
    {
        if (!IsDown) return;
        if ((Time.time - ClickTime) >= PressTime)
        {
            if (OnPress != null) OnPress();
            IsDown = false;
            IsOnPress = true;
        }
    }
    new void OnEnable()
    {
        Manage.Instance.AddUpdate(update);
        base.OnEnable();
    }
    new void OnDisable()
    {
        Manage.Instance.RemoveUpdate(update);
        base.OnDisable();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        ClickTime = Time.time;
        IsDown = true;
        IsOnPress = false;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        IsDown = false;
        if (EndPress != null)
            EndPress();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (EndPress != null)
            EndPress();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsOnPress)
        {
            IsOnPress = false;
            return;
        }
        if (OnClick != null) OnClick();
    }
}