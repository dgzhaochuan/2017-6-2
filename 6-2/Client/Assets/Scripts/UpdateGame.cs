using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpdateGame : MonoBehaviour {

    protected abstract void OnUpdate();
    private void OnEnable()
    {
        Manage.Instance.AddUpdate(OnUpdate);
    }
    private void OnDisable()
    {
        Manage.Instance.RemoveUpdate(OnUpdate);
    }
}
