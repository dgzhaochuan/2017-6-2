

using UnityEngine;
using UnityEngine.UI;

public class ClampPanelPoint : MonoBehaviour
{
    public int[] off = new int[] { 20, 20, 20, 20 };

    RectTransform rect;
    //左右上下
    float[] Clamp;
    Canvas canvas;
    void Awake()
    {
        rect = transform as RectTransform;
        rect.anchorMax = rect.anchorMin = Vector2.zero;
    }
    private void Start()
    {
        canvas = GetComponentInChildren<Image>().canvas;
    }
    void Update()
    {
        UpdateClamp();
        rect.anchoredPosition = MousePoint();
        Vector2 point = rect.anchoredPosition;
        point.x = Mathf.Clamp(point.x, Clamp[0], Clamp[1]);
        point.y = Mathf.Clamp(point.y, Clamp[3], Clamp[2]);
        rect.anchoredPosition = point;
    }
    void UpdateClamp()
    {
        Clamp = new float[4];
        Clamp[0] = rect.pivot.x * rect.sizeDelta.x + off[0];
        Clamp[1] = Screen.width - (1 - rect.pivot.x) * rect.sizeDelta.x - off[1];
        Clamp[2] = Screen.height - (1 - rect.pivot.y) * rect.sizeDelta.y - off[2];
        Clamp[3] = rect.pivot.y * rect.sizeDelta.y + off[3];
    }
    Vector2 MousePoint()
    {
        Vector2 pos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.GetComponent<Camera>(), out pos);
        pos.x += Screen.width * .5f;
        pos.y += Screen.height * .5f;
        return pos;
    }
}
