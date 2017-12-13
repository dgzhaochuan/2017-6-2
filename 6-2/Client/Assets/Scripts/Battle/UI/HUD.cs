

using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private RectTransform rectTransorm;
    private Transform target;
    private Text text;
    private HudSlider slider;

    public  void Init()
    {
        rectTransorm = transform as RectTransform;
        slider = GetComponent<HudSlider>();
        slider.Init();
        text = transform.Find("Text").GetComponent<Text>();
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    void update()
    {
        if (target)
            transform.position = Camera.main.WorldToScreenPoint(target.position);
    }
    public void UpdateValue(float max, float current)
    {
        text.text = string.Format("{0}/{1}", current, max);
        slider.SetValue(current/max);
    }
    private void OnEnable()
    {
        Manage.Instance.AddUpdate(update);
    }
    protected   void OnDisable()
    {
        Manage.Instance.RemoveUpdate(update);
    }


    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close() { gameObject.SetActive(false); }
}