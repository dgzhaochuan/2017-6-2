

using System;
using UnityEngine;

public class UpdateManager : BaseManaget
{
    public event Action OnUpdate;
    void Update()
    {
        if (OnUpdate != null)
        {
            OnUpdate();
        }
    }

    public override void OnInit()
    {
    }
    public override void Close()
    {
        OnUpdate = null ;
        base.Close();
    }
}
