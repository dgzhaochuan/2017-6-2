

using System.Collections.Generic;
using UnityEngine;

public class BattlePanel :MonoBehaviour
{
    private static BattlePanel  _instance;
    public static BattlePanel  Instance
    {
        get {
            if(_instance==null)
            {
                GameObject Canvas = null;
                //Canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
                if (Canvas == null)
                {
                    GameObject CanvasPrefab = Manage.Instance.Resources.GetObj(ResourcesEnum.UIPrefab, "Canvas");
                    Canvas = Instantiate<GameObject>(CanvasPrefab);
                    Canvas.GetComponent<Canvas>().sortingOrder = 0;
                }
                GameObject panelPrefab= Manage.Instance.Resources.GetObj(ResourcesEnum.BattleUIPanel, "BattlePanel");               
                GameObject panel = Instantiate<GameObject>(panelPrefab);
                panel.transform.UIReset(Canvas.transform);
                _instance = panel.AddComponent<BattlePanel >();
            }
            return _instance;
        }
    }

    public DownMenu downMenu;
    public UnitIconQueue UnitIconQueue;
    Transform HudTextParent;
    private void Awake()
    {
        downMenu = transform.GetComponentInChildren<DownMenu>() ;
        UnitIconQueue = transform.GetComponentInChildren<UnitIconQueue>();
        HudTextParent = transform.Find("HudText");
    }
    public void Test(string msg)
    {
        Debug.Log(msg);
    }
    public void Over(int rank)
    {
        if (rank == Config.playerRanks)
        {
            Instance.Test("over");
        }
        else
        {
            Instance.Test("win");
        }
    }
    public void UpdateHudText(Transform target,int value,HudTextEnum type)
    {
        HudText hud = ObjectPooling.Instance.GetObject<HudText>
            (Manage.Instance.Resources.GetObj(ResourcesEnum.UIPrefab, "HudText"), HudTextParent);
        hud.Reset(target, value, type);

    }
}