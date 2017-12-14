using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;


namespace UI
{
    public class SelectedCharacterPanel : Panel
    {
        class RanksItem
        {
            Text nameText;
            GameObject game;
            CharacterAttribute character;
            Action<CharacterAttribute> click;
            public RanksItem(GameObject _game, Action<CharacterAttribute> click)
            {
                character = null;
                this.click = click;
                nameText = _game.transform.Find("name").GetComponent<Text>();
                game = _game.gameObject;
                _game.GetComponent<Button>().onClick.AddListener(OnClick);
            }

            void OnClick()
            {
                click(character);
            }

            public void OnUpdate(CharacterAttribute character )
            {
                this.character = character;
                nameText.text = character.name;
                Open();
            }

            public void Close()
            {
                game.SetActive(false);
            }
            public void Open()
            {
                game.SetActive(true);
            }
        }

        Action<CharacterAttribute> click;
        GameObject prefab;
        Transform grid;
        List<CharacterAttribute> characterAry = new List<CharacterAttribute>();
        List<RanksItem> ItemAry = new List<RanksItem>();
        protected override bool ChangeClose
        {
            get
            {
                return true;
            }
        }
        protected override bool IsOnly
        {
            get
            {
                return false;
            }
        }

        public override void mAwake()
        {
            base.mAwake();
            grid = transform.Find("Mask/Grid");
            prefab = grid.transform.Find("Prefab").gameObject;
            prefab.SetActive(false);
            GetComponent<Button>().onClick.AddListener(()=> {
                if (click != null)
                    click(null);
                Close();
            });
        }
        public void Open(List<CharacterAttribute> ary,Action<CharacterAttribute> click)
        {
            this.click = click;
            characterAry = ary;
            base.Open();
        }

        public override void OnUpdate()
        {
            if (gameObject.activeSelf == false) return;
            int index = characterAry.Count - ItemAry.Count;
            for (int i = 0; i < index; i++)
            {
                NewItem();
            }
            for (index = 0; index < characterAry.Count; index++)
            {
                ItemAry[index].OnUpdate(characterAry[index]);
            }
            for (int i = index; i < ItemAry.Count; i++)
            {
                ItemAry[i].Close();
            }
        }

        void OnClick(CharacterAttribute character)
        {
            if (click != null)
                click(character);
            Close();
        }

        void NewItem()
        {
            GameObject game = Instantiate<GameObject>(prefab);
            game.transform.SetParent(grid,false);
            game.transform.localScale = Vector3.one;
            ItemAry.Add(new RanksItem(game,OnClick));
        }
    }
}
