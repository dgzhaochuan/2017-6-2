
using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// buff展示
/// </summary>
public class BuffStatsPanel : Panel
{
    BuffModel model;
    BaseAttribute attributeModel;
    Text GetLabel
    {
        get
        {
            Text text = null;
            if (IdelLabel.Count > 0)
            {
                text = IdelLabel.Dequeue();
                text.gameObject.SetActive(true);
            }
            else
            {
                GameObject game = Instantiate<GameObject>(LabelPrefab);
                game.transform.SetParent(LabelParent);
                game.transform.localScale = Vector3.one;
            }
            briskLabel.Enqueue(text);
            return text;
        }
    }
    GameObject LabelPrefab
    {
        get
        {
            return Manage.Instance.Resources.GetObj(ResourcesEnum.UIPrefab, Config.LabelPrefab, true);
        }
    }
    Transform LabelParent;
    Text name_text;
    Queue<Text> briskLabel = new Queue<Text>();
    Queue<Text> IdelLabel = new Queue<Text>();


    public override void mAwake()
    {
        base.mAwake();
        name_text = transform.Find("name").GetComponent<Text>();
        EventTrigger.Get(gameObject).onClick = delegate (GameObject game) { Close(); };
        LabelParent = transform.Find("Grid");
    }

    public void OnUpdate(BuffModel model, BaseAttribute attributeModel)
    {
        this.attributeModel = attributeModel;
        this.model = model;
        OnUpdate();
    }
    public override void OnUpdate()
    {
        if (gameObject.activeSelf == false) return;
        while (briskLabel.Count > 0)
        {
            Text label = briskLabel.Dequeue();
            label.gameObject.SetActive(false);
            IdelLabel.Enqueue(label);
        }
        name_text.text = model.name;
        GetLabel.text = model.GetEffect(attributeModel);
       
        GetLabel.text = "";
        GetLabel.text = "持续时间:" + model.Duration + "回合";
    }

    void UpdateLabel(float value,string name)
    {
        Text label = GetLabel;
        label.color= value > 0 ? Config.UpColor : Config.DownColor;
        label.text = name + ":" + (value > 0 ? "+" : "-") + value;
    }

   
}