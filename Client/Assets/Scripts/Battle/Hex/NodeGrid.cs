using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NodeGrid:MonoBehaviour
{
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
    public enum NodeType
    {
        Node_Square,
        Node_Hex,
    }

    public bool ShowGrid;

    public NodeType node_type;
    public Vector2 size=Vector2.one;
    public Vector2 off = Vector2.zero;
    public float maxHight=.5f;
    public int width=10;
    public int length=10;

    Transform TileParent;
    float outerRadius { get { return (size.x ) * .5f; } }
    float innerRadius { get { return outerRadius * 0.866025404f; } }


    public Node[] node_array;
    
    [ContextMenu("aaaa")]
    public void StartMap( )
    {
        TileParent = transform.Find("Tile");
        while (TileParent != null)
        {
            DestroyImmediate(TileParent.gameObject);
            TileParent = transform.Find("Tile");
        }

        TileParent = new GameObject("Tile").transform;
        TileParent.parent = transform;
        TileParent.localPosition = Vector3.zero;
        // new Vector3((size.x + off.x) * width, 0, (size.y + off.y) *length) * .5f;


        node_array = new Node[width * length];
        switch (node_type)
        {
            case NodeType.Node_Square:
                Start_SquareNode();
                break;
            case NodeType.Node_Hex:
                Start_HexNode();
                break;
        }
    }
    void Start_SquareNode( )
    {
        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 point = new Vector3((size.x + off.x) * x, 0,(size.y + off.y) * z) -new Vector3(size.x, 0, -size.y) * .5f;
                //var node = new SquareNode(x, z, point);
                var node = GetNode(x,z,point);
                node_array[z*length+x]=node;
                if (z > 0)
                {
                    node.SetNeithbor(2, node_array[(z - 1)* length]);
                }
                if (x > 0)
                {
                    node.SetNeithbor(3, node_array[x-1]);
                }               
            }
        }
    }
    void Start_HexNode()
    {
        Vector2 _size = size;
        for (int z = 0, i = 0; z < length; z++,i++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 position = Vector3.zero;
                position.x = x;
                position.z = z;

                position.x = (x + z * 0.5f - z / 2) * (innerRadius * 2f) + off.x * x;
                position.z = z * (_size.y / 2 * 1.5f) + off.y * z;
                //var node = new HexNode(x, z, position);
                var node = GetNode(x, z, position);
                node_array[i]=node;

                //设置左边个邻居，因为是双向设置，所以第一个不能设置，第一个没有左边个
                if (x > 0)
                {
                    node.neithbor[(int)HexDirection.W] = node_array[i - 1];
                }
                if (z > 0)
                {
                    //设置偶数行下面邻居，
                    if ((z & 1) == 0)
                    {
                        node.neithbor[(int)HexDirection.SE] = node_array[i - width];
                        //不是第一个才会有左下的邻居
                        if (x > 0)
                        {
                            node.neithbor[(int)HexDirection.SW] = node_array[i - width - 1];
                        }
                    }
                    //设置奇数行下面邻居，
                    else
                    {
                        node.SetNeithbor((int)HexDirection.SW, node_array[i - width]);
                        //不是最后一个才会有右下邻居
                        if (x < width - 1)
                        {
                            node.SetNeithbor((int)HexDirection.SE, node_array[i - width + 1]);
                        }
                    }
                }

            }
        }
    }

    Node GetNode(int x,int z,Vector3 point)
    {
        Node node = null;
        GameObject game = new GameObject(string.Format("{0}_{1}", x, z));  
        switch (node_type)
        {
            case NodeType.Node_Square:
             node=   game.AddComponent<SquareNode>();
                break;
            case NodeType.Node_Hex:
                node = game.AddComponent<HexNode>();
                break;
        }
        node.OnInit(x, z, point);
        game.transform.SetParent(TileParent,false);
        game.transform.localPosition = node.point;
        return node;
    }

    Vector3[] GetVertex(Node node)
    {
        Vector3 parent = transform.position;
        var vertex = new Vector3[0];
        switch (node_type)
        {
            case NodeType.Node_Square:
                vertex= Get_SquareNodeVertex(node.point, parent);
                break;
            case NodeType.Node_Hex:
                vertex= Get_HexNodeVertex(node.point, parent);
                break;
        }
        return vertex;
    }
    Vector3 PointOff()
    {
        return Vector3.zero;
        return new Vector3(size.x, 0, -size.y) * .5f;
    }
    Vector3[] Get_SquareNodeVertex(Vector3 point, Vector3 parent )
    {
        Vector3[] points = {
                                         point +parent+PointOff(),
                                         point + new Vector3(size.x , 0, 0) +parent+PointOff(),
                                         point + new Vector3(size.x , 0, size.y ) +parent+PointOff(),
                                         point + new Vector3(0, 0, size.y ) +parent+PointOff(),
                                         };
        return points;
    }
    Vector3[] Get_HexNodeVertex(Vector3 point,Vector3 parent )
    {
        Vector3[] points = {
                                           new Vector3(0f, 0f, outerRadius)+parent+point,
                                           new Vector3(innerRadius, 0f, 0.5f *outerRadius)+parent+point,
                                           new Vector3(innerRadius, 0f, -0.5f* outerRadius)+parent+point,
                                           new Vector3(0f, 0f, -outerRadius)+parent+point,
                                           new Vector3(-innerRadius, 0f, -0.5f* outerRadius)+parent+point,
                                           new Vector3(-innerRadius, 0f, 0.5f* outerRadius)+parent+point,
                                            };
        return points;
    }

    bool  UpdateVertexPoint(Vector3[] vertex)
    {
        List<Transform> point = new List<Transform>();
        for (int i = 0; i < vertex.Length; i++)
        {
            Ray ray = new Ray(vertex[i], Vector3.down);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit)) continue;
            point.Add(hit.collider.transform);
        }
        if (point.Count <=3) return false;
        float hight = point[1].position.y-point[0].position.y;
        float value = 0;
        for (int i = 1; i < point.Count-1; i++)
        {
            value = point[i - 1].position.y - point[i].position.y;
            if (value > hight)
                hight = value;
        }
        return value > maxHight;
    }

    void OnDrawGizmos()
    {
        if (!ShowGrid) return;
        //StartMap();
        Gizmos.color = Color.red;
        foreach (var item in node_array)
        {
            var points = GetVertex(item);
            Gizmos.DrawLine(points[points.Length - 1], points[0]);
            for (int i = 0; i < points.Length - 1; i++)
            {
                Gizmos.DrawLine(points[i], points[i+1]);
            }
        }
    }    
}


[System.Serializable]
public   class Node:MonoBehaviour
{
    public int x;
    public int y;
    public Vector3 point;
    public Node[] neithbor;
    
    public virtual void OnInit(int x, int y, Vector3 point)
    {
        this.x = x;
        this.y = y;
        this.point = point;
    }

    public virtual void SetNeithbor(int index, Node node) { }
}
[System.Serializable]
public class SquareNode : Node
{
    public override void OnInit(int x, int y, Vector3 point) 
    {
        base.OnInit(x,y,point);
        neithbor = new Node[4];
    }

    public override void SetNeithbor(int index, Node node)
    {
        neithbor[index] = node;
        node.neithbor[index -2] = this;
    }

}
[System.Serializable]
public class HexNode : Node
{
    public override void OnInit(int x, int y, Vector3 point)
    {
        base.OnInit(x, y, point);
        neithbor = new Node[6];
    }
    public override void SetNeithbor(int index, Node node)
    {
        neithbor[index] = node;
        node.neithbor[Opposite(index)] = this;
    }
    int Opposite(int direction)
    {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }
}


/*

void CreateCell(int x, int z, int i)
{
    Vector3 position;
    position.x = x;
    position.y = 0f;
    position.z = z;

    position.x = (x + z * 0.5f - z / 2) * (cellBounds.x);
    position.z = z * (cellBounds.z / 2 * 1.5f);


    HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
    cell.name = i.ToString();
    cell.Id = i;
    cell.tag = Config.Hex;
    cell.gameObject.layer = Config.HexLayer;
    cell.transform.SetParent(hexMesh.transform, false);
    cell.transform.localPosition = position;

    //设置左边个邻居，因为是双向设置，所以第一个不能设置，第一个没有左边个
    if (x > 0)
    {
        cell.SetNeighbor(HexDirection.W, cells[i - 1]);
    }
    if (z > 0)
    {
        //设置偶数行下面邻居，
        if ((z & 1) == 0)
        {
            cell.SetNeighbor(HexDirection.SE, cells[i - width]);
            //不是第一个才会有左下的邻居
            if (x > 0)
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
            }
        }
        //设置奇数行下面邻居，
        else
        {
            cell.SetNeighbor(HexDirection.SW, cells[i - width]);
            //不是最后一个才会有右下邻居
            if (x < width - 1)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
            }
        }
    }

    if (gridCanvas)
    {
        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition =
             new Vector2(position.x, position.z);
        label.text = i.ToString();
        label.gameObject.layer = Config.TestLayer;
    }
}

*/
