

using System;
using System.Collections.Generic;
using UnityEngine;
using UI;


public class PanelManager : BaseManaget
{
    static private PanelManager _instance;
    static public PanelManager Instantiate
    {
        get
        {
            if (_instance == null)
            {

                GameObject ca = null;
                //ca = FindObjectOfType<Canvas>().gameObject;
                if (ca == null)
                {
                    ca = Manage.Instance.Resources.GetObj<GameObject>(ResourcesEnum.UIPrefab, "Canvas", false);
                    ca.GetComponent<Canvas>().sortingOrder = 1;
                }
                _instance = Instantiate<GameObject>(ca).AddComponent<PanelManager>();
            }
            return _instance;
        }
    }

    Dictionary<string, Panel> PanelAry = new Dictionary<string, Panel>();

    public CharacterPanel Character
    {
        get
        {
            return GetPanel<CharacterPanel>();
        }
    }
    public BackPackPanel BackPack { get { return GetPanel<BackPackPanel>(); } }
    public PropStatsPanel PropStatsPanel { get { return GetPanel<PropStatsPanel>(); } }
    public SelectedCharacterPanel SelectedCharacterPanel
    { get { return GetPanel<SelectedCharacterPanel>(); } }
    public ErrorPanel ErrorPanel { get { return GetPanel<ErrorPanel>(); } }
    public ShopPanel ShopPanel { get { return GetPanel<ShopPanel>(); } }
    public InputNumberPanel InputnumberPanel { get { return GetPanel<InputNumberPanel>(); } }
    public SkillStatsPanel SkillStatsPanel { get { return GetPanel<SkillStatsPanel>(); } }
    public SelectBattleUnit SelectBattleUnit { get { return GetPanel<SelectBattleUnit>(); } }



    public T GetPanel<T>() where T : Panel
    {
        string key = typeof(T).Name;
        T panel = null;
        if (PanelAry.ContainsKey(key) == false)
        {
            panel = transform.NewPanel<T>(Manage.Instance.Resources.GetObj<GameObject>(ResourcesEnum.UIPanel, key, false));
            PanelAry.Add(key, panel);
        }
        else
        {
            panel = PanelAry[key] as T;
        }
        return panel;
    }

    List<Panel> ChangeClosePanel = new List<Panel>();
    public void AddChageClosePanel(Panel panel)
    {
        if (ChangeClosePanel.Contains(panel) == false)
            ChangeClosePanel.Add(panel);
    }
    public void ChangePanel()
    {
        for (int i = 0; i < ChangeClosePanel.Count; i++)
        {
            if (ChangeClosePanel[i].Tweener.IsOpen)
                ChangeClosePanel[i].Close();
        }
    }


    public override void OnInit() { }
    public override void Close()
    {
        PanelAry.Clear();
    }

}


