using System;
using System.Collections.Generic;


/// <summary>
/// 属性点附加的数据比例
/// </summary>
[System.Serializable]
public static class AdditionalDataModel
{
    struct Addition
    {
        public int dataType;
        public int attributeType;
        public float value;
        public Addition(int _dataType, int _attributeType, float _value)
        {
            dataType = _dataType;
            attributeType = _attributeType;
            value = _value;
        }

        public float GetValue(BaseAttribute attribute)
        {
            return attribute[(AttributeEnum)attributeType] * value;
        }
    }

    const string FileName = "AdditionalAttribute";
    static Dictionary<AttributeEnum, List<Addition>> ModelAry = new Dictionary<AttributeEnum, List<Addition>>();

    static AdditionalDataModel()
    {
        Read();
    }

    static void Read()
    {
        string[] data = ReadData.OpenFile(DataManager.GetExcelPath(FileName)).Replace(" ", "").Split(new string[] { "\r\n" }, StringSplitOptions.None);
        List<int> _enum = new List<int>();
        string[] EnumValue = data[0].Split(',');
        for (int i = 1; i < EnumValue.Length; i++)
        {
            _enum.Add((int)(DataEnum)Enum.Parse(typeof(DataEnum), EnumValue[i]));
        }
        for (int i = 1; i < data.Length; i++)
        {
            if (string.IsNullOrEmpty(data[i])) continue;
            string[] value = data[i].Split(',');
            AttributeEnum type = (AttributeEnum)Enum.Parse(typeof(AttributeEnum), value[0]);
            List<Addition> dataAry = new List<Addition>();
            for (int j = 1; j < value.Length; j++)
            {
                if (string.IsNullOrEmpty(value[j])) continue;
                Addition model = new Addition(_enum[j - 1], i - 1, float.Parse(value[j]));
                dataAry.Add(model);
            }
            ModelAry.Add(type, dataAry);
        }
    }


    public static void OnUpdateAttribute(this BaseAttribute attribute)
    {
        foreach (var item in ModelAry)
        {
            if (attribute[item.Key] == 0) continue;
            for (int i = 0; i < item.Value.Count; i++)
            {
                float value= item.Value[i].GetValue(attribute);
                attribute[(DataEnum)item.Value[i].dataType] = attribute[item.Value[i].dataType] + (int)value;
            }
        }
    }
    
}
