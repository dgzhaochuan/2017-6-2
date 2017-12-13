using UI;
using UnityEngine;
using UnityEngine.UI;

public class ErrorPanel : Panel
{
    protected override bool ClickClose
    {
        get
        {
            return true;
        }
    }

    string error;
    Text errorText;
    GameObject button;
    float delay;
    float startTime;

    public override void mAwake()
    {
        base.mAwake();
        errorText = transform.Find("message").GetComponent<Text>();
        button = transform.Find("Button").gameObject;
        button.GetComponent<Button>().onClick.AddListener(delegate() { Close(); });
        EventTrigger.Get(gameObject).onDown = (GameObject g) => { Close(); };
    }

    public void Open(string error,float delay=3)
    {
        this.delay = delay;
        this.error = error;
        base.Open();
        startTime = Time.time;
    }

    private void Update()
    {
        if (delay == -1) return;
        if (startTime + delay > Time.time) return;
        delay = -1;
        base.Close();
    }

    public override void OnUpdate()
    {
        errorText.text = error;
        Debug.LogError(error);
    }
}

