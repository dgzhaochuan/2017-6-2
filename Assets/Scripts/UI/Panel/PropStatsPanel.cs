using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using System;

namespace UI
{
    /// <summary>
    /// 道具展示
    /// </summary>
    public class PropStatsPanel : StatsPanel
    {
        public enum OpenType
        {
            backpack,
            character,
            characterBackpack,
            shop,
        }
        Text moneyText;
        Text weightText;
        List<GameObject> buttonAty = new List<GameObject>();
        CharacterAttribute character;
        int PropIndex = -1;
        BaseAdditionalAttribute attribute;
        OpenType openType;
        Action CurrentFunction;
        int CurrentNumber;
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
            moneyText = transform.Find("money/money/Text").GetComponent<Text>();
            weightText = transform.Find("money/weight/Text").GetComponent<Text>();
            buttonAty = new List<GameObject>();
            foreach (Transform item in transform.Find("button"))
            {
                buttonAty.Add(item.gameObject);
                EventTrigger.Get(item.gameObject).onClick = (GameObject game) => { OnClick(game.name); };
            }
        }
        /// <summary>
        /// 0 交给
        /// 1 使用
        /// 2 装备
        /// 3 卸下
        /// 4 放回
        /// 5 出售
        /// 6 丢弃
        /// 7 购买
        /// </summary>
        /// <param name="index"></param>
        void OpenButton(int index)
        {
            buttonAty[index].SetActive(true);
        }
        void OnUpdateButton()
        {
            switch (openType)
            {
                case OpenType.backpack:
                    OpenButton(0);
                    OpenButton(5);
                    OpenButton(6);
                    break;
                case OpenType.character:
                    OpenButton(3);
                    break;
                case OpenType.characterBackpack:
                    if (attribute is EquipmentAttribute)
                        OpenButton(2);
                    else
                        OpenButton(1);
                    OpenButton(4);
                    break;
                case OpenType.shop:
                    //buttonAty[7].GetComponent<Button>().interactable =
                    //    attribute.money* attribute.number <= SaveSprite.Model.money;
                    OpenButton(7);
                    break;
            }
        }
        #region Click

        List<CharacterAttribute> GetCharacterAry()
        {
            List<CharacterAttribute> ary = new List<CharacterAttribute>();
            ary.Add(Manage.Instance.Data.GetObj<CharacterAttribute>(1));
            return ary;
        }
        void OnClick(string name)
        {
            CurrentNumber = 1;
            switch (name)
            {
                case "User":
                    SetNumber(UserFunction, false);
                    break;
                case "Get":
                    SetNumber(GetFunction, false,false);
                    break;
                case "equipment":
                    EquipmentFunction();
                    Close();
                    break;
                case "Unload":
                    UnloadFunction();
                    Close();
                    break;
                case "Return":
                    SetNumber(ReturnFunction, false);
                    break;
                case "sell":
                    SetNumber(SellFunction, false);
                    break;
                case "Discard":
                    SetNumber(DiscardFunction, false);
                    break;
                case "Purchase":
                    SetNumber(PurchaseFunction, false);
                    break;
            }
            PanelManager.Instantiate.Character.OnUpdate();
            PanelManager.Instantiate.BackPack.OnUpdate();
        }
        void SetNumber(Action func,bool IsPurchase,bool close=true)
        {
            CurrentFunction = func;
            if (attribute.number > 1)
                PanelManager.Instantiate.InputnumberPanel.Open
                    (SetAttributeNumber, attribute.number, IsPurchase ? attribute.money : -1);
            else
                SetAttributeNumber(attribute.number);
            if (close)
                Close();
        }
        void SetAttributeNumber(int number)
        {
            CurrentNumber = number;
            CurrentFunction();
            PanelManager.Instantiate.BackPack.OnUpdate();
        }
        void GetFunction()
        {
            //交给
            List<CharacterAttribute> ary = SaveSprite.GetCharacterAttributeAry();
            PanelManager.Instantiate.SelectedCharacterPanel.Open(ary, delegate (CharacterAttribute Character)
            {
                if (Character == null) return;
                GameNumber game = new GameNumber(this.attribute);
                if (Character.AddProp(game) == false)
                {
                    PanelManager.Instantiate.ErrorPanel.Open("full");
                }
                else
                {
                    SaveSprite.RemoveIndexProp(this.attribute,PropIndex,CurrentNumber);
                    PanelManager.Instantiate.Character.OnUpdate();
                    PanelManager.Instantiate.BackPack.OnUpdate();
                    Close();
                }
            });
        }
        void UserFunction()
        {
            //使用 默认一个
            Debug.LogError("user prop");
            character.RemoveProp(CurrentNumber, PropIndex);
        }
        void EquipmentFunction()
        {
            //装备
            if (character.UpdateWeapon(attribute as EquipmentAttribute)==false)
            {
                PanelManager.Instantiate.ErrorPanel.Open("full");
                return;
            }
            character.RemoveProp(CurrentNumber, PropIndex);
            SaveSprite.Write();
        }
        void UnloadFunction()
        {
            //卸下
            if (character.NotEquipment(attribute as EquipmentAttribute) == false)
                PanelManager.Instantiate.ErrorPanel.Open("full");
            SaveSprite.Write();
        }
        void ReturnFunction()
        {
            //放回
            if (SaveSprite.BackPackIsNull(1) == false) return;
            for (int i = 0; i < character.prop.Length; i++)
            {
                if(character.prop[i].id== attribute.id)
                {
                    if (SaveSprite.AddProp(attribute, CurrentNumber))
                    {
                        character.prop[i].SetNull();
                    }else
                    {
                        PanelManager.Instantiate.ErrorPanel.Open("full");
                    }
                    break;
                }
            }
        }
        void SellFunction()
        {
            //出售
            SaveSprite.AddMoney(attribute.money * Config.sell);
            SaveSprite.RemoveIndexProp(this.attribute, PropIndex, CurrentNumber);
        }
        void DiscardFunction()
        {
            //丢弃
            SaveSprite.RemoveIndexProp(this.attribute, PropIndex, CurrentNumber);
        }
        void PurchaseFunction()
        {
            if (attribute.number == 0) return;
            //购买
            int money = attribute.money * attribute.number;
            if (SaveSprite.Model.money < money)
            {
                PanelManager.Instantiate.ErrorPanel.Open("钱不够");
                return;
            }
            SaveSprite.AddMoney(money * -1);
            SaveSprite.AddProp(attribute, CurrentNumber);
            PanelManager.Instantiate.BackPack.OnUpdate();
        }
        
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prp"></param>
        /// <param name="type"></param>
        /// <param name="character"></param>
        /// <param name="PropIndex">在角色背包里面的位置</param>
        public void Open(BaseAdditionalAttribute prp, OpenType type,CharacterAttribute character,int PropIndex)
        {
            this.character = character;
            this.openType = type;
            this.attribute = prp;
            this.PropIndex = PropIndex;
            base.Open();
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (gameObject.activeSelf == false) return;
            for (int i = 0; i < buttonAty.Count; i++)
            {
                buttonAty[i].SetActive(false);
            }
            moneyText.text = attribute.money.ToString();
            weightText.text = attribute.weight.ToString();

            SetName(attribute.name, attribute.GetEffect(character == null ? null : character.baseAttribute),
                    attribute.GetDescribe);
            SetGame(APParent, "装备:", attribute.AP);
            SetGame(APParent, "使用:", attribute.UseAP);
            SetGame(NeedAttributeParent, "使用范围:", attribute.releaseRange);
            SetGame(NeedAttributeParent, "作用范围:", attribute.targetRange);
            OnUpdateAttributeGame(attribute.baseAttribute);
            OnUpdateNeedAttribute(attribute.needAttribute);

            OnUpdateButton();
        }
        public override void Close(float delay = 0)
        {
            CurrentNumber = 0;
            CurrentFunction = null;
            base.Close(delay);
        }
    }
}
