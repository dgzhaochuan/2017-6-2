using System.Collections.Generic;


    [System.Serializable]
    public struct SkillSaveModel
    {
        public int type;
        public List<int> skillAry;
        public int Count { get { return skillAry.Count; } }
        public SkillSaveModel(int type)
        {
            this.type = type;
            skillAry = new List<int>();
        }
    }

[System.Serializable]
public class CharacterAttribute : BaseModel
{
    /// <summary>
    /// 负重
    /// </summary>
    public float CurrentWeight;
    /// <summary>
    /// 
    /// </summary>新建一个类是因为要区分开不能技能点的技能
    public SkillSaveModel[] skillArray;
    public BaseAttribute baseAttribute = new BaseAttribute();
    public int attributeCount;
    public int userAttributeCount;
    public int skillCount;
    public int userSkillCount;


    
    public int propCount = Config.StartPropCount;
    public int[] weapon;
    public GameNumber[] prop;
    public int[] equipment;




    private BaseAttribute _EquipmentAddition;
    public BaseAttribute EquipmentAddition
    {
        get
        {
            if (_EquipmentAddition == null)
                UpdateEquipmentAddition();
            return _EquipmentAddition;
        }
    }


    void SetBaseAttribute(int[] value)
    {
        baseAttribute.SetData(value);
    }
    void SetAttribute(List<int[]> value)
    {
        baseAttribute.SetAttribute(2, value);
    }
    void SetResistance(List<int[]> value)
    {
        baseAttribute.SetAttribute(3, value);
    }
    void SetSkillData(List<int[]> value)
    {
        baseAttribute.SetAttribute(4, value);
    }
    void SetSkill(List<int[]> id)
    {
        skillArray = new SkillSaveModel[(int)skillEnum.length];
        for (int i = 0; i < (int)skillEnum.length; i++)
        {
            skillArray[i] = new SkillSaveModel(i);
        }
        for (int i = 0; i < id.Count; i++)
        {
            skillArray[id[i][0]].skillAry.Add(id[i][1]);
        }
    }
    void SetWeapon(int[] data)
    {
        weapon = new int[2];
        data.CopyTo(weapon,0);
    }
    void SetEquipment(int[] data)
    {
        equipment = new int[Config.EquipmentEnumLeng];
        data.CopyTo(equipment,0);
    }
    void SetProp(List<int[]> data)
    {
        if(data.Count> propCount) propCount=data.Count;
        prop = new GameNumber[propCount];
        for (int i = 0; i < data.Count; i++)
        {
            prop[i] = new GameNumber(data[i][0], data[i][1], AdditionalAttributeEnum.prop);
        }

    }

    public bool HaveSkill(SkillAttribute model)
    {
        return skillArray[(int)model.skillType].skillAry.Contains(model.id);
    }
    public bool AddProp(GameNumber game)
    {
        for (int i = 0; i < prop.Length; i++)
        {
            if (prop[i].IsNull)
            {
                prop[i] = game;
                return true;
            }
        }
        return false;
    }
    public bool RemoveProp(int number, int index)
    {
        if (prop[index].number < number) return false;
        prop[index].number -= number;
        if (prop[index].number <= 0)
            prop[index].SetNull();
        return true;
        //for (int i = 0; i < prop.Length; i++)
        //{
        //    if (prop[i].IsNull) continue;
        //    if (prop[i].Equals(game))
        //    {
        //        if (prop[i].number < game.number)
        //        {
        //           return 
        //        }
        //        prop[i].number -= game.number;
        //        if (prop[i].number <= 0)
        //            prop[i].SetNull();
        //    }
        //}
        //return false;
    }
    public bool PropNull(int number)
    {
        int count = 0;
        for (int i = 0; i < prop.Length; i++)
        {
            if (prop[i].IsNull)
            {
                count++;
                if (count >= number)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool IsUserWeapon(EquipmentAttribute Weapon)
    {
        switch (Weapon.type)
        {
            case EquipmentEnum.zhushou:
                if (weapon[0] == 0) return true;
                return PropNull(1);
            case EquipmentEnum.fushou:
                if (weapon[1] == 0) return true;
                return PropNull(1);
            case EquipmentEnum.danshou:
                if (weapon[1] == 0 || weapon[0] == 0) return true;
                return PropNull(1);
            case EquipmentEnum.shuangshou:
                int number = 0;
                if (weapon[0] != 0) number++;
                if (weapon[1] != 0) number++;
                return PropNull(number);
        }
        return true;
    }
    public bool UpdateWeapon(EquipmentAttribute newWeapon)
    {
        if (IsUserWeapon(newWeapon) == false) { return false; }
        int[] oldWeapon = new int[2];
        switch (newWeapon.type)
        {
            case EquipmentEnum.danshou:
                if (weapon[0] != 0 && weapon[1] == 0)
                {
                    oldWeapon[0] = weapon[1];
                    weapon[1] = newWeapon.id;
                }
                else
                {
                    oldWeapon[0] = weapon[0];
                    weapon[0] = newWeapon.id;
                }
                break;
            case EquipmentEnum.zhushou:
                oldWeapon[0] = weapon[0];
                weapon[0] = newWeapon.id;
                break;
            case EquipmentEnum.fushou:
                oldWeapon[0] = weapon[1];
                weapon[1] = newWeapon.id;
                break;
            case EquipmentEnum.shuangshou:
                weapon.CopyTo(oldWeapon, 0);
                weapon[0] = newWeapon.id;
                weapon[1] = 0;
                break;
            default:
                oldWeapon[0] = equipment[(int)newWeapon.type];
                equipment[(int)newWeapon.type] = newWeapon.id;
                break;
        }
        for (int i = 0; i < oldWeapon.Length; i++)
        {
            if (oldWeapon[i] != 0)
                AddProp(new GameNumber(oldWeapon[i], 1, AdditionalAttributeEnum.equipment));
        }
        UpdateEquipmentAddition();
        return true;
    }
    public bool NotEquipment(EquipmentAttribute eq)
    {
        if (PropNull(1))
        {
            bool find = false;
            for (int j = 0; j < equipment.Length; j++)
            {
                if (equipment[j] == eq.id)
                {
                    equipment[j] = 0;
                    find = true;
                    break;
                }
            }
            if (find == false)
                for (int i = 0; i < weapon.Length; i++)
                {
                    if (weapon[i] == eq.id)
                    {
                        weapon[i] = 0;
                        break;
                    }
                }
            AddProp(new GameNumber(eq.id, 1, AdditionalAttributeEnum.equipment));
            UpdateEquipmentAddition();
            return true;
        }
        return false;
    }
    void UpdateEquipmentAddition()
    {
        _EquipmentAddition = new BaseAttribute();
        for (int i = 0; i < equipment.Length; i++)
        {
            if (equipment[i] == 0) continue;
            EquipmentAttribute equi = Manage.Instance.Data.GetObj<EquipmentAttribute>(equipment[i]);
            if (equi == null) continue;
            _EquipmentAddition.UpdateAdditional(baseAttribute, equi.baseAttribute);
        }
    }

}
