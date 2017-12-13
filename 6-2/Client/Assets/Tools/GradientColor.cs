using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//渐变
public class GradientColor : MaskableGraphic
{

    public enum Corner
    {
        Left,Right,Up,Down
    }


    float leng = 1;
    public Corner StartCorner;
    //左 下  右 上
    Vector4 outer;
    Vector2[] GetOff(int index,float value)
    {
        Vector2[] cube = new Vector2[4];
        switch (StartCorner)
        {
            case Corner.Left:
                cube[0]= new Vector2(outer.x,outer.y)+Vector2.right* +rectTransform.rect.width * value*index;
                cube[1]= new Vector2(outer.x , outer.w) + Vector2.right * +rectTransform.rect.width * value * index;
                cube[2] = cube[1] + Vector2.right * +rectTransform.rect.width * value;
                cube[3] = cube[0] + Vector2.right * +rectTransform.rect.width * value;
                break;
            case Corner.Right:
                cube[0] = new Vector2(outer.z, outer.y) + Vector2.left * +rectTransform.rect.width * value * index;
                cube[1] = new Vector2(outer.z, outer.w) +Vector2.left * +rectTransform.rect.width * value * index;
                cube[2] = cube[1] + Vector2.right * -rectTransform.rect.width * value;
                cube[3] = cube[0]  + Vector2.right * -rectTransform.rect.width * value;
                break;
            case Corner.Up:
                cube[0] = new Vector2(outer.x, outer.w) + Vector2.up * +rectTransform.rect.width * value * index;
                cube[1] = new Vector2(outer.z, outer.w) + Vector2.up * +rectTransform.rect.width * value * index;
                cube[2] = cube[1] + Vector2.down * +rectTransform.rect.height * value;
                cube[3] = cube[0]  + Vector2.down * +rectTransform.rect.height * value;
                break;
            case Corner.Down:
                cube[0] = new Vector2(outer.x, outer.y) + Vector2.down * +rectTransform.rect.width * value * index;
                cube[1] = new Vector2(outer.z, outer.y) + Vector2.down * +rectTransform.rect.width * value * index;
                cube[2] = cube[1] + Vector2.up * +rectTransform.rect.height * value;
                cube[3] = cube[0]  + Vector2.up * +rectTransform.rect.height * value;
                break;
        }
        return cube;
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        outer = new Vector4(-rectTransform.pivot.x * rectTransform.rect.width,
                -rectTransform.pivot.y * rectTransform.rect.height,
               (1 - rectTransform.pivot.x) * rectTransform.rect.width,
               (1 - rectTransform.pivot.y) * rectTransform.rect.height);
        
        vh.Clear();
        List<UIVertex> _vh = new List<UIVertex>();
        var vert = UIVertex.simpleVert;
        float color_a = 1 / leng;
        Color _color = color;    
        //透明度-会让方向相反
        for (int i = 0; i < leng; i++)
        {           
            var cube = GetOff(i, color_a);
            _color.a = (leng-i )* color_a;

            _color.a=color.a-1;
            vert.position = cube[0];
            vert.color = _color;
            _vh.Add(vert);

            _color.a = color.a - 1;
            vert.position = cube[1];
            vert.color = _color;
            _vh.Add(vert);

            _color.a = color.a - 0;
            vert.position = cube[2];
            vert.color = _color;
            _vh.Add(vert);

            _color.a = color.a - 0;
            vert.position = cube[3];
            vert.color = _color;
            _vh.Add(vert);

            vh.AddUIVertexQuad(_vh.ToArray());
            _vh = new List<UIVertex>();
        }
    }
}
