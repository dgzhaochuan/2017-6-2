
using UnityEngine;
namespace BattleState
{
    public class InItBattleState : BattleState
    {
        public override void Enter(object[] obj)
        {
            print("Start Scene");
            owner.Unitmanager.Init(4001);
            //Reset第一个角色，加AP，不然第一个的reset不会被调用到，加不到ap
            owner.Unitmanager.NextUnit().Reset();
            owner.ChangeState<SelectUnitBattleState>(null);
        }
    }
}