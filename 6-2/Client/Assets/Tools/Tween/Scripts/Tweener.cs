using System;
using UnityEngine;


public abstract class Tweener : MonoBehaviour
{
    public enum AnimatorType
    {
        one,
        loop,
        pingpong
    }

    public AnimationCurve animationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
    public AnimatorType animatorType;
    public bool StartOpen;
    public float ConsumeTime = .1f;
    public float delay = 0;
    public Action OnOpenEvent;
    public Action OnCloseEvent;
    public Action OnOpenEndEvent;
    public Action OnCloseEndEvent;
    public bool IsPlay { get { return isPlay; } }
    public bool IsOpen { get { return isOpen; } }

    bool isOpen;
    bool isPlay;
    bool isPause;
    float currentTime;
    float startTime;
    float pauseTime;
    float startPauseTime;
    public virtual void OnOpen(float DelayTime = 0)
    {
        startTime = Time.time + DelayTime;
        isOpen = true;
        isPlay = true;
        currentTime = 0;
        pauseTime = 0;
        this.delay = DelayTime;
        if (OnOpenEvent != null) OnOpenEvent();
    }
    public virtual void OnClose(float DelayTime = 0)
    {
        startTime = Time.time + DelayTime;
        isOpen = false;
        isPlay = true;
        currentTime = 0;
        pauseTime = 0;
        this.delay = DelayTime;
        if (OnCloseEvent != null) OnCloseEvent();
    }
    public virtual void OpenEnd()
    {
        if (OnOpenEndEvent != null)
            OnOpenEndEvent();
    }
    public virtual void CloseEnd()
    {
        if (OnCloseEndEvent != null)
            OnCloseEndEvent();
    }

    public void OnChange()
    {
        if (isOpen) OnClose();
        else OnOpen();
    }
    public void OnPlay()
    {
        if (isOpen) OnOpen();
        else OnClose();
    }
    public void OnStop()
    {
        isPlay = false;
        OnUpdate(animationCurve.Evaluate(isOpen ? 1 : 0));
    }
    public void OnPause()
    {
        if (!isPause)
        {
            pauseTime = 0;
            startPauseTime = Time.time;
        }
        else
        {
            pauseTime = Time.time - startPauseTime;
            print(pauseTime);
        }
        isPause = !isPause;
    }
    private void update()
    {
        if (!isPlay) return;
        if (isPause) return;

        currentTime = (Time.time - (startTime + pauseTime)) / ConsumeTime;
        currentTime = Mathf.Clamp01(currentTime);
        OnUpdate(animationCurve.Evaluate(isOpen ? currentTime : 1 - currentTime));
        if (currentTime >= 1)
        {
            if (isOpen)
            {
                OpenEnd();
            }
            else
            {
                CloseEnd();
            }
            switch (animatorType)
            {
                case AnimatorType.one:
                    isPlay = false;
                    break;
                case AnimatorType.loop:
                    OnPlay();
                    break;
                case AnimatorType.pingpong:
                    OnChange();
                    break;
            }
        }
    }
    private void Start()
    {
        if (StartOpen)
            OnPlay();
    }

    private void OnEnable()
    {
        Manage.Instance.AddUpdate(update);
    }

    private void OnDisable()
    {
        isPlay = false;
        Manage.Instance.RemoveUpdate(update);
    }

    protected abstract void OnUpdate(float factor);



    [ContextMenu("SetClose")]
    public abstract void SetClose();
    [ContextMenu("SetOpen")]
    public abstract void SetOpen();
    [ContextMenu("ToClose")]
    public virtual void ToClose()
    {
        isOpen = false;
        OnUpdate(animationCurve.Evaluate(0));
        if (OnCloseEndEvent != null) OnCloseEndEvent();
    }
    [ContextMenu("ToOpen")]
    public virtual void ToOpen()
    {
        isOpen = true;
        OnUpdate(animationCurve.Evaluate(1));
        if (OnOpenEndEvent != null) OnOpenEndEvent();
    }
    public void UpdateSlider(float value)
    {
        OnUpdate(animationCurve.Evaluate(value));
    }
}
