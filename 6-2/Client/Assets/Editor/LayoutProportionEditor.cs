using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[CustomEditor(typeof(LayoutProportion))]
public class LayoutProportionEditor : Editor
{
    Dictionary<LayoutElement, float> f = new Dictionary<LayoutElement, float>();
    LayoutProportion la;
    public override void OnInspectorGUI()
    {
        la = target as LayoutProportion;
        List<LayoutElement> LayoutElement = new List<UnityEngine.UI.LayoutElement>();
        foreach (Transform item in la.transform)
        {
            LayoutElement lay = item.GetComponent<LayoutElement>();
            if (lay)
            {
                LayoutElement.Add(lay);
                if (f.ContainsKey(lay) == false)
                {
                    float value = 0;
                    value = la.GetComponent<RectTransform>().sizeDelta.x / lay.minWidth;
                    f.Add(lay, value);
                }
            }
        }
        UpdateProportion(LayoutElement);
        base.OnInspectorGUI();
    }

    void UpdateProportion(List<LayoutElement> Layout)
    {
        for (int i = 0; i < Layout.Count; i++)
        {
            if (f.ContainsKey(Layout[i]) == false) f.Remove(Layout[i]);
            f[Layout[i]] = EditorGUILayout.FloatField(Layout[i].name+":", f[Layout[i]]);
            Layout[i].minWidth = la.GetComponent<RectTransform>().sizeDelta.x * f[Layout[i]];
        }
    }
}
