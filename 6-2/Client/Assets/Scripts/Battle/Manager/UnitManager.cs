

using UnityEngine;
using System.Collections.Generic;
using System;


public struct Round
{
    public System.Action action;
    public int round;
    public int startRound;

    public Round(int startRound, int round, System.Action action)
    {
        this.startRound = startRound;
        this.round = round;
        this.action = action;
        UnitManager.UpdateRound -= Update;
        UnitManager.UpdateRound += Update;
    }
    void Update(int round)
    {
        if (round - startRound >=this.round)
        {
            if (action != null) action();
            Close();
        }
    }
    public void Close()
    {
        UnitManager.UpdateRound -= Update;
    }
}
public class UnitManager:MonoBehaviour
{

    public static event Action<int> UpdateRound;

    List<Unit> units;
    public Unit unitPrefab;

    int round;
    int currentUnit;

    public void Init(int id)
    {
        SceneModel scene = Manage.Instance.Data.GetObj<SceneModel>(id);
        units = new List<Unit>();
        InItRanks(1, scene.ranks1);
        InItRanks(2, scene.ranks2);
        units.Sort((a, b) => b.DataModel[DataEnum.move].CompareTo(a.DataModel[DataEnum.move]));
        Manage.Instance.Battle.currentScene = scene;
        for (int i = 0; i < units.Count; i++)
        {
            units[i].StartHud();
        }
        BattlePanel.Instance.UnitIconQueue.UpdateChild();
        currentUnit = 0;
        round = 0;

        new Round(0, 3, delegate () { print("3 Round end"); });
    }
    void InItRanks(int id, List<SceneModel.UnitModel> ranks)
    {
        Transform parent = new GameObject("Ranks"+id).transform;
        HUD hudPrefab = Manage.Instance.Resources.GetObj<HUD>(ResourcesEnum.UIPrefab, Config.HUD,false);
        for (int i = 0; i < ranks.Count; i++)
        {
            Unit _unit = Instantiate(unitPrefab);
            UnitModel model = new UnitModel();
            model.dataModel = Manage.Instance.Data.GetObj<CharacterAttribute>(ranks[i].id);
            _unit.Init(model, id);
            _unit.transform.SetParent(parent,false);
            _unit.gameObject.layer = Config.UnitLayer;
            HexCell _cell = HexGrid.Instantiate.GetCell(ranks[i].cell);
            _cell.SetUnit(_unit);
            units.Add(_unit);
            _unit.name = model.dataModel.id.ToString();
        }
    }    
    public void IsOver(int ranks)
    {
        var unit = units.Find(a => a.ranks == ranks && a.IsDie == false);
        if (unit == null)
        {
            BattlePanel .Instance.Over(ranks);
        }
    }
    public bool IsPlayerRanks(int ranks)
    {
        return ranks == Config.playerRanks;
    }
    public Unit GetMaxValue(int rank, List<Unit> UnitArray=null)
    {
        if (UnitArray == null) UnitArray = units;
        if (UnitArray.Count == 1) return UnitArray[0];
        Unit unit = UnitArray[0];
        int value = unit.GetValue();
        for (int i = 1; i < UnitArray.Count; i++)
        {
            if (UnitArray[i].ranks == rank) continue;
            bool change = false;
            if (unit == null)
            {
                change = true;
            }
            else
                if (UnitArray[i].GetValue() > value)
            {
                change = true;
            }
            if (change)
            {
                unit = UnitArray[i];
                value = unit.GetValue();
            }
        }
        return unit;
    }
    public Unit GetMaxValue(Unit unit)
    {
        Unit target_unit = null;
        int value = 0;
        for (int i = 0; i < units.Count; i++)
        {
            bool change = false;
            if (target_unit == null)
            {
                change = true;
            }
            else
                if (units[i].GetValue() > value)
            {
                change = true;
            }
            if (change)
            {
                target_unit = units[i];
                value = target_unit.GetValue();
            }
        }
        return target_unit;
    }
    public void AddUnit(Unit unit)
    {
        int index=0;
        for (; index < units.Count; index++)
        {
            if (units[index].DataModel[DataEnum.move] < unit.DataModel[DataEnum.move]) break;
        }
        units.Insert(index,unit);
        unit.StartHud();
        BattlePanel.Instance.UnitIconQueue.SetIconIndex(unit.hud, index);
    }
    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
    }
    public void NoActive()
    {
        var _unit = units[0];
        units.Add(_unit);
        units.RemoveAt(0);
        units[0].Reset();     
        BattlePanel.Instance.UnitIconQueue.SetIconIndex(_unit.hud, -1);
        currentUnit++;
        if (currentUnit >= units.Count)
        {
            currentUnit = 0;
            round++;
            if (UpdateRound != null)
            {
                UpdateRound(round);
            }
        }
    }
    public Unit NextUnit()
    {
        return units[0];
    }
    public List<Unit> GetEnemy(int rank)
    {
        List<Unit> targets = new List<Unit>();
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].ranks != rank) targets.Add(units[i]);
        }
        return targets;
    }
    public List<Unit> GetTeammate(int rank)
    {
        List<Unit> targets = new List<Unit>();
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].ranks == rank) targets.Add(units[i]);
        }
        return targets;
    }
    public List<Unit> GetAllUnit(int rank)
    {
        return units;
    }

}