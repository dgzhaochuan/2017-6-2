
using System;
using UnityEngine;

public class BulletFly:UpdateGame
{
    public float speed=10;

     Action hitEvent;
    Vector3 end;
    Vector3 start;
    float _time;
    
    public void Play(Vector3 point,HexCell target,Action hit)
    {
        transform.position = point;
        hitEvent = hit;
        end = target.GetPoint;
        end.y = point.y;
        start = point;
        _time = 0;
    }

    protected override void OnUpdate()
    {
        transform.position = Lerp(transform.position, end, speed * Time.deltaTime);
    }

    Vector3 Lerp(Vector3 to,Vector3 from,float t)
    {
        _time += t;
        _time = Mathf.Clamp01(_time);
        if (_time >= 1) hitEvent();
        return to + (from - to) * _time;
    }
    
}