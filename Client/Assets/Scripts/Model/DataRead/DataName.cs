
using System;
using System.Collections.Generic;


public static class DataName
{
    public const string EndColor = "</color>";
    const string FileName = "DataName";
    static Dictionary<string, string> NameAry = new Dictionary<string, string>();
    static Dictionary<string, string> ColorAry = new Dictionary<string, string>();

    static DataName()
    {
        ReadData();
    }

    static void ReadData()
    {
        string[] data = SaveSprite.ReadFile(DataManager.GetExcelPath(FileName))
            .Replace(" ", "").Split(new string[] { "\r\n" }, StringSplitOptions.None);
        NameAry.Clear();
        ColorAry.Clear();
        for (int i = 0; i < data.Length; i++)
        {
            string[] value = data[i].Split(',');
            if (string.IsNullOrEmpty(value[0])) continue;
            if (NameAry.ContainsKey(value[0]))
            {
                //UnityEngine.Debug.LogError(
                //    string.Format("DataName Key 重复：key-{0},value-{1}", value[0], value[1]));
            }
            else
            {
                NameAry.Add(value[0], value[1]);
                ColorAry.Add(value[0], value[2]);
            }
        }
    }
    static public string Get_Name(int Type,int index,bool color)
    {
        switch (Type)
        {
            case 1:  return GetDataName(index, color);
            case 2: return GetAttributeName(index, color);
            case 3: return GetSkillName(index, color);
            case 4: return GetResistanceName(index, color);
        }
        return "";
    }
    static public string GetDataName(int index, bool color)
    {
        return GetName(((DataEnum)index).ToString(), color);
    }
    static public string GetAttributeName(int index, bool color)
    {
        return GetName(((AttributeEnum)index).ToString(), color);
    }
    static public string GetSkillName(int index, bool color)
    {
        return GetName(((skillEnum)index).ToString(), color);
    }
    static public string GetResistanceName(int index, bool color)
    {
        return GetName(((ResistanceEnum)index).ToString(), color);
    }
    static public string GetName(string key, bool color)
    {
        string value = "";
        try
        {
            value = NameAry[key];
            if (color)
            {
                value = GetColor(key, value);
            }
        }
        catch
        {
            UnityEngine.Debug.Log(key);
        }
        return value;
    }
    static string GetColor(string key)
    {
        try
        {
            return "<color=" + ColorAry[key] + ">";
        }
        catch
        {
            UnityEngine.Debug.Log(key);
        }
        return "";
    }
   public static string GetColor(string key,string name)
    {
        try
        {
            return "<color=" + ColorAry[key] + ">"+name+EndColor;
        }
        catch
        {
            UnityEngine.Debug.Log(key);
            return  "<color=#3D3D3D>" + name + EndColor;
        }
    }
    static public string GetColor(float EnumType, float Type)
    {
        string key = "";
        switch ((int)EnumType)
        {
            case 1:
                key = ((DataEnum)Type).ToString();
                break;
            case 2:
                key = ((AttributeEnum)Type).ToString();
                break;
            case 3:
                key = ((ResistanceEnum)Type).ToString();
                break;
            case 4:
                key = ((skillEnum)Type).ToString();
                break;
        }
        return GetColor(key);
    }
}

