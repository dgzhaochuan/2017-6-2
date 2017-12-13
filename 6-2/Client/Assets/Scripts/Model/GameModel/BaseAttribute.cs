
using System.Collections.Generic;


/// <summary>
/// 一组数据合集，比如说所有装备附加属性，所有buff附加属性
/// </summary>
[System.Serializable]
public class BaseAttribute
{
    public int[] finalData = new int[(int)DataEnum.length];
    public int[] data = new int[(int)DataEnum.length];
    public int[] attribute = new int[(int)AttributeEnum.length];
    public int[] resistance = new int[(int)ResistanceEnum.length];
    public int[] skill = new int[(int)skillEnum.length];


    public int GetFinalDataValue(DataEnum type) { return finalData[(int)type]; }
    public void SetFinalDataValue(DataEnum type, int value) { finalData[(int)type] = value; }
    public int GetAttributeValue(AttributeEnum type) { return attribute[(int)type]; }
    public void SetAttributeValue(AttributeEnum type, int value) { finalData[(int)type] = value; }
    public int GetResistanceValue(ResistanceEnum type) { return resistance[(int)type]; }
    public void SetResistanceValue(ResistanceEnum type, int value) { resistance[(int)type] = value; }
    public int GetSkillValue(skillEnum type) { return skill[(int)type]; }
    public void SetSkillValue(skillEnum type, int value) { skill[(int)type] = value; }
    public int GetDataValue(DataEnum type) { return data[(int)type]; }
    public void SetDataValue(DataEnum type, int value) { data[(int)type] = value; }

    public int this[DataEnum type] { get { return finalData[(int)type]; } set { finalData[(int)type] = value; } }
    public int this[AttributeEnum type] { get { return attribute[(int)type]; } set { attribute[(int)type] = value; } }
    public int this[ResistanceEnum type] { get { return resistance[(int)type]; } set { resistance[(int)type] = value; } }
    public int this[skillEnum type] { get { return skill[(int)type]; } set { skill[(int)type] = value; } }
    public int this[int type] { get { return data[type]; } set { data[type] = value; } }


    public BaseAttribute()
    {
        finalData = new int[(int)DataEnum.length];
        data = new int[(int)DataEnum.length];
        attribute = new int[(int)AttributeEnum.length];
        resistance = new int[(int)ResistanceEnum.length];
        skill = new int[(int)skillEnum.length];
    }

    void Set(int type, int[] value)
    {
        switch (type)
        {
            case 1:
                this[value[0]] = value[1];
                break;
            case 2:
                this[(AttributeEnum)value[0]] = value[1];
                break;
            case 3:
                this[(ResistanceEnum)value[0]] = value[1];
                break;
            case 4:
                this[(skillEnum)value[0]] = value[1];
                break;
        }
    }
    public void SetAttribute(int type, List<int[]> value)
    {
        for (int i = 0; i < value.Count; i++)
        {
            Set(type, value[i]);
        }
        Update();
    }
    /// <summary>
    /// 
    /// </summary>单独设置角色属性，只有角色才有，所有数据都设置
    /// <param name="value"></param>
    public void SetData(int[] value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            this[i] = value[i];
        }
        Update();
    }
    /// <summary>
    /// 
    /// </summary>单独设置数据
    /// <param name="value"></param>
    void SetData(List<int[]> value)
    {
        SetAttribute(1, value);
    }
    /// <summary>
    /// 
    /// </summary>单独设置属性点
    /// <param name="value"></param>
    void SetAttribute(List<int[]> value)
    {
        SetAttribute(2, value);
    }
    /// <summary>
    /// 
    /// </summary>单独设置抗性
    /// <param name="value"></param>
    void SetResistance(List<int[]> value)
    {
        SetAttribute(3, value);
    }
    /// <summary>
    /// 
    /// </summary>单独设置技能
    /// <param name="value"></param>
    void SetSkill(List<int[]> value)
    {
        SetAttribute(4, value);
    }

    public void CopyFina()
    {
        data.CopyTo(finalData, 0);
    }
    public void Update()
    {        
       // data.CopyTo(finalData, 0);
        this.OnUpdateAttribute();
    }
    public void UpdateWeight(float CurrentWeight)
    {

        float ratio = CurrentWeight / this[DataEnum.fuzhong];
        if (ratio > 1)
        {
            ratio -= 1;
            this[AttributeEnum.minjie] -= (this[AttributeEnum.minjie] * ratio).ToInt();
            if (this[AttributeEnum.minjie] <= 0) this[AttributeEnum.minjie] = 1;
            Update();
            CorrectData(DataEnum.move, ratio);
            CorrectData(DataEnum.baoji, ratio);
            CorrectData(DataEnum.shanbi, ratio);
            CorrectData(DataEnum.minzhong, ratio);
            CorrectData(DataEnum.sudu, ratio * 1.2f);
        }
    }

    public void Clear()
    {
        finalData = new int[(int)DataEnum.length];
        data = new int[(int)DataEnum.length];
        attribute = new int[(int)AttributeEnum.length];
        resistance = new int[(int)ResistanceEnum.length];
        skill = new int[(int)skillEnum.length];
    }
    public void AddSkill(int[] value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            skill[i] += value[i];
        }
    }
    void CorrectData(DataEnum type, float ratio)
    {
        this[type] -= (this[type] * ratio).ToInt();
        if (this[type] <= 0) this[type] = 1;
    }



    public static BaseAttribute operator +(BaseAttribute a, BaseAttribute b)
    {
        BaseAttribute attribute = new BaseAttribute();
        //a.Update();
        //b.Update();
        for (int i = 0; i < (int)DataEnum.length; i++)
        {
            attribute[(DataEnum)i] = a[(DataEnum)i] + b[(DataEnum)i];
            attribute[i] = a[i] + b[i];
        }
        for (int i = 0; i < (int)AttributeEnum.length; i++)
        {
            attribute[(AttributeEnum)i] = a[(AttributeEnum)i] + b[(AttributeEnum)i];
        }
        for (int i = 0; i < (int)ResistanceEnum.length; i++)
        {
            attribute[(ResistanceEnum)i] = a[(ResistanceEnum)i] + b[(ResistanceEnum)i];
        }
        for (int i = 0; i < (int)skillEnum.length; i++)
        {
            attribute[(skillEnum)i] = a[(skillEnum)i] + b[(skillEnum)i];
        }
        return attribute;
    }
    public static BaseAttribute operator -(BaseAttribute a, BaseAttribute b)
    {
        BaseAttribute attribute = new BaseAttribute();
        //a.Update();
        //b.Update();
        for (int i = 0; i < (int)DataEnum.length; i++)
        {
            attribute[(DataEnum)i] = a[(DataEnum)i] - b[(DataEnum)i];
            attribute[i] = a[i] - b[i];
        }
        for (int i = 0; i < (int)AttributeEnum.length; i++)
        {
            attribute[(AttributeEnum)i] = a[(AttributeEnum)i] - b[(AttributeEnum)i];
        }
        for (int i = 0; i < (int)ResistanceEnum.length; i++)
        {
            attribute[(ResistanceEnum)i] = a[(ResistanceEnum)i] - b[(ResistanceEnum)i];
        }
        for (int i = 0; i < (int)skillEnum.length; i++)
        {
            attribute[(skillEnum)i] = a[(skillEnum)i] - b[(skillEnum)i];
        }
        return attribute;
    }
}
