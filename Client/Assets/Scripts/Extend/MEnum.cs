using System;
using System.Collections.Generic;
using System.Text;


    #region dataEnum
    public enum DataEnum
    {
        //改了这里需要改角色配置表，属性名配置表
        hp,
        mp,
        Atk,
    minatkRange,
    def,
        matk,
        mdef,
        move,
        baoji,
        shanbi,
        minzhong,
        fuzhong,
        sudu,
        xingdong,

        //下面这些是不显示的属性
        maxatkRange,
        startAP,
        addAP,
        maxAP,
        maxAtk,

        length = 19,
        showLength=14,
    }

    public enum AttributeEnum
    {
        liliang,
        tizhi,
        minjie,
        jingshen,
        yizhi,

        length = 5,
    }

    public enum ResistanceEnum
    {
    none,
        qi,
        shui,
        huo,
        tu,
        du,

        wuli,
        mofa,
        length = 5,
    }

    public enum skillEnum
    {
        zhanshi,
        tangke,
        shui,
        huo,
        qi,
        cike,
        gongjianshou,

        length=7,
    }

#endregion

public enum EquipmentEnum
    {
        tou,
        yifu,
        hushou,
        xiezi,

        zhushou,
        fushou,
        danshou,
        shuangshou,
    }
    public enum weaponEnum
    {
        none,
        jian,
        fu,
        dun,
        zhang,
        gong,
        nu,
    }
    public enum propEnum
    {
        //恢复
        recovery,
        //道具
        item,
    }
    public enum AdditionalAttributeEnum
    {
        none=-1,
        equipment=1,
        prop=2,
    }
public enum targetTypeEnum
{
    enemy,
    allenemy,
    teammate,
    allteammate,
    all,
}
    public enum buffEnum
    {
        none,
        daodi,
        yun,
        liuxue,
        chengmo,
        zhongdu,
        buff,
        dbuff,


    }

public enum HudTextEnum
{
    hp,mp,
}

