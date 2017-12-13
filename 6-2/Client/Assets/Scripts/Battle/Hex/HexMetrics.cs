using UnityEngine;

public static class HexMetrics
{
    //外援半径
    public const float outerRadius = 1f;
    //内圆半径
     public const float innerRadius = outerRadius * 0.866025404f;
    public const float inner = 0.866025404f;
    //纯色占比
    public const float solidFactor = 1;
    //调和边界色
    public const float blendFactor = 1f - solidFactor;


    public static Vector3[] corners = {
           new Vector3(0f, 0f, outerRadius),
           new Vector3(innerRadius, 0f, 0.5f *outerRadius),
           new Vector3(innerRadius, 0f, -0.5f* outerRadius),
           new Vector3(0f, 0f, -outerRadius),
           new Vector3(-innerRadius, 0f, -0.5f* outerRadius),
           new Vector3(-innerRadius, 0f, 0.5f* outerRadius),
           new Vector3(0f, 0f, outerRadius)
     };

    public static Vector3 GetFirstCorner(HexDirection direction)
    {
        return corners[(int)direction];
    }

    public static Vector3 GetSecondCorner(HexDirection direction)
    {
        return corners[(int)direction + 1];
    }

    public static Vector3 GetFirstSolidCorner(HexDirection direction)
    {
        return corners[(int)direction] * solidFactor;
    }

    public static Vector3 GetSecondSolidCorner(HexDirection direction)
    {
        return corners[(int)direction + 1] * solidFactor;
    }
}