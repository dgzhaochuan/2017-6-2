

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class Movement : MonoBehaviour
{
    protected Unit unit;

    public abstract IEnumerator Traverse(NavCell nav);

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }
    protected virtual IEnumerator Turn(NavCell target)
    {
        unit.ModelTransform.LookAt(target.cell.transform);
        yield return null;
    }
}