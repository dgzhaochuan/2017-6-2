using UnityEngine;
using UnityEditor;

public abstract  class PoolObj : MonoBehaviour
{
    public abstract void Init();

    protected virtual void OnDisable()
    {
        ObjectPooling.Instance.AddObject(this);
    }
}