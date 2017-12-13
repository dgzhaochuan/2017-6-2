

using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleState
{
    public class AutomaticSelectUnit
    {
        Unit unit;
        int move_minRange = 0;
        int move_maxRange = 0;
        int atk_minRange = 0;
        int atk_maxRange = 0;
        BattleManager owner { get { return Manage.Instance.Battle; } }
        public AutomaticSelectUnit() { }
        public void Enter(Unit unit)
        {
            this.unit = unit;
            SkillAttribute _skill=  SeleAction();
            if (move_maxRange <= 0)
            {
                owner.Unitmanager.NoActive();
                owner.ChangeState<SelectUnitBattleState>();
                return;
            }
            var nav = HexGrid.Instantiate.GetRange(unit.cell, move_maxRange, move_minRange, this.unit.IsAtk);
            List<Unit> units = new List<Unit>();
            for (int i = 0; i < nav.Count; ++i)
            {
                if (nav[i].unit != null)
                {
                    if (nav[i].unit.IsDie) continue;
                    if (nav[i].unit.ranks == unit.ranks) continue;
                    units.Add(nav[i].unit);
                }
            }
            HexCell Atk_Target = null;
            NavCell target = null;
            List<NavCell> path = new List<NavCell>();
            if (units.Count() == 0)
            {
                path = GetPath();
            }
            else
            {
                Atk_Target = Manage.Instance.Battle.Unitmanager.GetMaxValue(unit.ranks,units).cell;
                List<NavCell> atkCell = HexGrid.Instantiate.GetAtkCell(Atk_Target, atk_maxRange, atk_minRange, unit.AtkRange);
                List<NavCell> moveCell = HexGrid.Instantiate.GetMoveRangePath(this.unit.cell,
                                                                              //this.unit.DataModel[DataEnum.move],
                                                                              move_maxRange,
                                                                              this.unit.IsMove);
               var _range = moveCell.Intersect(atkCell, new NavCell.StudentComparer()).ToList();
                //var _cell = atkCell.Find(a => a.cell == owner.CurrentUnit.cell);
                //if (_cell != null)
                //{
                //    _range.Add(_cell);
                //}
                if (_range.Count == 0)
                {
                    path = GetPath();
                    Atk_Target = null;
                }
                else
                {
                    _range.Sort((a, b) => b.GetValue(unit).CompareTo(a.GetValue(unit)));
                    target = _range[0];
                    if (target.cell != unit.cell)
                        path = HexGrid.Instantiate.GetPath(this.unit.cell, target.cell,
                                                                                     this.unit.DataModel[DataEnum.move],
                                                                                     this.unit.IsMove);
                }
            }
            if (path.Count > 0)
            {
                if (path[0].cell.unit != null)
                {
                    path.RemoveAt(0);
                }
            }
            if (path.Count > 0)
                target = path[0];
            if (target == null)
            {
                owner.Unitmanager.NoActive();
                owner.ChangeState<SelectUnitBattleState>();
                return;
            }
            owner.ChangeState<MoveBattleState>(target, Atk_Target,_skill);
        }
        List<NavCell> GetPath()
        {
            List<NavCell> path = new List<NavCell>();
            Unit _unit = owner.Unitmanager.GetMaxValue(unit.ranks);
            path = HexGrid.Instantiate.GetPath(this.unit.cell, _unit.cell, this.unit.DataModel[DataEnum.move], this.unit.IsMove);
            return path;
        }

        SkillAttribute SeleAction()
        {
            //List<SkillAttribute> AllSkill = new List<SkillAttribute>();
            //var _arraySkill = unit.DataModel.dataModel.skillArray;
            //for (int i = 0; i < _arraySkill.Length; i++)
            //{
            //    for (int j = 0; j < _arraySkill[i].skillAry.Count; j++)
            //    {
            //        AllSkill.Add(Manage.Instance.Data.GetObj<SkillAttribute>(_arraySkill[i].skillAry[j]));
            //    }
            //}
            //Unit targetUnit = owner.Unitmanager.GetMaxValue(owner.CurrentUnit);

            //暂时不用技能  直接攻击
            EquipmentAttribute eq = Manage.Instance.Data.GetObj<EquipmentAttribute>(owner.CurrentUnit.DataModel.dataModel.weapon[0]);
            move_minRange = 0;
            move_maxRange = unit.CurrentAP- eq.UseAP;
            atk_minRange = eq.GetValue(DataEnum.minatkRange);
            atk_maxRange = eq.GetValue(DataEnum.maxatkRange);
            return null;
        }
    }
}