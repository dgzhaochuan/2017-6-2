

using UnityEngine;
namespace BattleState
{
    public class BattleState : State
    {
        protected BattleManager owner { get { return Manage.Instance.Battle; } }

        protected override void AddListeners()
        {
            InputManager.OnClick += OnClick;
        }
        protected override void RemoveListeners()
        {
            InputManager.OnClick -= OnClick;
        }

        protected virtual void OnClick()
        {
           
        }

        protected HexCell ClickCell()
        {
            HexCell cell = BattleManager.SelectedCell();
            owner.SetSeleTarget(cell);
            return cell;
        }
    }
}