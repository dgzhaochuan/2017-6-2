
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(UIPolygon))]
public class UIPolygonEditor : Editor {

    UIPolygon Image;

    public void OnEnable()
    {
        Image = target as UIPolygon;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //if (GUILayout.Button("SetCollider", GUILayout.MinHeight(30))) { GetCollider(); }
    }
    
     void GetCollider()
    {
        Sprite sprite = Image.GetComponent<Image>().sprite;

        GameObject game = new GameObject();
        game.transform.SetParent(Image.transform, false);
        game.AddComponent<SpriteRenderer>().sprite = sprite;
        var collider = game.AddComponent<PolygonCollider2D>();
        var points = collider.points;
        for (int i = 0; i < points.Length; i++)
        {
            points[i] *= 100;
        }
        var _collider = Image.GetComponent<PolygonCollider2D>();
        if (_collider == null) _collider = Image.gameObject.AddComponent<PolygonCollider2D>();
        _collider.points = points;
        try
        {
            DestroyImmediate(game);
        }
        catch
        {
            Destroy(game);
        }
        DestroyImmediate(Image.GetComponent<Image>());
        Image.gameObject.AddComponent<UIPolygon>().sprite = sprite;
    }

}
