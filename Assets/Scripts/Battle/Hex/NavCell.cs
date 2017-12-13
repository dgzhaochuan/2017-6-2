

using System.Collections.Generic;

public class NavCell
{
    public int value = 0;
    public HexCell cell;
    public int consume;
    public NavCell parent;

    public NavCell(HexCell cell)
    {
        this.cell = cell;
        this.parent = null;
        this.consume = cell.Consume();
    }
    public NavCell(HexCell cell, NavCell parent)
    {
        this.cell = cell;
        this.parent = parent;
        this.consume = cell.Consume() + parent.consume;
    }
    public void ReviseParent(NavCell parent)
    {
        this.parent = parent;
        if (this.parent == null) consume = 0;
        consume = cell.Consume() + parent.consume;
    }
    public List<NavCell> GetPath()
    {
        NavCell _cell = this;
        List<NavCell> path = new List<global::NavCell>();
        while (_cell != null)
        {
            path.Add(_cell);
            _cell = parent;
        }
        return path;
    }
    public NavCell Copy()
    {
        NavCell cell = new NavCell(this.cell,parent);
        cell.consume = consume;
        return cell;
    }
    public int GetValue(Unit unit)
    {
        value = cell.GetValue(unit);
        value -= consume;
        return value;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        return cell == (obj as NavCell).cell;
    }

    public class StudentComparer : IEqualityComparer<NavCell>
    {
        public bool Equals(NavCell x, NavCell y)
        {
            return x.cell == y.cell;
        }
        public int GetHashCode(NavCell obj)
        {
            int hashStudentId = obj.GetHashCode();
            int hashScore = obj.GetHashCode();
            return hashStudentId ^ hashScore;
        }
    }
}