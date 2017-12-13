

using UnityEngine;
using System.Collections.Generic;
using System;

public class LoadWMangae : BaseManaget
{
    Queue<WObject> IdelObj = new Queue<WObject>();


    private WWW load(string path)
    {
        WObject obj = null;
        if (IdelObj.Count > 0)
        {
            obj = IdelObj.Dequeue();
        }
        else
        {
            obj = new WObject(path);
        }
        StartCoroutine(obj.Load());
        WWW w = obj.mLoad();
        IdelObj.Enqueue(obj);
        return w;
    }
    public string loadText(string path)
    {
        WWW w = load(path);
        if (w == null) return "";
        return w.text;
    }
    public override void OnInit() { }
    public override void Close()
    {
        IdelObj.Clear();
        base.Close();
    }
}
