using UnityEngine;
using UnityEngine.UI;


public enum HexCellState
{
    none,
    Null,
    Move,
    MovePath,
    Select,
    Obstacle,
    Attack,
}

public class HexCell : MonoBehaviour
{

    private HexCellState state = HexCellState.none;
    public HexCellState State
    {
        get
        {
            return state;
        }

        set
        {
            state = value;
            SetState();
        }
    }

    [SerializeField]
    HexCell[] neighbors = new HexCell[6];
    public Vector3 GetPoint { get { return transform.position; } }

    public Unit unit;
    public GameObject obstacle;
    public int Id;

    Renderer stateRender;

    private void Awake()
    {
        stateRender = transform.Find("State").GetComponent<Renderer>();
    }
    public int GetValue(Unit unit)
    {
        int value = 10;
        int direction = Mathf.Abs(Mathf.RoundToInt(Vector3.Distance(unit.transform.position, transform.position) / HexGrid.CellDirection));
        value -= direction;
        print(direction);
        return value;
    }
    public void SetUnit(Unit unit)
    {
        if (unit)
        {
            if (unit.cell)
                unit.cell.unit = null;
            unit.cell = this;
            unit.ResetPoint();
            //unit.transform.position = this.transform.position;
        }
        this.unit = unit;
    }
    public HexCell GetNeighbor(int direction)
    {
        return neighbors[direction];
    }
    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }
    public bool UnitEnter(Unit unit)
    {
        return false;
    }
    public void UnitExit()
    {

    }
    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }
    public virtual int Consume()
    {
        return 1;
    }
    void SetState()
    {
        stateRender.material.color = this.GetCellColor();
        stateRender.gameObject.SetActive(state!=HexCellState.none);
    }
}
