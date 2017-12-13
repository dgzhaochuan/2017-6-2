using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudText : PoolObj
{

    public float time=2;
    public float speed = 10;

    float startTime = 0;
    Text text;
    TweenerAlpha alpha_tweener;
    TweenerScale scale_tweener;
    Transform target;
    Vector3 off;

    void OnUpdate()
    {
        Vector3 point = Camera.main.WorldToScreenPoint(target.position);
        off += Vector3.up * speed * Time.deltaTime;
        transform.position = point + off;
        if ((Time.time - startTime) >= time && alpha_tweener.IsPlay == false)
        {
            OnClose();
            return;
        }
    }
    protected  override  void OnDisable()
    {
        base.OnDisable();
        Manage.Instance.RemoveUpdate(OnUpdate);
    }
    private void OnEnable()
    {
        Manage.Instance.AddUpdate(OnUpdate);
    }
    private void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Reset(Transform target,int value,HudTextEnum type)
    {
        off = Vector2.zero;
        this.target = target;
        startTime = Time.time;
        Color color = Color.white;
        string str=(value>0?"":"-")+value;
        switch (type)
        {
            case HudTextEnum.hp:
                if (value > 0) color = Color.green;
                else
                    color = Color.red;
                break;
            case HudTextEnum.mp:
                color = Color.blue;
                break;
        }
        text.text = str;
        text.color = color;
        Open();
    }
    public void Open()
    {
        gameObject.SetActive(true);
        alpha_tweener.ToOpen();
        scale_tweener.ToOpen();
    }
    public void OnClose()
    {
        alpha_tweener.OnClose();
        scale_tweener.OnClose();
    }

    public override void Init()
    {
        text = transform.Find("Text").GetComponent<Text>();
        alpha_tweener = GetComponent<TweenerAlpha>();
        scale_tweener = GetComponent<TweenerScale>();
        alpha_tweener.OnCloseEndEvent = Disable;
    }
}
