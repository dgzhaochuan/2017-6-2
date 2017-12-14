

using BattleState;
using UnityEngine;


public class EquiAtkState: AtkBattleState
{
    EquipmentAttribute model;
    public override void Enter(object[] obj)
    {
        model = Manage.Instance.Data.GetObj<EquipmentAttribute>(owner.CurrentUnit.DataModel.dataModel.weapon[0]);
        TargetCell = null;
        if (obj.Length > 0)
        {
            TargetCell = obj[0] as HexCell;
            OnAtk();
            return;
        }
        FindAtkRange(model.GetValue(DataEnum.maxatkRange), model.GetValue(DataEnum.minatkRange));
        if (atkRange.Count == 0)
        {
            UpdatePrompt("范围内没有可以攻击的目标");
        }
        else
        {
            UpdatePrompt("选择一个攻击目标");
        }
        base.Enter(obj);
    }
    protected override void OnClick()
    {
        HexCell cell = ClickCell();
        if (cell==null||cell.State != HexCellState.Attack|| cell == null)
        {
            owner.ChangeState<SelectUnitBattleState>();
            return;
        }
        if (cell.unit == null) return;
        if (cell.unit.ranks == owner.CurrentUnit.ranks) return;
        if (TargetCell == null)
        {
            TargetCell = cell;
            UpdatePrompt("再次点击确认攻击目标");
            return;
        }
        if (TargetCell != cell)
        {
            TargetCell = cell;
            return;
        }
        OnAtk();
        base.OnClick();
    }

    protected override void OnAtk()
    {
        owner.CurrentUnit.UserApp(model.UseAP);
        owner.CurrentUnit.OnAtk(model,TargetCell);
        base.OnAtk();
    }
}