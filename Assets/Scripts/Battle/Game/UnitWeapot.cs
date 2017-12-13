

using BattleState;
using PigeonCoopToolkit.Effects.Trails;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitWeapot : MonoBehaviour
{
    //L是副手
    public Transform Hand_L, Hand_R, Shield;
    Dictionary<string, GameObject> Weapot_L;
    Dictionary<string, GameObject> Weapot_R;
    Dictionary<string, GameObject> Shield_L;
    GameObject Curren_R, Curren_L;
    Unit unit;
    TrailRenderer_Base Trail;
    DamageMessage damage;
    HexCell TargetCell;
    EquipmentAttribute weapon;
    void Awake()
    {
        unit = GetComponent<Unit>();
        Weapot_L = new Dictionary<string, GameObject>();
        Weapot_R = new Dictionary<string, GameObject>();
        Shield_L = new Dictionary<string, GameObject>();
        foreach (Transform item in Hand_L)
        {
            if (item.name.Contains("Finger")) continue;
            Weapot_L.Add(item.name, item.gameObject);
            item.gameObject.SetActive(false);
        }
        foreach (Transform item in Hand_R)
        {
            if (item.name.Contains("Finger")) continue;
            Weapot_R.Add(item.name, item.gameObject);
            item.gameObject.SetActive(false);
        }
        foreach (Transform item in Shield)
        {
            Shield_L.Add(item.name, item.gameObject);
            item.gameObject.SetActive(false);
        }
    }
    void Start()
    {
        unit.animation.AddEvent(IntEvent, FolatEvent, StringEvent, ObjEvent);
    }

    public void UpdateWwapot(CharacterAttribute data)
    {
        //TODO
        if (Curren_L != null) Curren_L.SetActive(false);
        if (Curren_R != null) Curren_R.SetActive(false);
        if (data.weapon[0] != 0)
        {
            weapon = Manage.Instance.Data.GetObj<EquipmentAttribute>(data.weapon[0]);
            if (weapon != null)
            {
                foreach (var item in Weapot_R.Values)
                {
                    Curren_R = item.gameObject;
                    Curren_R.SetActive(true);
                    Trail = Curren_R.GetComponentInChildren<TrailRenderer_Base>();
                    break;
                }
            }
        }
        if (data.weapon[1] != 0)
        {
            EquipmentAttribute eq = Manage.Instance.Data.GetObj<EquipmentAttribute>(data.weapon[1]);
            if (eq != null)
            {
                if (eq.weaponType == weaponEnum.dun)
                {
                    foreach (var item in Shield_L.Values)
                    {
                        Curren_L = item.gameObject;
                        Curren_L.SetActive(true);
                        break;
                    }
                }
                else
                {
                    foreach (var item in Weapot_L.Values)
                    {
                        Curren_L = item.gameObject;
                        Curren_L.SetActive(true);
                        break;
                    }
                }
            }
        }

    }
    public void AtkTarget(EquipmentAttribute model, HexCell TargetCell)
    {
        this.TargetCell = TargetCell;
        int[] _damage = model.GetDamage(unit.DataModel.dataModel.baseAttribute);
        damage = new DamageMessage(unit, _damage, ResistanceEnum.none, null, null);
    }
    void OnAtk()
    {
        if (weapon.weaponType == weaponEnum.gong || weapon.weaponType == weaponEnum.nu)
        {
           var arrow= ObjectPooling.Instance.GetObject<Bullet>(PoolEnum.Arrow);
            arrow.Play(unit.FirePoint.position, TargetCell, OnDamage);
        }
        else
        {
            OnDamage();
        }
    }
    void OnDamage()
    {
        TargetCell.unit.Damage(damage);
        Manage.Instance.Battle.ChangeState<SelectUnitBattleState>();
    }
    void IntEvent(int msg)
    {
        switch (msg)
        {
            case 0:
                if (Trail != null) Trail.Emit = true;
                break;
            case 1:
                OnAtk();
                break;
            case 2:
                if (Trail != null) Trail.Emit = false;
                break;
        }
    }
    void FolatEvent(float msg) { }
    void StringEvent(string msg) { }
    void ObjEvent(Object msg) { }

}