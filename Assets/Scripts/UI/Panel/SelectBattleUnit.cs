
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SelectBattleUnit : Panel
    {

        class IconItem
        {
            public Action<IconItem> Click;
            public bool IsSelected;
            public CharacterAttribute unit;

            Image kuangImage;
            Image selectedImage;
            Text nameText;
            Image iconImage;
            GameObject game;


            public IconItem(GameObject game,Action<IconItem> Click)
            {
                this.Click = Click;
                this.game = game;
                kuangImage = Find<Image>("kuang");
                selectedImage = Find<Image>("selected");
                iconImage = Find<Image>("icon");
                nameText = Find<Text>("name");
                EventTrigger.Get(game).onDown = OnClick;
            }
            T Find<T>(string name) where T : UnityEngine.Component
            {
                return game.transform.Find(name).GetComponent<T>();
            }
            public void Change(bool selected)
            {
                IsSelected = selected;
                selectedImage.gameObject.SetActive(selected);
                kuangImage.gameObject.SetActive(!selected);
            }
            public void OnClick(GameObject obj)
            {
                Click(this);
            }
            public void SetData(Sprite icon, string name)
            {
                iconImage.sprite = icon;
                nameText.text = name;
            }
            public void SetData(CharacterAttribute unit)
            {
                this.unit = unit;
                
                Sprite sprite = Manage.Instance.AB.GetGame<Sprite>(AssetbundleEnum.UnitIcon, unit.icon);
                string name = unit.name;
                SetData(sprite, name);
                Open();
            }
            public void Open()
            {
                game.SetActive(true);
            }
            public void Close()
            {
                game.SetActive(false);
                unit = null;
            }

        }
        IconItem GetItem
        {
            get
            {
                IconItem item = null;
                if (IdelItem.Count ==0)
                {
                    GameObject game = Instantiate<GameObject>(ItemPrefab);
                    game.transform.SetParent(unitParent,false);
                    item = new IconItem(game,Click);
                }
                else
                {
                    item = IdelItem.Dequeue();
                }
                item.Open();
                item.Change(false);
                return item;
            }
        }
        protected override bool ClickClose
        {
            get
            {
                return true;
            }
        }



        Transform unitParent;
        GameObject ItemPrefab;
        Queue<IconItem> IdelItem = new Queue<IconItem>();
        Queue<IconItem> BriskItem = new Queue<IconItem>();
        List<CharacterAttribute> UnitAry = new List<CharacterAttribute>();
        List<CharacterAttribute> SelectUnit = new List<CharacterAttribute>();
        public Action<List<CharacterAttribute>> EnterEvent; 
        int Count;

        public override void mAwake()
        {
            base.mAwake();
            unitParent = transform.Find("Middle/Grid");
            ItemPrefab = unitParent.Find("Icon").gameObject;
            ItemPrefab.gameObject.SetActive(false);
            transform.Find("Down/Enter").GetComponent<Button>().onClick.AddListener(Enter);
            transform.Find("Down/Exit").GetComponent<Button>().onClick.AddListener(Exit);
        }

        public override void OnUpdate()
        {
            while (BriskItem.Count>0)
            {
                IconItem item = BriskItem.Dequeue();
                item.Close();
                IdelItem.Enqueue(item);
            }
            for (int i = 0; i < UnitAry.Count; i++)
            {
                IconItem item = GetItem;
                item.SetData(UnitAry[i]);
                BriskItem.Enqueue(item);
            }
        }

        void Enter()
        {
            if (EnterEvent != null)
            {
                EnterEvent(SelectUnit);
            }
        }
        void Exit()
        {
            Close();
        }

        public void OnOpen(List<CharacterAttribute> UnitAry,int count, Action<List<CharacterAttribute>> EnterEvent)
        {
            SelectUnit.Clear();
            this.EnterEvent = EnterEvent;
            this.Count = count;
            this.UnitAry = UnitAry;
            base.Open();
        }


        void Click(IconItem item)
        {
            int number = SelectUnit.Count;
            bool selected = !item.IsSelected;
            if (selected)
            {
                number++;
            }
            else
            {
                number--;
            }
            if (number <= Count)
            {
                item.Change(selected);
                if (selected)
                    SelectUnit.Add(item.unit);
                else
                    SelectUnit.Remove(item.unit);
            }
            else
            {
                PanelManager.Instantiate.ErrorPanel.Open("full",3);
            }
        }
    
    }
}
