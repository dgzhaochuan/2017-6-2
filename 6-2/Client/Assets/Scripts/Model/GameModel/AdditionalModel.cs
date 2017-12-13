
/// <summary>
/// 1是数据类型，
/// 2是枚举类型，
/// 3个是属性效果是否是百分比，
/// 4是效果值 ，
[System.Serializable]
public class AdditionalModel
{
    //初始值
    public float value;
    //加成是否是按比例计算
    public bool ratio;
    //加成数据类型
    public int Type;
    //数据枚举
    public int EnumType;

    public AdditionalModel(float[] data)
    {
        EnumType = (int)data[0];
        Type = (int)data[1];
        ratio = data[2] == 1;
        value = data[3];
    }
    public int GetValue() { return (int)value; }
    public int GetValue(BaseAttribute baseData)
    {
        if (ratio)
        {
            return (int)(GetAddValue(baseData) * value * .01f);
        }
        else
        {
            return (int)value;
        }
    }
    float GetAddValue(BaseAttribute baseData)
    {
        if (baseData == null) return 0;
        switch (EnumType)
        {
            case 1:
                return baseData[(DataEnum)Type];
            case 2:
                return baseData[(AttributeEnum)Type];
            case 3:
                return baseData[(ResistanceEnum)Type];
            case 4:
                return baseData[(skillEnum)Type];
            default:
                return 0;
        }
    }
}
