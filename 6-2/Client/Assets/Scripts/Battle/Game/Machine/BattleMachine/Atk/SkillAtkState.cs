
using BattleState;
using System.Collections.Generic;
using UnityEngine;

public class SkillAtkState : AtkBattleState
{
    SkillAttribute model;
    List<NavCell> Range = new List<NavCell>();
    public override void Enter(object[] obj)
    {
        model = obj[0] as SkillAttribute;
        FindAtkRange(model.releaseRange, 0);
    }
    public override void Exit()
    {
        if (Range.Count > 0)
        {
            HexGrid.Instantiate.ChangeCellState(Range, HexCellState.none);
        }
        Range.Clear();
        base.Exit();
    }
    protected override bool IsAtk(HexCell cell)
    {
        return base.IsAtk(cell);
    }
    protected override void OnClick()
    {
        HexCell cell = ClickCell();
        if (cell == null || cell.State != HexCellState.Attack || cell.State != HexCellState.MovePath)
        {
            owner.ChangeState<SelectUnitBattleState>();
            return;
        }
        switch (model.releaseType)
        {
            case 0:

                break;
            case 1:
                if (cell.unit == null) return;
                if (cell.unit.ranks == owner.CurrentUnit.ranks) return;
                break;
            case 2:
                if (cell.unit == null) return;
                if (cell.unit.ranks != owner.CurrentUnit.ranks) return;
                break;
            case 3:
                OnAtk();
                base.OnClick();
                return;
        }
        switch (model.targetType)
        {
            case targetTypeEnum.enemy:
                if (cell.unit.ranks == owner.CurrentUnit.ranks) return;
                break;
            case targetTypeEnum.allenemy:
                break;
            case targetTypeEnum.teammate:
                if (cell.unit.ranks != owner.CurrentUnit.ranks) return;
                break;
            case targetTypeEnum.allteammate:
                break;
        }
        if (TargetCell == null)
        {
            ChangeTargetCell(cell);
            //UpdatePrompt("");
            return;
        }
        if (TargetCell != cell)
        {
            ChangeTargetCell(cell);
            return;
        }
        OnAtk();
        base.OnClick();
    }
    protected override void OnAtk()
    {
        owner.CurrentUnit.UserApp(model.GetAP(owner.CurrentUnit.DataModel.finalAttribute));
        GameObject prefab = Manage.Instance.Resources.GetObj(ResourcesEnum.SkillPrefab, model.model);
        ObjectPooling.Instance.GetObject<BaseSkillObject>(prefab,null).Enter(owner.CurrentUnit,model, TargetCell, ChangeState);
    }
    void ChangeState()
    {
        owner.ChangeState<SelectUnitBattleState>();
    }
    void ChangeTargetCell(HexCell cell)
    {
        TargetCell = cell;
        if (Range.Count > 0)
        {
            HexGrid.Instantiate.ChangeCellState(Range, HexCellState.none);
        }
        Range.Clear();
        Range= HexGrid.Instantiate.GetAtkRange(TargetCell, model.targetRange, 0, IsAtk);
        HexGrid.Instantiate.ChangeCellState(Range,HexCellState.MovePath);
    }
}