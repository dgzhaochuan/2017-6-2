

using System;
using System.Collections.Generic;
using UnityEngine;

public enum AssetbundleEnum
{
    UnitIcon,
    PropIcon,
    EquipmentIcon,
    SkillIcon,
    SkillAudio
}

public class AssetBundleGame : BaseManaget
{
     Dictionary<AssetbundleEnum, string> PathAry = new Dictionary<AssetbundleEnum, string>();

    public override void OnInit()
    {
        PathAry = new Dictionary<AssetbundleEnum, string>();
        PathAry.Add(AssetbundleEnum.UnitIcon, "AssetBundle/Icon/UnitIcon/");
        PathAry.Add(AssetbundleEnum.PropIcon, "AssetBundle/Icon/PropIcon/");
        PathAry.Add(AssetbundleEnum.EquipmentIcon, "AssetBundle/Icon/EquipmentIcon/");
        PathAry.Add(AssetbundleEnum.SkillIcon, "AssetBundle/Icon/SkillIcon/");
        PathAry.Add(AssetbundleEnum.SkillAudio, "AssetBundle/Audio/SkillAudio/");
    }

    public T GetGame<T>(AssetbundleEnum type, string name) where T : UnityEngine.Object
    {
        string path = PathAry[type] + name;
        return Resources.Load<T>(path);
    }
    public Sprite GetPropSprite(BaseAdditionalAttribute attribute)
    {
        AssetbundleEnum _Enum = AssetbundleEnum.EquipmentIcon;
        switch (attribute.PropType)
        {
            case AdditionalAttributeEnum.equipment:
                _Enum = AssetbundleEnum.EquipmentIcon;
                break;
            case AdditionalAttributeEnum.prop:
                _Enum = AssetbundleEnum.PropIcon;
                break;
        }
        return GetGame<Sprite>(_Enum, attribute.icon);
    }

    public override void Close()
    {
        Debug.LogError("destory assetBund");
        base.Close();
    }
}

