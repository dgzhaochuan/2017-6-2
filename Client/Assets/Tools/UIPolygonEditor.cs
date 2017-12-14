using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPolygonEditor : MonoBehaviour {

    [ContextMenu("GetCollider")]
    public void GetCollider()
    {
        Sprite sprite = GetComponent<Image>().sprite;

        GameObject game = new GameObject();
        game.transform.SetParent(transform, false);
        game.AddComponent<SpriteRenderer>().sprite = sprite;
        var collider = game.AddComponent<PolygonCollider2D>();
        var points = collider.points;
        for (int i = 0; i < points.Length; i++)
        {
            points[i] *= 100;
        }
        var _collider = GetComponent<PolygonCollider2D>();
        if (_collider == null) _collider = gameObject.AddComponent<PolygonCollider2D>();
        _collider.points = points;
        try
        {

            DestroyImmediate(game);
        }
        catch
        {
            Destroy(game);
            //DestroyImmediate(game);
        }
        DestroyImmediate(GetComponent<Image>());
        gameObject.AddComponent<UIPolygon>().sprite = sprite;
        DestroyImmediate(GetComponent<UIPolygonEditor>());
    }

}
