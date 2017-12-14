

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace BattleState
{
    public class AtkBattleState : BattleState
    {
        protected HexCell TargetCell;
        protected List<NavCell> atkRange = new List<NavCell>();
        public override void Exit()
        {
            HexGrid.Instantiate.ChangeCellState(atkRange, HexCellState.none);
            base.Exit();
        }
        protected void FindAtkRange(int range, int minRange)
        {
            HexCell cell = owner.CurrentUnit.cell;
            atkRange = HexGrid.Instantiate.GetAtkRange(cell.unit.cell, range, minRange, IsAtk);
            HexGrid.Instantiate.ChangeCellState(atkRange, HexCellState.Attack);
        }
        protected virtual bool IsAtk(HexCell cell)
        {
            return true;
            return cell.unit != null && cell.unit.ranks != owner.CurrentUnit.ranks;
        }
        protected virtual void OnAtk() { }
        protected void UpdatePrompt(string msg)
        {
            Extend.UpdatePrompt(msg);
        }

    }
}