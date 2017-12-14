

using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace BattleState
{
    public class SelectUnitBattleState : BattleState
    {
        HexCell SeleCell;
        NavCell moveTarget;
        List<NavCell> range = new List<NavCell>();
        List<NavCell> path = new List<NavCell>();
        AutomaticSelectUnit AutoSelect = new AutomaticSelectUnit();
        public override void Enter(object[] obj)
        {
            base.Enter(obj);
            Reset();
            owner.CurrentUnit = owner.Unitmanager.NextUnit();
            if (!owner.CurrentUnit.IsPlayRank())
            {
                StartCoroutine(Auto());
            }
            else
            {
                FindMoveRnage();
            }
        }
        IEnumerator Auto()
        {
            yield return new WaitForSeconds(.5f);
            AutoSelect.Enter(owner.CurrentUnit);
        }
        public override void Exit()
        {
            base.Exit();
            Reset();
        }
        protected override void OnClick()
        {            
            HexCell cell = ClickCell();
            if (cell == null
                || (cell.State != HexCellState.Move&& cell.State != HexCellState.MovePath)
                || cell == owner.CurrentUnit.cell
                )
            {
                moveTarget = null;
                SeleCell = null;
                if (path != null)
                    HexGrid.Instantiate.ChangeCellState(path, HexCellState.Move);
                path.Clear();
                BattlePanel.Instance.downMenu.UpdateAp(owner.CurrentUnit,0);
                Extend.ClosePrompt();
                return;
            }
            if (SeleCell == null|| SeleCell != cell)
            {
                SeleCell = cell;
                Extend.UpdatePrompt("再次点击确定移动目标");
                moveTarget = range.Find(a => a.cell == cell);
                if(path!=null)
                    HexGrid.Instantiate.ChangeCellState(path, HexCellState.Move);
                path = new List<NavCell>();
                NavCell nav = moveTarget.Copy() ;
                while (nav != null)
                {
                    path.Insert(0, nav);
                    nav = nav.parent;
                }
                HexGrid.Instantiate.ChangeCellState(path, HexCellState.MovePath);
                BattlePanel.Instance.downMenu.UpdateAp(owner.CurrentUnit, moveTarget.consume);
                return;
            }
            owner.ChangeState<MoveBattleState>(moveTarget);
        }
        void FindMoveRnage()
        {
            HexCell cell = owner.CurrentUnit.cell;
            if (cell.unit.CurrentAP <= 0)
            {
                print("NoAction");
                //owner.Unitmanager.NoActive();
                return;
            }
            owner.SetSeleTarget(cell);
            CameraManager.Instance.SetTarget(cell.unit.transform);
            range = HexGrid.Instantiate.GetMoveRangePath(cell.unit.cell,
                                                  cell.unit.CurrentAP,
                                                  cell.unit.IsMove);
            HexGrid.Instantiate.ChangeCellState(range, HexCellState.Move);
        }
        void Reset()
        {
            HexGrid.Instantiate.ChangeCellState(range, HexCellState.none);
            HexGrid.Instantiate.ChangeCellState(path, HexCellState.none);
            range.Clear();
            path.Clear();
        }
    }
}