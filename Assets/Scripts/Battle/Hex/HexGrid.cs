using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class HexGrid : MonoBehaviour
{
    static private HexGrid grid;
    static public new HexGrid Instantiate
    {
        get
        {
            if (grid == null)
                grid = GameObject.FindObjectOfType<HexGrid>();
            return grid;
        }
    }
    static public float CellDirection;



    public int width = 6;
    public int height = 6;
    public HexCell cellPrefab;
    public Text cellLabelPrefab;
    public Vector3 cellSize=Vector3.one;


    HexCell[] cells;
    HexMesh hexMesh;
    Canvas gridCanvas;
    Vector3 cellBounds;

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        //position.x = x * 10f;
        //position.y = 0f;
        //position.z = z * 10f;
        position.x = x;
        position.y = 0f;
        position.z = z;

        position.x = (x + z * 0.5f - z / 2) * (cellBounds.x* cellSize.x);
        position.z = z * (cellBounds.z* cellSize.z / 2 * 1.5f);


        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.name = i.ToString();
        cell.Id = i;
        cell.tag = Config.Hex;
        cell.gameObject.layer = Config.HexLayer;
        cell.transform.SetParent(hexMesh.transform, false);
        cell.transform.localPosition = position;
        cell.transform.localScale = cellSize;

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

    void SetObstacle(HexCell cell)
    {
        cell.State = HexCellState.none;
        //, ~(1 << Config.HexLayer)
        var hit = Physics.SphereCastAll(cell.transform.position, cellBounds.x * .49f, Vector3.up);
        if (hit.Length == 0)
        {
            cell.State = HexCellState.Null;
            return;
        }
        foreach (var item in hit)
        {
            if (item.collider.gameObject.tag == Config.Obstacle)
            {
                cell.State = HexCellState.Obstacle;
                cell.obstacle = item.collider.gameObject;
                return;
            }
            //SceneObj obj = item.collider.GetComponent<SceneObj>();
            //if (obj)
            //{
            //    cell.obstacle = item.collider.gameObject;
            //    return;
            //}
        }

        #region
        //List<SceneObj> objs = new List<SceneObj>();
        //foreach (var item in hit)
        //{
        //    if (item.collider.gameObject.tag == Config.Obstacle)
        //    {
        //        cell.NotMove = true;
        //        return;
        //    }
        //    SceneObj obj = item.collider.GetComponent<SceneObj>();
        //    if (obj)
        //    {
        //        objs.Add(obj);
        //    }
        //}
        //if (objs.Count == 0) return;
        //SceneObj game = objs[0];
        //if (objs.Count > 1)
        //{
        //    SceneObj obj = null;
        //    float direction1 = Vector3.Distance(cell.transform.position, game.transform.position);
        //    float direction2 = Vector3.Distance(cell.transform.position, game.transform.position);
        //    for (int i = 1; i < objs.Count; i++)
        //    {
        //        obj = objs[i];
        //        direction2 = Vector3.Distance(cell.transform.position, obj.transform.position);
        //        if (direction2 < direction1)
        //            game = obj;
        //    }
        //}
        //objs.Remove(game);
        //game.cell = cell;
        //cell.NotMove = true;
        //foreach (var item in objs)
        //{
        //    DestroyObject(item.gameObject);
        //}
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>返回移动范围（range里面可能加了攻击范围）
    /// <param name="startCell">起点</param>
    /// <param name="range">移动范围+攻击范围</param>
    /// <param name="func">判断是否加入移动列表，海陆空移动范围不一样，非机械单位可以移动到载具里面</param>
    /// <returns></returns>
    public List<NavCell> GetMoveRangePath(HexCell startCell,int range,Func<HexCell,bool> func)
    {
        List<NavCell> path = new List<NavCell>();//返回的范围
        NavCell start = new NavCell(startCell);//起点
        start.consume = 0;//起点的移动消耗
        path.Add(start);//把起点添加到查找列表
        Queue<NavCell> openNav = new Queue<global::NavCell>();//已经找到的范围
        openNav.Enqueue(start);
        NavCell navCell = null;        
        while (openNav.Count > 0)
        {
            navCell = openNav.Dequeue();
            for (int i = 0; i < 6; i++)
            {
                HexCell cell = navCell.cell.GetNeighbor(i);//返回一个附近的格子
                if (cell == null) continue;               
                if (!func(cell))continue;//判断是否满足移动条件
                NavCell nav = path.Find(a => a.cell == cell);//已经找到的范围判断当前格子走过去是否消耗更小
                if (nav != null)
                {
                    if (navCell.consume + cell.Consume() >= nav.consume)
                        continue;
                    nav.ReviseParent(navCell);
                }
                else
                {                   
                    nav = new NavCell(cell, navCell);//新找到的范围，构造函数里面有移动消耗叠加的操作
                }
                if (nav.consume <= range)//如果距离没有超过移动距离 
                {
                    path.Add(nav);
                    if (nav.consume < range)
                        openNav.Enqueue(nav);
                }
            }
        }
        return path;
    }
    /// <summary>
    /// 
    /// </summary>返回攻击范围
    /// <param name="start">起点</param>
    /// <param name="range">最大攻击范围</param>
    /// <param name="minRange">最小攻击范围</param>
    /// <returns></returns>
    public List<NavCell> GetAtkRange(HexCell start, int range, int minRange, Func<HexCell, bool> func)
    {
        List<NavCell> nav = GetMoveRangePath(start, range, func);
        List<NavCell> cell = nav.Where(a => a.consume >= minRange).ToList();
        return cell;
    }
    /// <summary>
    /// 
    /// </summary>返回可以攻击到这个格子的范围，不是攻击范围
    /// <param name="start">被攻击的格子</param>
    /// <param name="range">最大攻击范围</param>
    /// <param name="minRange">最小攻击范围</param>
    /// <returns></returns>
    public List<NavCell> GetAtkCell(HexCell start,int range,int minRange, Func<HexCell, bool> func)
    {
        List<NavCell> nav = GetMoveRangePath(start, range, func);
        List<NavCell> cell = nav.Where(a => (range-a.consume) >= minRange-1).ToList();
        return cell;
    }
    /// <summary>
    /// 
    /// </summary>找到一条到目标的路径
    /// <param name="start">起点</param>
    /// <param name="end">目标点</param>
    /// <param name="move">移动力，搜索深度达到一定值就不在搜索</param>
    /// <returns></returns>
    public List<NavCell> GetPath(HexCell start, HexCell end, int move, Func<HexCell, bool> func)
    {
        int depth = (int)(move * Config.pathDepth);
        Queue<NavCell> open = new Queue<NavCell>();
        List<HexCell> close = new List<HexCell>();
        List<NavCell> path = new List<NavCell>();
        NavCell startNav = new NavCell(start);
        startNav.consume = 0;
        open.Enqueue(startNav);
        NavCell nav = null;
        HexCell cell = null;
        NavCell EndNav = null;
        while (open.Count > 0)
        {
            if (EndNav != null) break;
            nav = open.Dequeue();
            if (close.Contains(nav.cell))
                continue;
            if (nav.consume > depth)
                continue;
            for (int i = 0; i < 6; i++)
            {
                cell = nav.cell.GetNeighbor(i);
                if (cell == null) continue;
                if (cell == end)
                {
                    EndNav = new NavCell(cell, nav);
                    break;
                }
                if (!func(cell)) continue;
                NavCell _nav = null;
                if (open.Count > 0)
                {
                    _nav = open.FirstOrDefault(a => a.cell == cell);
                }
                if (_nav != null)
                {
                    if (_nav.consume > cell.Consume() + nav.consume)
                    {
                        _nav.ReviseParent(nav);
                    }
                }
                else
                {
                    _nav = new NavCell(cell, nav);
                    open.Enqueue(_nav);
                }
            }
            close.Add(nav.cell);
        }
        if (EndNav != null)
        {
            while (EndNav.consume > move)
            {
                EndNav = EndNav.parent;
            }
            while (EndNav != null)
            {
                path.Add(EndNav);
                EndNav = EndNav.parent;
            }
            return path;
        }
        return path;
    }
    public List<HexCell> GetRange(HexCell start, int range, int minRange, Func<HexCell, bool> movefunc)
    {
        List<HexCell> path = new List<HexCell>();
        Queue<HexCell> close = new Queue<global::HexCell>();
        path.Add(start);
        Queue<NavCell> openNav = new Queue<global::NavCell>();
        NavCell startNav=new NavCell(start);
        startNav.consume = 0;
        openNav.Enqueue(startNav);
        NavCell navCell = null;
        while (openNav.Count > 0)
        {
            navCell = openNav.Dequeue();
            for (int i = 0; i < 6; i++)
            {
                HexCell cell = navCell.cell.GetNeighbor(i);              
                if (cell == null) continue;
                if (close.Contains(cell)) continue;
                if (!movefunc(cell)) continue;
                if (path.Contains(cell)) continue;
                NavCell nav = new NavCell(cell, navCell);
                if (nav.consume <= range && nav.consume >= minRange)
                {
                    path.Add(cell);
                    close.Enqueue(cell);
                    openNav.Enqueue(nav);
                }
            }
        }
        return path;
    }
    public HexCell GetCell(int index) { return cells[index]; }
    public void ChangeCellState(List<NavCell> moveRange, HexCellState state)
    {
        for (int i = 0; i < moveRange.Count; i++)
        {
            moveRange[i].cell.State = state;
        }
    }

    bool CellIsMove(HexCell cell) { return cell.State == HexCellState.none; }

    #region Text

    public void mAwake()
    {
        destory();
        cellBounds = cellPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size;
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
        cells = new HexCell[height * width];
        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
        CellDirection = Vector3.Distance(cells[0].transform.position, cells[1].transform.position);
        UpdateObstacle();
    }
    public void destory()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
        if (gridCanvas) DestoryParent(gridCanvas.transform);
        if (hexMesh) DestoryParent(hexMesh.transform);
    }
    void DestoryParent(Transform parent)
    {
        while (parent.childCount > 0)
        {
            foreach (Transform item in parent)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
    public void UpdateObstacle()
    {
        foreach (var item in cells)
        {
            SetObstacle(item);
        }
    }
    public void ClearState()
    {
        foreach (var item in cells)
        {
            item.State = HexCellState.none;
        }
    }



    #endregion
}
