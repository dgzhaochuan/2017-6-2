

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BattleState
{
    public class MoveBattleState : BattleState
    {
        List<NavCell> atkRange = new List<NavCell>();
        NavCell MoveTargetCell;
        HexCell AtkTarget;
        SkillAttribute seleSkill;
        public override void Enter(object[] obj)
        {
            base.Enter(obj);
            MoveTargetCell = obj[0] as NavCell;
            if (obj.Length > 1)
            {
                AtkTarget = obj[1] as HexCell;
                seleSkill = obj[2] as SkillAttribute;
            }
            if (MoveTargetCell.cell == owner.CurrentUnit.cell)
            {
                MoveEnd();
            }
            else
            {
                BattleManager.ChangeState(GameState.moveing);
                StartCoroutine(Swquence());
            }
        }
        public override void Exit()
        {
            base.Exit();
            Reset();
        }      
        void Reset()
        {
            HexGrid.Instantiate.ChangeCellState(atkRange, HexCellState.none);
            atkRange.Clear();
        }
        void MoveEnd()
        {
            owner.CurrentUnit.UserApp(MoveTargetCell.consume);
            BattleManager.ChangeState(GameState.play);
            MoveTargetCell.cell.SetUnit(owner.CurrentUnit);
            if (owner.CurrentUnit.IsPlayRank())
            {
                owner.ChangeState<SelectUnitBattleState>(null);
            }
            else
            {
                if (seleSkill == null)
                {
                    if (AtkTarget != null)
                    {
                        owner.ChangeState<EquiAtkState>(AtkTarget);
                    }
                    else
                    {
                        owner.Unitmanager.NoActive();
                        owner.ChangeState<SelectUnitBattleState>();
                    }
                } else
                {
                    owner.ChangeState<SkillAtkState>(seleSkill,AtkTarget);
                }
            }
        }
        IEnumerator Swquence()
        {
            if (MoveTargetCell != null)
            {
                CameraManager.Instance.SetTarget(owner.CurrentUnit.transform);
                Movement m = owner.CurrentUnit.GetComponent<Movement>();
                yield return StartCoroutine(m.Traverse(MoveTargetCell));
            }
            CameraManager.Instance.SetTarget( null);
            MoveEnd();
        }
    }
}