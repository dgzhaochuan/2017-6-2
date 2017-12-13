

using UnityEngine;
using UnityEngine.UI;

public class SizeFitter:MonoBehaviour
{
    public float Spacing = 0;
    RectTransform _rect;
    RectTransform rect { get
        {
            if (_rect == null) _rect = transform as RectTransform;
            return _rect;
        } }
    public void UpdateChild()
    {
        Vector2 point = rect.sizeDelta;
        point.y = 0;
        foreach (RectTransform rect in transform)
        {
            point.y += rect.sizeDelta.y;
        }
        point.y += Spacing * transform.childCount;
        rect.sizeDelta = point;
    }

    private void OnDrawGizmos()
    {
        UpdateChild();
    }
}