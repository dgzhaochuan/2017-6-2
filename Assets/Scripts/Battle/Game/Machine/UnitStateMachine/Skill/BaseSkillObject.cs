
using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class BaseSkillObject:PoolObj
{

    
    protected Unit unit;
    protected SkillAttribute attribute;
    protected List<Action> animationEvent;
    protected Action skillEnd;
    protected HexCell target;


    GameObject StartObj, FireObj;
    public override void Init()
    {
        StartObj = transform.Find("Start").gameObject;
        FireObj = transform.Find("Fire").gameObject;
        animationEvent = new List<Action>();
        animationEvent.Add(Start_Move);
        animationEvent.Add(Start_Skill);
        animationEvent.Add(End_Skill);
        animationEvent.Add(End_Move);
    }

    public virtual void Enter(Unit unit,SkillAttribute model,HexCell target,Action skillEnd)
    {
        this.unit = unit;
        this.target = target;
        this.skillEnd = skillEnd;
        attribute = model;
        start();
        unit.animation.AddEvent(IntEvent,FolatEvent,StringEvent,ObjEvent);
    }
    public virtual void Exit()
    {
        unit.animation.ReEvent(IntEvent, FolatEvent, StringEvent, ObjEvent);
        if (skillEnd != null) skillEnd();
        gameObject.SetActive(false);

    }
    protected virtual void IntEvent(int msg)
    {
        animationEvent[msg]();
        //switch (msg)
        //{
        //    case 1:
        //        Start_Move();
        //        break;
        //    case 2:
        //        Start_Skill();
        //        break;
        //    case 3:
        //        End_Skill();
        //        break;
        //    case 4:
        //        End_Move();
        //        break;
        //}
    }
    protected virtual void StringEvent(string msg) { }
    protected virtual void FolatEvent(float msg) { }
    protected virtual void ObjEvent(UnityEngine.Object msg) { }
    protected virtual void Start_Move()
    {
        StartObj.SetActive(true);
    }
    protected virtual void Start_Skill()
    {
        FireObj.SetActive(true);
    }
    protected virtual void End_Skill()
    {
        FireObj.SetActive(false);
    }
    protected virtual void End_Move()
    {
        StartObj.SetActive(false);
    }
    protected virtual void start()
    {
        unit.GetComponent<Animation>().Play(attribute.animationClip);
    }
    protected virtual bool AtkFunction(HexCell cell)
    {
        if (cell.unit == null) return false;
        if (attribute.targetType == targetTypeEnum.all) return true;
        return cell.unit.ranks != unit.ranks;
    }
    protected virtual List<Unit> GetEnemy()
    {
        List<Unit> targets = new List<Unit>();
        switch (attribute.targetType)
        {
            case targetTypeEnum.allenemy:
                Manage.Instance.Battle.Unitmanager.GetEnemy(unit.ranks);
                break;
            case targetTypeEnum.allteammate:
                Manage.Instance.Battle.Unitmanager.GetTeammate(unit.ranks);
                break;
            case targetTypeEnum.all:
                Manage.Instance.Battle.Unitmanager.GetAllUnit(unit.ranks);
                break;
            default:
                var cells = HexGrid.Instantiate.GetRange(unit.cell, attribute.releaseRange, 0, AtkFunction);
                foreach (var item in cells)
                {
                    targets.Add(item.unit);
                }
                break;
        }
        return targets;
    }
    protected virtual void OnAtk()
    {
        BuffModel _buff = null;
        List<BuffModel> damagebuff = new List<BuffModel>();
        List<bool> buffadd = new List<bool>();
        for (int i = 0; i < attribute.buff.Count; i++)
        {
            _buff = Manage.Instance.Data.GetObj<BuffModel>(attribute.buff[i].data[1]);
            if (_buff == null) continue;
            if (attribute.buff[i].data[4] == 1)
            {
                buffadd.Add(attribute.buff[i].data[3]==1);
                damagebuff.Add(_buff);
            }
            else
            {
                unit.BuffManager(attribute.buff[i].data[3] == 1, _buff);
            }
        }
        var targets = GetEnemy();
        if (targets.Count != 0)
        {
            int[] hit = attribute.GetDamage(unit.DataModel.finalAttribute);
            DamageMessage damage = new DamageMessage(unit, hit, attribute.damageType, damagebuff, buffadd);
            int value = 0;
            for (int i = 0; i < targets.Count; i++)
            {
                value += targets[i].Damage(damage);
            }
        }
    }

}