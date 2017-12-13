

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DownItemStatsPanel : Panel
{
    struct ItemGame
    {
        public GameObject game;
        public Text text;

        public ItemGame(GameObject g)
        {
            game = g;
            text = g.transform.GetComponentInChildren<Text>();
        }
        public void Update(string msg)
        {
            text.text = msg;
            Open();
        }
        public void Open()
        {
            game.SetActive(true);
        }
        public void Close()
        {
            game.SetActive(false);
        }
    }

    Text nameText;
    Transform ItemParent;
    GameObject ItemPrefab;
    List<ItemGame> ItemArray = new List<ItemGame>();
    private void Awake()
    {
        nameText = transform.Find("Title/name").GetComponent<Text>();
        ItemParent = transform.Find("Middle");
        ItemPrefab = ItemParent.Find("Item").gameObject;
        ItemPrefab.SetActive(false);
        gameObject.AddComponent<ClampPanelPoint>();
    }
    public void Open(string name, string[] msg)
    {
       
        nameText.text = name;
        int index = 0;
        for (; index < msg.Length; index++)
        {
            GetItem(index).Update(msg[index]);
        }
        for (; index < ItemArray.Count; index++)
        {
            ItemArray[index].Close();
        }
        base.Open();
    }
    public override void OnUpdate() { }
    ItemGame GetItem(int index)
    {
        if (index >= ItemArray.Count)
        {
            ItemGame item =new ItemGame( ItemPrefab.UIInstantiate(ItemParent));            
            ItemArray.Add(item);
            return item;
        }
        else
        {
            return ItemArray[index];
        }
    }

}