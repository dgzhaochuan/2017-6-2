
using BattleState;
using System;
using UnityEngine;


public enum GameState
{
    play,
    pause,
    noInput,
    moveing,
}

public class BattleManager : BaseManaget
{
    public Unit CurrentUnit
    {
        get
        {
            return currentUnit;
        }

        set
        {
            currentUnit = value;
            BattlePanel.Instance.downMenu.UpdateCharacter(currentUnit);
        }
    }
    private Unit currentUnit;
    public NavCell currentNavCell;
    public HexCell currentHexcell;
    public UnitManager Unitmanager;
    public SceneModel currentScene;
    static public GameState gameState;
    static public void ChangeState(GameState state)
    {
        gameState = state;
    }
    public static HexCell SelectedCell()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit, 100, (1 << Config.HexLayer) | (1 << Config.UnitLayer)))
        {
            if (hit.collider.GetComponent<Unit>())
                return hit.collider.GetComponent<Unit>().cell;
            return hit.collider.GetComponent<HexCell>();
        }
        return null;
    }

    State _currentState;
     bool _inTransition;
     GameObject SeleTarget;

    public override void OnInit()
    {
        HexGrid.Instantiate.mAwake();
        SeleTarget = GameObject.Find("SeleTarget");
        SeleTarget.SetActive(false);
    }
    public void mStart()
    {
        Unitmanager = InitManager<UnitManager>();
        ChangeState<BattleState.InItBattleState>(null);
    }
    T InitManager<T>() where T : Component
    {
        T t = GameObject.FindObjectOfType<T>();
        if (t == null)
        {
            t = new GameObject(typeof(T).Name).AddComponent<T>();
        }
        return t;
    }
    public void UnitDie(Unit unit)
    {
        BattlePanel.Instance.UnitIconQueue.RemoveUnitIcon(unit.hud);
        Unitmanager.RemoveUnit(unit);
        Manage.Instance.Battle.Unitmanager.IsOver(unit.ranks);

    }
    public void ClearCurrent()
    {
        CurrentUnit = null;
        currentNavCell = null;
        currentHexcell = null;
    }
    public  State CurrentState
    {
        get { return _currentState; }
        set { _currentState = (value); }
    }
    public  T GetState<T>() where T : State
    {
        T target = GetComponent<T>();
        if (target == null)
            target = gameObject.AddComponent<T>();
        return target;
    }
    public  void ChangeState<T>() where T : State
    {
        ChangeState<T>(new object[0]);
    }
    public  void ChangeState<T>(object obj) where T : State
    {
        ChangeState<T>(new object[] { obj });
    }
    public  void ChangeState<T>(object obj1, object obj2) where T : State
    {
        ChangeState<T>(new object[] { obj1, obj2 });
    }
    public  void ChangeState<T>(object obj1, object obj2, object obj3) where T : State
    {
        ChangeState<T>(new object[] { obj1, obj2, obj3 });
    }
    public  void ChangeState<T>(object[] obj) where T : State
    {
        Transition(GetState<T>(), obj);
    }
    protected  void Transition(State value, object[] obj)
    {
        if ((_currentState is SelectUnitBattleState) == false)
            if (_currentState == value) return;
        if (_currentState != null)
            _currentState.Exit();
        _currentState = value;
        if (_currentState != null)
            _currentState.Enter(obj);
    }

    public void SetSeleTarget(HexCell cell)
    {
        if (cell == null)
        {
            CloseSeleTarget();
            return;
        }
        SeleTarget.transform.position = cell.GetPoint + Vector3.up * .3f;
        SeleTarget.SetActive(true);
    }
    public void CloseSeleTarget()
    {
        SeleTarget.SetActive(false);
    }

}