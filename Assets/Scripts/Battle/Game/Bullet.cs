

using System;
using UnityEngine;

[RequireComponent(typeof(BulletFly))]
public class Bullet : PoolObj
{
    Action HitEvent;
    GameObject hitObject;
    BulletFly fly;
    public override void Init()
    {
        fly = GetComponent<BulletFly>();
        hitObject = transform.Find("hit").gameObject;
    }
    public void Play(Vector3 point, HexCell target, Action hit)
    {
        this.HitEvent = hit;
        hitObject.SetActive(false);
        fly.Play(point,target, OnHit);
    }

    void OnHit()
    {
        hitObject.SetActive(true);
        if (HitEvent != null) HitEvent();
    }
}