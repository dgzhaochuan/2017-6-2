

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manage : MonoBehaviour
{
    public static Manage _Instance;
    public static Manage Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new GameObject("Manage").AddComponent<Manage>();
            }
            return _Instance;
        }
    }

    public InputManager _input;
    private UpdateManager _update;
    public LoadWMangae _load;

    private ResourcesGame _Resources;
    private AssetBundleGame _AssetBundle;
    private DataManager _Data;
    private BattleManager _BattleManager;

    public ResourcesGame Resources
    {
        get
        {
            if (_Resources == null)
                _Resources = start<ResourcesGame>();
            return _Resources;
        }
    }
    public AssetBundleGame AB
    {
        get
        {
            if (_AssetBundle == null) _AssetBundle = start<AssetBundleGame>();
            return _AssetBundle;
        }
    }
    public DataManager Data
    {
        get
        {
            if (_Data == null) _Data = start<DataManager>();
            return _Data;
        }
    }
    public BattleManager Battle
    {
        get
        {
            if (_BattleManager == null) _BattleManager = start<BattleManager>();
            return _BattleManager;
        }
    }
    public bool IsBattle { get { return _BattleManager != null; } }

    List<BaseManaget> array = new List<BaseManaget>();

    public void mAwake()
    {
        _input = start<InputManager>();
        _update = start<UpdateManager>();
        _load = start<LoadWMangae>();
        SceneManager.sceneLoaded += SceneManager_sceneUnloaded;
    }

    public void RemoveUpdate(Action action)
    {
        if (_update != null) _update.OnUpdate -= action;
    }
    public void AddUpdate(Action action)
    {
        if (_update == null) _update = start<UpdateManager>();
        _update.OnUpdate -= action;
        _update.OnUpdate += action;
    }

    T start<T>() where T : BaseManaget
    {
        T manager= gameObject.AddComponent<T>();
        manager.OnInit();
        array.Add(manager);
        return manager;
    }
    private void SceneManager_sceneUnloaded(Scene arg0, LoadSceneMode sceneMode)
    {
        print("loadScene:"+arg0.name+"  "+sceneMode);
        for (int i = 0; i < array.Count; i++)
        {
            array[i].Close();
        }
        Destroy(gameObject);
    }
    
}