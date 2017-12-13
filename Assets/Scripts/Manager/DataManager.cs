using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;


public class DataManager : BaseManaget
{

    Dictionary<string, Dictionary<int, BaseModel>> ObjAry = new Dictionary<string, Dictionary<int, BaseModel>>();

    #region  Path
    public static string GetJsonPath(string fileName)
    {
        return Application.streamingAssetsPath + "/Json/" + fileName + JsonType;
    }
    public static string GetExcelPath(string fileName)
    {
        return Application.streamingAssetsPath + "/Excel/" + fileName + FileType;
    }
    public const string ExcelFileType = ".xlsx";
    public const string FileType = ".csv";
    public const string JsonType = ".txt";
    public const string PropFileName = "Prop";
    public const string EquipmentFileName = "Equipment";
    public const string CharacterFileName = "Character";
    public const string SkillFileName = "Skill";
    public const string BuffFileName = "buff";
    public const string MapNodeFileName = "MapNode";
    public const string NPCFileName = "NPC";
    public const string SceneFileName = "Scene";

    public static string GetKey<T>() where T : BaseModel { return typeof(T).ToString(); }
    #endregion

    public override void OnInit()
    {
        ReadData();
    }
    void ReadData()
    {
        ReadJson<EquipmentAttribute>(EquipmentFileName);
        ReadJson<PropAttribute>(PropFileName);
        ReadJson<CharacterAttribute>(CharacterFileName);
        ReadJson<SkillAttribute>(SkillFileName);
        ReadJson<BuffModel>(BuffFileName);
        ReadJson<SceneModel>(SceneFileName);
    }
    public  void ReadJson<T>(string FileName) where T : BaseModel
    {
        string path = DataManager.GetJsonPath(FileName);
        string data = File.ReadAllText(path, System.Text.Encoding.Default);
        SaveObject<T> model = JsonUtility.FromJson<SaveObject<T>>(data);
        AddAry(model);
    }
    void AddAry<T>(SaveObject<T> model) where T : BaseModel
    {
        string key = GetKey<T>();
        if (ObjAry.ContainsKey(key) == false)
        {
            ObjAry.Add(key, new Dictionary<int, BaseModel>());
        }
        for (int i = 0; i < model.ObjAry.Count; i++)
        {
            ObjAry[key].Add(model.ObjAry[i].id, model.ObjAry[i]);
        }
    }
    public Dictionary<int, BaseModel> GetDictionary<T>() where T : BaseModel
    {
        return ObjAry[GetKey<T>()];
    }
    public T GetObj<T>(int id) where T : BaseModel
    {
        try
        {
            return ObjAry[GetKey<T>()][id] as T;
        }
        catch (Exception e)
        {
            //UnityEngine.Debug.LogError(string.Format("Type:{0},ID:{1}", typeof(T).ToString(), id));
            return null;
        }
    }
    public List<T> GetObjAry<T>() where T : BaseModel
    {
        var ary = ObjAry[GetKey<T>()].Values.ToList();
        List<T> data = new List<T>();
        for (int i = 0; i < ary.Count; i++)
        {
            data.Add(ary[i] as T);
        }
        return data;
    }
    public BaseAdditionalAttribute GetBaseProp(GameNumber prop)
    {
        BaseAdditionalAttribute attribute = null;
        switch ((AdditionalAttributeEnum)prop.PropType)
        {
            case AdditionalAttributeEnum.equipment:
                attribute =
                Manage.Instance.Data.GetObj<EquipmentAttribute>(prop.id);
                break;
            case AdditionalAttributeEnum.prop:
                attribute =
                Manage.Instance.Data.GetObj<PropAttribute>(prop.id);
                break;
        }
        return attribute;
    }

}
