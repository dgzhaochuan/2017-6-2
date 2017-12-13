using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FarAtkSkill : BaseSkillObject
{
    GameObject  BulletObj, HitObj, ObjectObj;
    BulletFly bulletFly;
    bool IsHit;
    public override void Init()
    {
        base.Init();
        BulletObj = transform.Find("Bullet").gameObject;
        HitObj = transform.Find("Hit").gameObject;
        ObjectObj = transform.Find("Object").gameObject;
        bulletFly = bulletFly.GetComponent<BulletFly>();
        IsHit = false;
    }
    public override void Exit()
    {
        ObjectObj.SetActive(false);
        BulletObj.SetActive(false);
        HitObj.SetActive(false);
        base.Exit();
    }
    protected override void Start_Skill()
    {
        base.Start_Skill();
        if (bulletFly != null)
        {
            bulletFly.Play(unit.FirePoint.position, target, OnAtk);
        }
        else
        {
            OnAtk();
        }
    }
    protected override void End_Move()
    {
        base.End_Move();
        if (IsHit) Exit();
        IsHit = true;
    }
    protected override void OnAtk()
    {
        base.OnAtk();
        //TODO  eff关闭后再推出
        HitObj.transform.position = BulletObj.transform.position;
        if (IsHit == false)
        {
            Exit();
        }
        IsHit = true;
        if (ObjectObj != null)
        {
            ObjectObj.transform.position = target.GetPoint;
            ObjectObj.SetActive(true);
        }
    }



}