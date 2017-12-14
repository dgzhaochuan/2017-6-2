using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class UnitModel
{
    public CharacterAttribute dataModel;

    public BaseAttribute buffAttribute = new BaseAttribute();
    public BaseAttribute skillAttribute = new BaseAttribute();
    public BaseAttribute equiAttribute { get { return dataModel.EquipmentAddition; } }
    public BaseAttribute finalAttribute = new BaseAttribute();
    public int this[AttributeEnum type] { get { return finalAttribute[type]; } set { finalAttribute[type] = value; } }
    public int this[skillEnum type] { get { return finalAttribute[type]; } set { finalAttribute[type] = value; } }
    public int this[ResistanceEnum type] { get { return finalAttribute[type]; } set { finalAttribute[type] = value; } }
    public int this[DataEnum type] { get { return finalAttribute[type]; } set { finalAttribute[type] = value; } }



    private Dictionary<int, List<AdditionalModel>> _buffAry = new Dictionary<int, List<AdditionalModel>>();

    /// <summary>
    /// 1 AttributeEnum
    /// 2 SkillEnum
    /// 3 DataEnum
    /// 4 ResistanceEnum
    /// </summary>
    /// <param name="EnumType"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public Color GetColor(int EnumType, int type)
    {
        float value = 0;
        switch (EnumType)
        {
            case 1:
                value = buffAttribute[(AttributeEnum)type] + equiAttribute[(AttributeEnum)type];
                break;
            case 2:
                value = buffAttribute[(skillEnum)type] + equiAttribute[(skillEnum)type];
                break;
            case 3:
                value = buffAttribute[type] + equiAttribute[type];
                break;
            case 4:
                value = buffAttribute[(ResistanceEnum)type] + equiAttribute[(ResistanceEnum)type];
                break;
        }
        if (value > 0)
            return Config.UpColor;
        else
            if (value < 0)
            return Config.DownColor;
        return Config.white;
    }
    public void OnUpdate()
    {
        finalAttribute.Clear();
        dataModel.baseAttribute.CopyFina();
        finalAttribute += dataModel.baseAttribute;
        finalAttribute += buffAttribute;
        finalAttribute += equiAttribute;
        finalAttribute.Update();
        UpdataSkillAttribute();
        finalAttribute += skillAttribute;
        finalAttribute.UpdateWeight(dataModel.CurrentWeight);
    }

    public void AddBuff(BuffModel buff)
    {
        if (_buffAry.ContainsKey(buff.id))
            _buffAry.Remove(buff.id);
        _buffAry.Add(buff.id, buff.AdditionalAttribute);
        UpdateBuffAttribute();
    }
    public void RemoveBuff(int id)
    {
        if (_buffAry.ContainsKey(id))
        {
            _buffAry.Remove(id);
            UpdateBuffAttribute();
        }
        else { Debug.LogError("not buff:" + id); }
    }
    void UpdateBuffAttribute()
    {
        buffAttribute.Clear();
        OnUpdate();
        foreach (var item in _buffAry.Values)
        {
            buffAttribute.AddListModel(finalAttribute, item);
        }
        OnUpdate();
    }
    void UpdataSkillAttribute()
    {
        skillAttribute = new BaseAttribute();
        SkillAttribute _skill = null;
        if (dataModel.skillArray == null) return;
        for (int i = 0; i < dataModel.skillArray.Length; i++)
        {
            for (int j = 0; j < dataModel.skillArray[i].skillAry.Count; j++)
            {
                _skill = Manage.Instance.Data.GetObj<SkillAttribute>(dataModel.skillArray[i].skillAry[j]);
                if (_skill.triggerType == 1) continue;
                if (_skill.effectType.Length == 0) continue;
                if (_skill.effectType[0] != 6) continue;
                if (_skill.IsNeedEquipment(dataModel) == false) continue;
                _skill.additionalAttribute.Additional(finalAttribute, skillAttribute);
            }
        }
    }
}

