using System;
using System.Collections.Generic;
using System.Text;



/// <summary>
/// 技能 技能是一次性效果，持续效果在buff里面
/// </summary>
[System.Serializable]
public class SkillAttribute : BaseModel
{

    /// <summary>
    /// 学习需求等级
    /// </summary>
    public int needLevel;
    /// <summary>
    /// 学习需要属性
    /// </summary>
    public List<AdditionalModel> StudyNeedAttribute;
    /// <summary>
    /// 效果属性
    /// </summary>
    public AdditionalAttribute additionalAttribute;
    /// <summary>
    /// 消耗
    /// </summary>
    public List<AdditionalModel> NeedAttribute;
    /// <summary>
    /// 触发类型 1主动/被动
    /// </summary>
    public int triggerType;
    /// <summary>
    /// 目标敌方/己方
    /// </summary>
    public targetTypeEnum targetType;
    /// <summary>
    /// 0无，选择地图格子就能释放
    /// 1敌人，选择敌人
    /// 2队友，选中队友
    /// </summary>
    public int releaseType;
    /// <summary>
    /// 释放范围
    /// </summary>
    public int releaseRange;
    /// <summary>
    /// 作用范围
    /// </summary>
    public int targetRange;
    //需求武器具体类型
    public int[] needEquipment;
    /// <summary>
    /// 出现几率:
    ///buffID：
    ///持续时间：
    ///添加还是解除:(1添加)
    ///出现目标（1目标：自己）
    /// </summary>
    public List<IntArray> buff = new List<IntArray>();
    public skillEnum skillType;
    public ResistanceEnum damageType;
    public string animationClip;
    public int[] startValue = new int[0];

    public int[] CD;
    public int[] AP;
    /// <summary>
    /// 
    /// </summary>描述类型
    public int[] effectType;

    public string model;


    void SetStudyNeedAttribute(List<float[]> data)
    {
        StudyNeedAttribute = data.GetAdditionalModel();
    }
    void SetAdditionalAttribute(List<float[]> data)
    {
        additionalAttribute = new AdditionalAttribute(data);
    }
    void SetDataModel(List<float[]> data)
    {
        NeedAttribute = data.GetAdditionalModel();
    }
    void SetBuff(List<int[]> data)
    {
        buff = new List<IntArray>();
        for (int i = 0; i < data.Count; i++)
        {
            buff.Add(new IntArray(data[i]));
        }
    }
    


    public string GetAPText(BaseAttribute character)
    {
        string s = "AP:";
        if (character != null)
        {
            s += GetAP(character).ToString();
        }
        if (AP.Length > 1)
        {
            s += string.Format("({0}-{1},得益于{2})", AP[0], AP[1], DataName.GetSkillName(AP[2], true));
        }
        return s;
    }
    public int GetAP(BaseAttribute character)
    {
        int _ap = AP[0];
        if (AP.Length > 1)
        {
            _ap = Extend.GetCurrentValue(AP[0], AP[1], character[(skillEnum)AP[2]], AP[3]);
            _ap= _ap.Clamp(AP[0], AP[1]);
        }
        return _ap;
    }
    public string GetCDText(BaseAttribute character)
    {
        string s = "CD:";
        if (character != null)
        {
            s += GetCD(character).ToString();
        }
        if (CD.Length > 1)
            s += string.Format("({0}-{1},得益于{2})", CD[0], CD[1], DataName.GetSkillName(CD[2], true));
        return s;
    }
    public int GetCD(BaseAttribute character)
    {
        int _cd = CD[0];
        if (CD.Length > 1)
        {
            _cd = Extend.GetCurrentValue(CD[0], CD[1], character[(skillEnum)CD[2]], CD[3]);
            _cd= _cd.Clamp(CD[0], CD[1]);
        }
        return _cd;
    }
    public int[] GetDamage(BaseAttribute _attribute)
    {
        return additionalAttribute.GetDamage(startValue, _attribute);
    }
    public string GetEffect(BaseAttribute _attribute)
    {
        List<string> obj = new List<string>();
        for (int i = 0; i < effectType.Length; i++)
        {
            BuffModel _buff = null;
            if (buff.Count > 0)
            {
                //TODO  先只有一个buff
                _buff = Manage.Instance.Data.GetObj<BuffModel>(buff[0].data[1]);
            }
            switch (effectType[i])
            {
                case 1:
                    int[] damage = GetDamage(_attribute);
                    obj.Add(damage[0].ToString());
                    obj.Add(damage[1].ToString());
                    break;
                case 2:
                    obj.Add(DataName.GetColor(_buff.name,_buff.name));
                    break;
                case 3:
                    obj.Add(DataName.GetColor(_buff.name,buff[0].data[2].ToString()));
                    break;
                case 4:
                    obj.Add(_buff.GetEffect(_attribute));
                    break;
                case 5:
                    for (int j = 0; j < startValue.Length; j++)
                    {
                        obj.Add(startValue[j].ToString());
                    }
                    break;
                case 6:
                    for (int j = 0; j < additionalAttribute.data.Count; j++)
                    {
                        string str = additionalAttribute.data[j].GetValue().ToString();
                        if (additionalAttribute.data[j].ratio)
                            str += "%";
                        obj.Add(str);
                    }
                    for (int j = 0; j < additionalAttribute.attribute.Count; j++)
                    {
                        string str = additionalAttribute.data[j].GetValue().ToString();
                        if (additionalAttribute.data[j].ratio)
                            str += "%";
                        obj.Add(str);
                    }
                    for (int j = 0; j < additionalAttribute.resistance.Count; j++)
                    {
                        string str = additionalAttribute.data[j].GetValue().ToString();
                        if (additionalAttribute.data[j].ratio)
                            str += "%";
                        obj.Add(str);
                    }
                    for (int j = 0; j < additionalAttribute.skill.Count; j++)
                    {
                        string str = additionalAttribute.data[j].GetValue().ToString();
                        if (additionalAttribute.data[j].ratio)
                            str += "%";
                        obj.Add(str);
                    }
                    break;
            }
        }
        try
        {
            string[] str = obj.ToArray();
            effect= effect.Replace("~", ",");
            return string.Format(effect, str);
        }
        catch
        {
            string s = effect;
            return "";
        }
    }
    /// <summary>
    /// 
    /// </summary>装备武器是否满足技能触发/使用条件
    /// <param name="dataModel"></param>
    /// <returns></returns>
    public bool IsNeedEquipment(CharacterAttribute character)
    {
        if (needEquipment == null || needEquipment.Length == 0) return true;
        EquipmentAttribute equipment = null;
        switch (needEquipment[0])
        {
            case 1:
                    if (character.weapon[0] == 0) return false;
                    equipment = Manage.Instance.Data.GetObj<EquipmentAttribute>(character.weapon[0]);
                    if (equipment.IsMelee != 1) return false;
                    if (character.weapon[1] != 0)
                    {
                        equipment = Manage.Instance.Data.GetObj<EquipmentAttribute>(character.weapon[1]);
                        if (equipment.IsMelee != 1) return false;
                    }
                    return true;
            case 2:
                    if (character.weapon[0] == 0) return false;
                    equipment = Manage.Instance.Data.GetObj<EquipmentAttribute>(character.weapon[0]);
                    if ((int)equipment.type != needEquipment[1]) return false;
                    if (character.weapon[1] != 0)
                    {
                        equipment = Manage.Instance.Data.GetObj<EquipmentAttribute>(character.weapon[1]);
                        if ((int)equipment.type != needEquipment[1]) return false;
                    }
                    return true;
            case 3:
                    if (character.weapon[0] == 0) return false;
                    equipment = Manage.Instance.Data.GetObj<EquipmentAttribute>(character.weapon[0]);
                    if ((int)equipment.weaponType != needEquipment[1]) return false;
                    if (character.weapon[1] != 0)
                    {
                        equipment = Manage.Instance.Data.GetObj<EquipmentAttribute>(character.weapon[1]);
                        if ((int)equipment.weaponType != needEquipment[1]) return false;
                    }
                    return true;
        }
        return false;
    }

}
