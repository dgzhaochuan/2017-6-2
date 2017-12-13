

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WalkMovement : Movement
{
   
    public override IEnumerator Traverse(NavCell nav)
    {
        List<NavCell> path = new List<global::NavCell>();
        while (nav != null)
        {
            path.Insert(0, nav);
            nav = nav.parent;
        }
        unit.animation.SetBool("Run",true);
        for (int i = 1; i < path.Count; ++i)
        {
            NavCell target = path[i];
            yield return StartCoroutine(Turn(target));
            yield return StartCoroutine(Walk(target));
            if (target.cell.UnitEnter(unit))
            {
                unit.NoActive();
                target.cell.SetUnit(unit);
                Manage.Instance.Battle.ClearCurrent();
                Manage.Instance.Battle.ChangeState<BattleState.SelectUnitBattleState>(null);
                break;
            }
        }
        unit.animation.SetBool("Run", false);
        yield return null;
    }

    IEnumerator Walk(NavCell target)
    {
        unit.tweener.open = target.cell.transform.position;
        unit.tweener.close = unit.transform.position;
        unit.tweener.OnOpen();
        while (unit.tweener.IsPlay)
            yield return null;
    }
}
