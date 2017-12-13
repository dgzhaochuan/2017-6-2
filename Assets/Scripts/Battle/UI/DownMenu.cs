using BattleState;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownMenu : MonoBehaviour
{


    List<DownMenuSkillItem> SkillItem = new List<DownMenuSkillItem>();
    Transform GridParent;
    Transform ApParent;
    GameObject skillItemPrefab;
    DownMenuSkillItem EndItem;
    DownMenuSkillItem AtkItem;
    GameObject apPrefab;
    List<DownAPItem> ApItem = new List<DownAPItem>();

    private void Awake()
    {
        GridParent = transform.Find("Grid/Grid");
        ApParent = transform.Find("ApGrid").transform;
        skillItemPrefab = GridParent.Find("Item").gameObject;
        apPrefab = ApParent.Find("Item").gameObject;
        AwakeMenu();
        skillItemPrefab.SetActive(false);
        apPrefab.SetActive(false);
    }
    private void AwakeMenu()
    {
        EndItem = NewSkillItem();
        AtkItem = NewSkillItem();
        AtkItem.transform.SetAsFirstSibling();
        EndItem.type = 1;
        AtkItem.type = 2;
        EndItem.name = "End";
        AtkItem.name = "Atk";       
    }
    public void UpdateCharacter(Unit unit)
    {
        if (unit == null
            || Manage.Instance.Battle.Unitmanager.IsPlayerRanks(unit.ranks)==false
            || unit.DataModel.dataModel.skillArray==null
            )
        {
            Close();
            return;
        }
        UpdateEquipment(unit);
        UpdateAp(unit,0);
        UpdateSkill(unit);
        Open();
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void UpdateAp(Unit unit,int user)
    {
        int count = 0;
        for (; count < unit.DataModel[DataEnum.maxAP]; count++)
        {
            int type = 3;
            if (count < unit.CurrentAP - user)
            {
                type = 1;
            }else if(count < unit.CurrentAP&& count >= unit.CurrentAP - user)
            {
                type = 2;
            }
            GetApItem(count).upddate(type);
        }
        for (count--; count < SkillItem.Count; count++)
        {
            ApItem[count].upddate(0);
        }
    }
    DownAPItem GetApItem(int count)
    {
        if (count >= ApItem.Count)
        {
            GameObject game = Instantiate(apPrefab);
            var item = game.AddComponent<DownAPItem>();
            item.transform.UIReset(ApParent);
            item.Init();
            ApItem.Add(item);
            EndItem.transform.SetAsLastSibling();
            return item;
        }
        else
        {
            return ApItem[count];
        }
    }
    void UpdateEquipment(Unit unit)
    {
        var eq = Manage.Instance.Data.GetObj<EquipmentAttribute>(unit.DataModel.dataModel.weapon[0]);
        if (eq != null)
        {
            Sprite sprite = Manage.Instance.AB.GetGame<Sprite>(AssetbundleEnum.EquipmentIcon, eq.icon);
            AtkItem.Interactable(true);
            AtkItem.update(sprite, eq.UseAP);
            AtkItem.Interactable(unit.CurrentAP >= eq.UseAP);
        }
        else
        {
            AtkItem.Interactable(false);
        }
    }
    void UpdateSkill(Unit unit)
    {
        int count = 0;
        for (int i = 0; i < unit.DataModel.dataModel.skillArray.Length; i++)
        {
            for (int j = 0; j < unit.DataModel.dataModel.skillArray[i].Count; j++)
            {
                SkillAttribute skill = Manage.Instance.Data.GetObj<SkillAttribute>(unit.DataModel.dataModel.skillArray[i].skillAry[j]);
                if (skill.triggerType != 1) continue;
                GetSkillItem(count).UpdateSkill(skill, unit);
                count++;
            }
        }
        for (count--; count < SkillItem.Count; count++)
        {
            SkillItem[count].Close();
        }
    }
    DownMenuSkillItem GetSkillItem(int count)
    {
        if (count >= SkillItem.Count)
        {
            var item = NewSkillItem();
            SkillItem.Add(item);
            EndItem.transform.SetAsLastSibling();
            return item;
        }
        else
        {           
            return SkillItem[count];
        }
    }
    DownMenuSkillItem NewSkillItem()
    {
        GameObject game = Instantiate<GameObject>(skillItemPrefab);
        var item = game.AddComponent<DownMenuSkillItem>();
        item.transform.UIReset(GridParent);
        item.Init(ClickSkillItem);
        return item;
    }
    void ClickSkillItem(DownMenuSkillItem item)
    {
        if (BattleManager.gameState == GameState.moveing) return;
        switch (item.type)
        {
            case 1:
                EndClick(item);
                break;
            case 2:
                AtkClick(item);
                break;
        }

    }
    void EndClick(DownMenuSkillItem item)
    {
        if (BattleManager.gameState == GameState.moveing) return;
        Manage.Instance.Battle.CurrentUnit.NoActive();
        Manage.Instance.Battle.Unitmanager.NoActive();
        Manage.Instance.Battle.ChangeState<SelectUnitBattleState>();
    }
    void AtkClick(DownMenuSkillItem item)
    {
        //TODO
        UpdateAp(Manage.Instance.Battle.CurrentUnit, item.data);
        Manage.Instance.Battle.ChangeState<EquiAtkState>();

    }
}
