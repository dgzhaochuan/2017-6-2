
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : BaseManaget
{

    public static event Action OnClick;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
#if IPHONE || ANDROID
			if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
            if (EventSystem.current.IsPointerOverGameObject())
#endif
                return;
        }

        if (Manage.Instance.IsBattle)
            if (BattleManager.gameState == GameState.noInput) return;
        bool click = Input.GetMouseButtonDown(0);
        if (click)
        {
            if (OnClick != null)
                OnClick();
        }
    }

    public override void OnInit()
    {
    }
    public override void Close()
    {
        OnClick=null;
        base.Close();
    }
    
}