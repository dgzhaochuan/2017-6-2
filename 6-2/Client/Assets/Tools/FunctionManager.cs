using System;
using System.Collections.Generic;
using UnityEngine;

public class FunctionManager : MonoBehaviour
{
    static private FunctionManager _instance;
    static public FunctionManager Game
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("FunctionManager").AddComponent<FunctionManager>();
            }
            return _instance;
        }
    }


    private event Action OnUpdate;
    private event Action OnFixedUpdate;
    private void Update()
    {
        if (OnUpdate != null)
            OnUpdate();
    }

    private void FixedUpdate()
    {
        if (OnFixedUpdate != null)
            OnFixedUpdate();
    }


    public void AddUpdate(Action function) { OnUpdate += function; }
    public void ReUpdate(Action function) { OnUpdate -= function; }
    public void AddFixedUpdate(Action function) { OnFixedUpdate += function; }
    public void ReFixedUpdate(Action function) { OnFixedUpdate -= function; }




}