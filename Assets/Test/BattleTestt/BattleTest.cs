using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleTest : MonoBehaviour
{
    public int range = 2;
    public int minrange = 2;

    public int type=1;
    private int maxtype = 2;
    List<NavCell> path = new List<NavCell>();

    HexCell start;

    private void Start()
    {   
        Manage.Instance.mAwake();
        Manage.Instance.Battle.mStart();
    }
    
    bool IsMove(HexCell cell)
    {
        if (cell.obstacle != null) return false;
        return true;
    }
    void MoveRange()
    {
        HexCell cell = BattleManager.SelectedCell();
        if (cell == null) return;
        NavCell target = path.Find(a=> a.cell == cell);
        if (target != null)
        {
            foreach (var item in path)
            {
                item.cell.State = HexCellState.none;
            }

            do
            {
                target.cell.State = HexCellState.Move;
                target = target.parent;
            } while (target != null);
        }
        else
        {
            path = HexGrid.Instantiate.GetMoveRangePath(cell, range, IsMove);
            foreach (var item in path)
            {
                item.cell.State = HexCellState.Move;
            }
        }
    }

    bool IsAtk(HexCell cell)
    {
        if (cell.obstacle != null) return false;
        return true;
    }
    void AttackRange()
    {
        HexCell cell = BattleManager.SelectedCell();
        if (cell == null) return;        
        List<NavCell> path = HexGrid.Instantiate.GetAtkRange(cell, range, minrange, IsAtk);
        foreach (var item in path)
        {
            item.cell.State = HexCellState.Attack;
        }
    }

    void MovePath()
    {
        var cell = BattleManager.SelectedCell();
        if (start == null)
            start = cell;
        else
        {
            if (cell != null)
            {
                //var target = HexGrid.Game.GetPath(start, cell, range);               
                //HexGrid.Game.ChangeCellState(target,HexCellState.Move);
            }
        }
    }
    
    private void OnGUI()
    {
            return;
        if (GUILayout.Button(type.ToString(), GUILayout.MinHeight(30), GUILayout.MinWidth(50)))
        {
            type++;
            if (type > maxtype) type = 1;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (type == 1)
            {
                MoveRange();
            }
            else
                if (type == 2)
            {
                AttackRange();
            }
            else
                if (type == 3)
            {
                MovePath();
            }
        }
    }
}
