using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class EventSystem: UnityEngine.EventSystems.EventSystem
{
    public static EventSystem _current;
    public new static EventSystem current
    {
        get
        {
            if (_current == null)
                _current = FindObjectOfType<EventSystem>();
            if (_current == null)
            {
                GameObject game = new GameObject("Event_System");
                _current = game.AddComponent<EventSystem>();
                game.AddComponent<StandaloneInputModule>();
            }
            return _current;
        }
    }

    public delegate bool Touch(Vector2 pos);
    private List<Touch> TouchDown=new List<Touch>();


    public void AddDown(Touch function)
    {
        TouchDown.Add(function);
    }
    public void ReDown(Touch function)
    {
        TouchDown.Remove(function);     
    }
    protected override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TouchDown.Count > 0)
            {
                Touch function = TouchDown.Last();
                if (!function(Input.mousePosition))
                {
                    TouchDown.ReLast();
                }
            }
        }
        base.Update();
    }
}