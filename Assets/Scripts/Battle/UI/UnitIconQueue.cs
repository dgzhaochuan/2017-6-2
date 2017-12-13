

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class UnitIconQueue : MonoBehaviour
{
    public Vector2 StartOff;
    public Vector2 CellSize = Vector2.one * 100;
    public bool AutoCellSize = true;
    public int Spacing;
    public float speed = 4;


    List<UnitIconQueueItem> ItemArray = new List<UnitIconQueueItem>();
    List<RectTransform> MoveRect = new List<RectTransform>();
    GameObject ItemPrefab;
    GameObject SplitGame;
    int to, from;

    void Awake()
    {
        ItemPrefab = transform.Find("Item").gameObject;
        SplitGame = transform.Find("Split").gameObject;
        ItemPrefab.SetActive(false);
    }
    public void SetIconIndex(UnitIconQueueItem item, int index)
    {
        int to = ItemArray.IndexOf(item);
        Insert(to, index);
    }
    public UnitIconQueueItem AddUnitIcon(Unit unit)
    {
        UnitIconQueueItem item = ObjectPooling.Instance.GetObject<UnitIconQueueItem>(ItemPrefab, transform);
        item.OnUpdate(unit);
        ItemArray.Add(item);
        return item;
    }
    public void RemoveUnitIcon(UnitIconQueueItem item)
    {
        int index = ItemArray.IndexOf(item);
        ItemArray.Remove(item);
        for (; index < ItemArray.Count; index++)
        {

        }
        item.Close(true);
        UpdateChild();
    }
    [ContextMenu("UpdateChild")]
    public void UpdateChild()
    {
        int index = 0;
        Vector2 point = Vector2.zero;
        foreach (RectTransform rect in transform)
        {
            rect.anchorMax = rect.anchorMin = new Vector2(0, 1);
            if (AutoCellSize)
                rect.sizeDelta = CellSize;
            Vector2 off = Vector2.zero;
            off.x = rect.pivot.x * rect.sizeDelta.x;
            off.y = -rect.pivot.y * rect.sizeDelta.y;
            rect.anchoredPosition = point + Spacing * index * Vector2.right + StartOff + off;
            point.x += rect.sizeDelta.x;
            index++;
        }
    }
    void Insert(int to, int from)
    {
        if (from == -1) from = ItemArray.Count;
        if (to == from) return;
        this.to = to;
        this.from = from;
        UnitIconQueueItem item = ItemArray[to];
        ItemArray.Insert(from, item);
        MoveRect.Clear();
        for (int i = to; i < from; i++)
        {
            MoveRect.Add(ItemArray[i].GetComponent<RectTransform>());
        }
        ItemArray.RemoveAt(0);
        StartCoroutine(InsertIng(item));
    }
    IEnumerator InsertIng(UnitIconQueueItem item)
    {
        item.Close(false);
        Vector2 disction = CellSize + Vector2.right * Spacing;
        Vector2 off = Vector2.zero;
        while (off.x < disction.x)
        {
            Vector2 value = Vector2.right * speed;
            off += value;
            for (int i = 0; i < MoveRect.Count; i++)
            {
                MoveRect[i].anchoredPosition -= value;
            }
            yield return null;
        }
        item.Open();
        item.transform.SetSiblingIndex(from+1);
        UpdateChild();
    }


    private void OnDrawGizmos()
    {
        //UpdateChild();
    }
}