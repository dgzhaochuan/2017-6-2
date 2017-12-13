

using System;
using UnityEngine;
public class AnimatorManager : MonoBehaviour
{
    public const string Attack = "Attack";
    public const string Hit = "Hit";
    public const string Run = "Run";
    public const string Death = "Death";


    event Action<int> EventInt;
     event Action<float> EventFloat;
     event Action<string> EventString;
     event Action<UnityEngine.Object> EventObj;

    new private Animator animation;

    private void Awake()
    {
        animation = GetComponent<Animator>();
    }

    public void SetTrigger(string animation_clip)
    {
        animation.SetTrigger(animation_clip);
        //animation.Play(animation_clip,0);
    }
    public void SetBool(string animation_clip,bool b)
    {
        animation.SetBool(animation_clip,b);
    }

    protected void IntEvent(int msg) { if (EventInt != null) EventInt(msg); }
    protected void StringEvent(string msg) { if (EventString != null) EventString(msg); }
    protected void FolatEvent(float msg) { if (EventFloat != null) EventFloat(msg); }
    protected void ObjEvent(UnityEngine.Object msg) { if (EventObj != null) EventObj(msg); }

    public void AddEvent(Action<int> EventInt, Action<float> EventFloat, Action<string> EventString, Action<UnityEngine.Object> EventObj)
    {
        ReEvent(EventInt,EventFloat,EventString,EventObj);
        this.EventInt += EventInt;
        this.EventFloat += EventFloat;
        this.EventString += EventString;
        this.EventObj += EventObj;
    }
    public void ReEvent(Action<int> EventInt, Action<float> EventFloat, Action<string> EventString, Action<UnityEngine.Object> EventObj)
    {
        this.EventInt -= EventInt;
        this.EventFloat -= EventFloat;
        this.EventString -= EventString;
        this.EventObj -= EventObj;
    }
}