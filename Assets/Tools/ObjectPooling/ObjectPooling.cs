using System;
using UnityEngine;
using System.Collections.Generic;

public enum PoolEnum
{
    Arrow,
    CcrossbowArrow
}
public class ObjectPooling:MonoBehaviour
{
    private static ObjectPooling _instance;
    public static ObjectPooling Instance
    {
        get
        {
            if(_instance==null)
            {
                GameObject game = new GameObject("ObjectPooling");
               _instance= game.AddComponent<ObjectPooling>();
            }
            return _instance;
        }
    }

    Dictionary<string, Queue<PoolObj>> Ary = new Dictionary<string, Queue<PoolObj>>();

    Dictionary<string, Transform> parents = new Dictionary<string, Transform>();
    Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

    public void AddObject(PoolObj obj)
    {
        string key = obj.GetType().FullName;
        if (!Ary.ContainsKey(key))
        {
            Ary.Add(key, new Queue<PoolObj>());
        }
        Ary[key].Enqueue(obj);
    }

    public T GetObject<T>(PoolEnum _key, Transform parent=null) where T : PoolObj
    {
        string key = _key.ToString();
        if (!Ary.ContainsKey(key))
        {
            Ary.Add(key, new Queue<PoolObj>());
            prefabs.Add(key,Manage.Instance.Resources.GetObj(ResourcesEnum.Prefab,key));
        }
        if (Ary[key].Count == 0)
        {
            GameObject game = Instantiate(prefabs[key]);
            if (parent == null) parent = GetParent(key);
            game.SetParent(parent);
            T obj = game.GetComponent<T>();
            if (obj == null) obj = game.AddComponent<T>();
            obj.Init();
            Ary[key].Enqueue(obj);
        }
        return Ary[key].Dequeue().GetComponent<T>();
    }
    public T GetObject<T>(GameObject prefab, Transform parent) where T : PoolObj
    {
        string key = GetKey<T>();
        if (!Ary.ContainsKey(key))
        {
            Ary.Add(key, new Queue<PoolObj>());
        }
        if (Ary[key].Count == 0)
        {
            GameObject game = Instantiate(prefab);
            if (parent == null) parent = GetParent(key);
            game.SetParent(parent);
            T obj = game.GetComponent<T>();
            if (obj == null) obj = game.AddComponent<T>();
            obj.Init();
            Ary[key].Enqueue(obj);
        }
        return Ary[key].Dequeue().GetComponent<T>();
    }

    Transform GetParent(string name)
    {
        if (parents.ContainsKey(name) == false)
        {
            Transform parent = new GameObject().transform;
            parent.gameObject.SetParent(transform);
            parents.Add(name,parent);
        }
       return parents[name];
    }
    string GetKey<T>()
    {
        return typeof(T).FullName;
    }
}