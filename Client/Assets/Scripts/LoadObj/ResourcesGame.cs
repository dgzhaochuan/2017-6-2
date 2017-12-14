
using System.Collections.Generic;
using UnityEngine;


public enum ResourcesEnum
{
    UIPrefab,
    UIPanel,
    BattleUIPanel,
    BattleUIPrefab,
    SkillPrefab,
    Prefab,
}
public class ResourcesGame : BaseManaget
{
    Dictionary<ResourcesEnum, string> _path;
    Dictionary<string, Object> ObjAry = new Dictionary<string, Object>();
    private void Awake()
    {
        _path = new Dictionary<ResourcesEnum, string>();
        _path.Add(ResourcesEnum.UIPanel, "UIPanel/");
        _path.Add(ResourcesEnum.UIPrefab, "UIPrefab/");
        _path.Add(ResourcesEnum.BattleUIPanel, "UIPanel/Battle/Panel/");
        _path.Add(ResourcesEnum.BattleUIPrefab, "UIPanel/Battle/Prefab/");
        _path.Add(ResourcesEnum.SkillPrefab, "Skill/");
        _path.Add(ResourcesEnum.Prefab, "Prefab/");
    }

    public string GetKay(ResourcesEnum type, string name)
    { return _path[type] +  name; }

    public Sprite GetSprite(ResourcesEnum type, string name)
    {
        return null;
    }

    public T GetObj<T>(ResourcesEnum type, string PrefabName, bool save) where T : Object
    {
        string path = GetKay(type, PrefabName);
        if (save)
        {
            if (ObjAry.ContainsKey(path))
                return ObjAry[path] as T;
        }
        T obj = Resources.Load<T>(path);
        if (save) ObjAry.Add(path, obj);
        return obj;
    }

    public GameObject GetObj(ResourcesEnum type, string PrefabName, bool save=false)
    {
        string path = GetKay(type, PrefabName);
        GameObject obj = null;
        if (save)
        {
            if (ObjAry.ContainsKey(path))
                return ObjAry[path]as GameObject;
        }
        obj = Resources.Load<GameObject>(path);
        if (save) ObjAry.Add(path, obj);
        return obj;
    }

    public override void OnInit()
    {
    }
    public override void Close()
    {
        ObjAry.Clear();
        base.Close();
    }
}

