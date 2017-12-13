using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FoldTools : MonoBehaviour
{
    Text openText;
    Text closeText;
    Tweener panel;
    GameObject OpenBtn;
    GameObject CloseBtn;
    private void Awake()
    {
        panel = transform.Find("panel").GetComponent<Tweener>();
        if (panel == null)
        {
            TweenerScale scale = transform.Find("panel").gameObject.AddComponent<TweenerScale>();
            scale.close = new Vector2(1,0);
            scale.open = new Vector2(1, 1);
            panel = scale;
        }
        panel.OnCloseEndEvent = CloseEnd;
        OpenBtn = transform.Find("button/open").gameObject;
        CloseBtn = transform.Find("button/close").gameObject;
        openText = OpenBtn.GetComponentInChildren<Text>();
        closeText= CloseBtn.GetComponentInChildren<Text>();
        OpenBtn.GetComponent<Button>().onClick.AddListener(open);
        CloseBtn.GetComponent<Button>().onClick.AddListener(close);
        OpenBtn.SetActive(true);
        CloseBtn.SetActive(false);
        panel.ToClose();
        //panel.OnClose();
    }

    void open()
    {
        if (panel.IsPlay) return;
        panel.gameObject.SetActive(true);
        OpenBtn.SetActive(false);
        CloseBtn.SetActive(true);
        panel.OnOpen();
    }
    void close()
    {
        if (panel.IsPlay) return;
        OpenBtn.SetActive(true);
        CloseBtn.SetActive(false);
        panel.OnClose();
    }
    
    void CloseEnd()
    {
        panel.gameObject.SetActive(false);
    }
    
    public void SetBtnText(string open,string close)
    {
        openText.text = open;
        closeText.text = close;
    }

}

