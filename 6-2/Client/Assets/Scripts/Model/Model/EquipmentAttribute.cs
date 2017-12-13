using System;


/// <summary>
/// 装备
/// </summary>
[Serializable]
public class EquipmentAttribute : BaseAdditionalAttribute
{
    public EquipmentEnum type;
    /// <summary>
    /// 武器类型，懒得再键一个武器类了
    /// </summary>
    public weaponEnum weaponType;
    //是否是近战武器
    public int IsMelee;
    public override AdditionalAttributeEnum PropType
    {
        get
        {
            return AdditionalAttributeEnum.equipment;
        }
    }
    public override string getType
    {
        get
        {
            return type.ToString();
        }
    }
}
