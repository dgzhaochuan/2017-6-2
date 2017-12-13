using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIPolygon : Image
{
    public UnityAction OnClick;
    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        return GetComponent<PolygonCollider2D>().OverlapPoint(screenPoint);
    }



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
            points[i] *= 90;
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
    }


    private void OnMouseDown()
    {
        //if (!Application.isEditor)
        //{
        //    print(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId));
        //    if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;
        //}
        //else
        //{
        //    print(EventSystem.current.IsPointerOverGameObject());
        //    if (!EventSystem.current.IsPointerOverGameObject()) return;
        //}
        if (OnClick != null) OnClick();
    }
}