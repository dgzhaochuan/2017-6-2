using System;

using UnityEngine;


public class Unit : MonoBehaviour,IDamage
{
    private TweenerTransform _tweener;
    public TweenerTransform tweener
    {
        get
        {
            if (_tweener == null)
            {
                _tweener = GetComponent<TweenerTransform>();
                if(_tweener==null)
                    _tweener = gameObject.AddComponent<TweenerTransform>();
                _tweener.ConsumeTime = Config.walkSpeed;
            }
            return _tweener;
        }
    }

    private UnitModel BaseModel;
    public UnitModel DataModel { get { return BaseModel; } }
    private Transform modelTransform; 
    public Transform ModelTransform
    {
        get
        {
            if (modelTransform == null)
            {
                modelTransform = transform.Find("Model");
            }
            return modelTransform;
        }
    }
    private Transform UIPoint;


    public HexCell cell;
    public bool isActive;
    public int ranks;
    public int CurrentAP;
    public int currentHP;
    public bool IsDie { get; private set; }
    public Transform FirePoint { get; private set; }
    public UnitIconQueueItem hud { get; private set; }
    public new AnimatorManager animation { get; private set; }

    UnitWeapot weapot;

    private void Awake()
    {
        UIPoint = transform.Find("UIPoint");
        FirePoint = transform.Find("FirePoint");
        animation = transform.GetComponentInChildren<Animator>().gameObject.AddComponent<AnimatorManager>();
        weapot = GetComponent<UnitWeapot>();
    }
    private void Start()
    {
        weapot.UpdateWwapot(DataModel.dataModel);
    }
    public void Init(UnitModel model,int ranks )
    {
        this.ranks = ranks;
        BaseModel = model;
        model.OnUpdate();
        currentHP = DataModel[DataEnum.hp]= DataModel[DataEnum.hp];
        CurrentAP = DataModel[DataEnum.startAP] = DataModel[DataEnum.startAP];
    }
    public void StartHud()
    {
        hud = BattlePanel.Instance.UnitIconQueue.AddUnitIcon(this);
        hud.UpdateValue(currentHP, currentHP);
        hud.name = DataModel.dataModel.name;
    }
    public void Reset()
    {
        isActive = true;
        //ResetPoint();
        CurrentAP += DataModel[DataEnum.startAP] = DataModel[DataEnum.addAP];
        CurrentAP = Mathf.Clamp(CurrentAP, 0, DataModel[DataEnum.maxAP]);
     
    }
    public void NoActive()
    {
        isActive = false;
    }
    public void ResetPoint()
    {
        transform.position = cell.GetPoint;
    }
    public bool IsMove(HexCell cell)
    {
        if (cell.obstacle != null) return false;
        if (cell.unit == null) return true;
        return false;
        //return cell.unit==null&&cell.obstacle==null;
    }
    public bool IsAtk(HexCell cell)
    {
        if (cell.obstacle != null) return false;
        if (cell.unit == null) return true;
        if (cell.unit != null && cell.unit.ranks != this.ranks) return true;
        return false;
    }
    public bool AtkRange(HexCell cell)
    {
        if (cell.obstacle != null) return false;
        if (cell.unit == null) return true;
        if (cell.unit != null && cell.unit == this) return true;
        return false;
    }
    public int  Hit(int value)
    {
        int damage = value;
        BattlePanel.Instance.UpdateHudText(UIPoint, damage, HudTextEnum.hp);
        currentHP -= damage;
        hud.UpdateValue(DataModel[DataEnum.hp], currentHP);
        if(currentHP <= 0)
        {
            print(BaseModel.dataModel.name+" die");
            Die();
            animation.SetTrigger(AnimatorManager.Death);
        }
        else
        {
            animation.SetTrigger(AnimatorManager.Hit);
        }
        return damage;
    }
    public int Damage(DamageMessage damage)
    {
        if (damage.buffadd != null)
        {
            for (int i = 0; i < damage.buffadd.Count; i++)
            {
                BuffManager(damage.buffadd[i], damage.buff[i]);
            }
        }
        return Hit(damage.hit);
    }
    public void Die()
    {
        IsDie = true;
        Manage.Instance.Battle.UnitDie(this);
    }
    public void Cure(int value)
    {

    }
    public bool IsPlayRank()
    {
        return Manage.Instance.Battle.Unitmanager.IsPlayerRanks(ranks);
    }
    public void UserApp(int ap)
    {
        CurrentAP -= ap;
    }
    public void BuffManager(bool add,BuffModel model)
    {
        if (add)
        {
            DataModel.AddBuff(model);
        }
        else
        {
            DataModel.RemoveBuff(model.id);
        }
    }
    public void OnAtk(EquipmentAttribute model,HexCell TargetCell)
    {
        //int[] _damage = model.GetDamage(DataModel.dataModel.baseAttribute);
        //DamageMessage damage = new DamageMessage(this, _damage, ResistanceEnum.none, null, null);
        //TargetCell.unit.Damage(damage);
        weapot.AtkTarget(model,TargetCell);
        ModelTransform.LookAt(TargetCell.transform);
        animation.SetTrigger(AnimatorManager.Attack);
    }


    public int GetValue(ResistanceEnum res = ResistanceEnum.none)
    {
        int value = 0;
        for (int i = 0; i < DataModel.finalAttribute.finalData.Length; i++)
        {
            value += Config.ValueData[i] * DataModel[(DataEnum)i];
        }
        value += CurrentAP * Config.ValueAp;

        switch (res)
        {
            case ResistanceEnum.wuli:
                //value -= DataModel[res] * Config.ValueResistance;
                break;
            case ResistanceEnum.mofa:
                //value -= DataModel[DataEnum.mdef] * Config.ValueDef;
                break;
            default:
                value -= DataModel[res] * Config.ValueResistance;
                break;
        }
        return value;
    }

    #region Test
    //private Renderer _renderer;
    //private new Renderer renderer
    //{
    //    get
    //    {
    //        if (_renderer == null)
    //        {
    //            _renderer = transform.Find("model/Sphere").GetComponent<Renderer>();
    //        }
    //        return _renderer;
    //    }
    //}
    //void TestChangeActive( )
    //{
    //    if (isActive)
    //    {
    //        renderer.material.color = Color.white;
    //    }
    //    else
    //    {
    //        renderer.material.color = Color.gray;
    //    }
    //}
    //void Test()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position, Vector3.down, out hit, 2, 1 << Config.HexLayer))
    //    {
    //        hit.collider.GetComponent<HexCell>().SetUnit(this);
    //    }
    //}
    #endregion
}