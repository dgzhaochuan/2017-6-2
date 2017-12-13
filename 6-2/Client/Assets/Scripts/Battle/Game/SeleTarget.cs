using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeleTarget : UpdateGame
{
    public float speed = 10;
    protected override void OnUpdate()
    {
        transform.Rotate(Vector3.up*Time.deltaTime*speed);
    }
}
