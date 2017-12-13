using System;
using System.Collections.Generic;
using System.Text;


/// <summary>
/// 额外属性
/// </summary>
[System.Serializable]
public class AdditionalAttribute
{
    public List<AdditionalModel> data = new List<AdditionalModel>();
    public List<AdditionalModel> attribute = new List<AdditionalModel>();
    public List<AdditionalModel> resistance = new List<AdditionalModel>();
    public List<AdditionalModel> skill = new List<AdditionalModel>();


    public AdditionalAttribute()
    {
        this.data = new List<AdditionalModel>();
        attribute = new List<AdditionalModel>();
        resistance = new List<AdditionalModel>();
        skill = new List<AdditionalModel>();
    }
    public AdditionalAttribute(List<float[]> _data)
    {
        data = new List<AdditionalModel>();
        attribute = new List<AdditionalModel>();
        resistance = new List<AdditionalModel>();
        skill = new List<AdditionalModel>();
        for (int i = 0; i < _data.Count; i++)
        {
            switch ((int)_data[i][0])
            {
                case 1:
                    this.data.Add(new AdditionalModel(_data[i]));
                    break;
                case 2:
                    this.attribute.Add(new AdditionalModel(_data[i]));
                    break;
                case 3:
                    this.resistance.Add(new AdditionalModel(_data[i]));
                    break;
                case 4:
                    this.skill.Add(new AdditionalModel(_data[i]));
                    break;
            }
        }
    }
    public void Set(int type, List<float[]> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            switch (type)
            {
                case 1:
                    this.data.Add(new AdditionalModel(data[i]));
                    break;
                case 2:
                    this.attribute.Add(new AdditionalModel(data[i]));
                    break;
                case 3:
                    this.resistance.Add(new AdditionalModel(data[i]));
                    break;
                case 4:
                    this.skill.Add(new AdditionalModel(data[i]));
                    break;
            }
        }
    }


    public bool IsNull
    {
        get
        {
            return data.Count == 0 && attribute.Count == 0 && resistance.Count == 0;
        }
    }

    public int[] GetDamage(int[] startValue, BaseAttribute _attribute)
    {
        int[] start = new int[2];
        startValue.CopyTo(start, 0);
        if (_attribute == null) return start;
        int damage = 0;
        int value = 0;

        for (int i = 0; i < data.Count; i++)
        {
            value = (int)data[i].GetValue(_attribute);
            damage += value;
        }
        for (int i = 0; i < attribute.Count; i++)
        {
            value = (int)attribute[i].GetValue(_attribute);
            damage += value;
        }
        for (int i = 0; i < resistance.Count; i++)
        {
            value = (int)resistance[i].GetValue(_attribute);
            damage += value;
        }
        for (int i = 0; i < skill.Count; i++)
        {
            value = (int)skill[i].GetValue(_attribute);
            damage += value;
        }
        int _damageoff = (int)(damage * Config.AtkOff);
        start[0] += damage - _damageoff;
        start[1] += damage + _damageoff;
        return start;
    }

    public void Additional(BaseAttribute base_attribute,BaseAttribute _attribute)
    {
        for (int i = 0; i < data.Count; i++)
        {
            _attribute[(DataEnum)data[i].Type] += (int)data[i].GetValue(base_attribute);
        }
        for (int i = 0; i < attribute.Count; i++)
        {
            _attribute[(AttributeEnum)attribute[i].Type] += (int)attribute[i].GetValue(base_attribute);
        }
        for (int i = 0; i < resistance.Count; i++)
        {
            _attribute[(ResistanceEnum)resistance[i].Type] += (int)resistance[i].GetValue(base_attribute);
        }
        for (int i = 0; i < skill.Count; i++)
        {
            _attribute[(skillEnum)skill[i].Type] += (int)skill[i].GetValue(base_attribute);
        }
    }

}
