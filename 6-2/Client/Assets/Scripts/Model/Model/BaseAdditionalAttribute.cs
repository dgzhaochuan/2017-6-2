using System.Collections.Generic;


[System.Serializable]
public class BaseAdditionalAttribute : BaseModel
{
    public AdditionalAttribute baseAttribute = new AdditionalAttribute();
    public AdditionalAttribute needAttribute;
    public int _number;
    public int _maxNumber = 1;
    public int number
    {
        get { return _number; }
        set
        {
            _number = value;
            if (_number > maxNumber) _number = maxNumber;
        }
    }
    public int maxNumber
    {
        get { return _maxNumber; }
        set
        {
            _maxNumber = value;
            if (_maxNumber < 1) _maxNumber = 1;
        }
    }
    //重量
    public float weight;
    public int money;
    public int AP;
    public int UseAP;
    /// <summary>
    /// 技能携带的buff 出现几率:buffID：持续时间：添加还是解除:出现目标（1目标：己方）
    /// 释放一个技能可能会给目标和自己加不同的buff
    /// </summary>
    public List<IntArray> buff = new List<IntArray>();
    public int[] startValue = new int[0];
    /// <summary>
    /// 释放范围
    /// </summary>
    public int releaseRange;
    /// <summary>
    /// 作用范围
    /// </summary>
    public int targetRange;




    /// <summary>
    /// 是否满足使用/装备条件 
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public bool IsWear(UnitModel character)
    {
        AdditionalModel model = null;
        for (int i = 0; i < needAttribute.data.Count; i++)
        {
            model = needAttribute.data[i];
            if (character[(DataEnum)model.Type] < model.value) return false;
        }
        for (int i = 0; i < needAttribute.attribute.Count; i++)
        {
            model = needAttribute.attribute[i];
            if (character[(AttributeEnum)model.Type] < model.value) return false;
        }
        for (int i = 0; i < needAttribute.resistance.Count; i++)
        {
            model = needAttribute.resistance[i];
            if (character[(ResistanceEnum)model.Type] < model.value) return false;
        }
        for (int i = 0; i < needAttribute.skill.Count; i++)
        {
            model = needAttribute.skill[i];
            if (character[(skillEnum)model.Type] < model.value) return false;
        }
        return true;
    }

    /// <summary>
    /// 读表的时候反射调用，配置在表里面
    /// </summary>
    /// <param name="data"></param>     
    protected void SetData(List<float[]> value)
    {
        baseAttribute.Set(1, value);
    }
    protected void SetAttribute(List<float[]> value)
    {
        baseAttribute.Set(2, value);
    }
    protected void SetResistance(List<float[]> value)
    {
        baseAttribute.Set(3, value);
    }
    protected void SetSkill(List<float[]> value)
    {
        baseAttribute.Set(4, value);
    }
    protected void SetBuff(List<int[]> value)
    {
        buff = new List<IntArray>();
        for (int i = 0; i < value.Count; i++)
        {
            buff.Add(new IntArray(value[i]));
        }
    }
    public void SetBaseAttribute(List<float[]> data)
    {
        baseAttribute = new AdditionalAttribute(data);
    }
    public void SetNeedAttribute(List<float[]> data)
    {
        needAttribute = new AdditionalAttribute(data);
    }


    public virtual AdditionalAttributeEnum PropType { get { return AdditionalAttributeEnum.equipment; } }

    public virtual string getType { get { return "Type"; } }




    public int[] GetDamage(BaseAttribute _attribute)
    {
        return baseAttribute.GetDamage(startValue, _attribute);
    }
    public string GetEffect(BaseAttribute _attribute)
    {
        List<string> obj = new List<string>();
        if (startValue.Length > 0)
        {
            //造成{0}伤害
            int[] damage = GetDamage(_attribute);
            obj.Add(damage[0].ToString());
            obj.Add(damage[1].ToString());
        }
        for (int i = 0; i < buff.Count; i++)
        {

            //{0}%几率添加{1}，持续{2}回合
            obj.Add(buff[i].data[0].ToString()+"%");
            BuffModel _buff = Manage.Instance.Data.GetObj<BuffModel>(buff[i].data[1]);
            obj.Add(_buff.name);
            obj.Add(_buff.GetEffect(_attribute));
            obj.Add(buff[i].data[2].ToString());
        }
        return string.Format(effect.Replace("~",","), obj.ToArray());
    }

}
