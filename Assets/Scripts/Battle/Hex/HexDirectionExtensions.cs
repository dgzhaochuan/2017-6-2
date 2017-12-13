using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public enum HexDirection
{
    //右上
    NE,
    //右
    E,
    //右下
    SE,
    //左下
    SW,
    //左
    W,
    //左上
    NW
}

public static class HexDirectionExtensions
{

    public static HexDirection Opposite(this HexDirection direction)
    {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }
    //前一个
    public static HexDirection Previous(this HexDirection direction)
    {
        return direction == HexDirection.NE ? HexDirection.NW : (direction - 1);
    }
    //后一个
    public static HexDirection Next(this HexDirection direction)
    {
        return direction == HexDirection.NW ? HexDirection.NE : (direction + 1);
    }
}
