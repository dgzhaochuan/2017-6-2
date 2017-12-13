using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class PromptPanel : Panel
{
    Text text;

    public override void mAwake()
    {
        text = transform.Find("PromptPanel/msg").GetComponent<Text>();
        base.mAwake();
    }

    public override void OnUpdate() { }
    public void Open(string msg)
    {
        text.text = msg;
        base.Open();
    }
}