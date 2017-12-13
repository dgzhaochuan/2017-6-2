
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UI
{
    public abstract class StatsPanel : Panel
    {

        Text NameText;
        Text DescribeText;
        Text EffectText;
        protected Transform AttributeParent;
        protected Transform APParent;
        protected Transform NeedAttributeParent;


        GameObject AttributeGamePrefab
        {
            get
            {
                return Manage.Instance.Resources.GetObj<GameObject>(
                    ResourcesEnum.UIPrefab,
                    Config.EquipemtAttributeGame, true);
            }
        }
        Queue<EquipemtAttributeGame> IdelGame = new Queue<EquipemtAttributeGame>();
        Queue<EquipemtAttributeGame> BriskGame = new Queue<EquipemtAttributeGame>();
        
        string GetAttributeString(float value)
        {
            string s = value.ToString();
            if (value > 0) s = "+" + s;
            else
                if (value < 0) s = "-" + s;
            return s;
        }

        protected void OnUpdateAttributeGame(AdditionalAttribute baseAttribute)
        {
            float _atk = 0;
            float _maxAtk = 0;
            for (int index = 0; index < baseAttribute.data.Count; index++)
            {
                if (baseAttribute.data[index].Type == (int)DataEnum.Atk)
                {
                    _atk = baseAttribute.data[index].GetValue();
                    continue;
                }else
                     if (baseAttribute.data[index].Type == (int)DataEnum.maxAtk)
                {
                    _maxAtk = baseAttribute.data[index].GetValue();
                    continue;
                }
                SetGame(AttributeParent, DataName.GetDataName(baseAttribute.data[index].Type, true), GetAttributeString(baseAttribute.data[index].value));
            }
            if (_atk != 0 || _maxAtk != 0)
            {
                string str = string.Format("{0}-{1}", _atk, _maxAtk);
                SetGame(AttributeParent, DataName.GetDataName((int)DataEnum.Atk, true), str);
            }
            for (int index = 0; index < baseAttribute.attribute.Count; index++)
            {
                SetGame(AttributeParent, DataName.GetAttributeName(baseAttribute.attribute[index].Type, true), GetAttributeString(baseAttribute.attribute[index].value));
            }
            for (int index = 0; index < baseAttribute.resistance.Count; index++)
            {
                SetGame(AttributeParent, DataName.GetResistanceName(baseAttribute.resistance[index].Type, true), GetAttributeString(baseAttribute.resistance[index].value));
            }
            for (int index = 0; index < baseAttribute.skill.Count; index++)
            {
                SetGame(AttributeParent, DataName.GetSkillName(baseAttribute.skill[index].Type, true), GetAttributeString(baseAttribute.skill[index].value));
            }
        }
        
        protected void OnUpdateNeedAttribute(List<AdditionalModel> needAttribute)
        {
            for (int i = 0; i < needAttribute.Count; i++)
            {
                string   str = "需要" + DataName.Get_Name(needAttribute[i].Type, needAttribute[i].EnumType, true);
                float data = needAttribute[i].value;
                SetGame(NeedAttributeParent,str,data);
            }
        }
        protected void OnUpdateAttribute(List<AdditionalModel> needAttribute,Transform parent)
        {
            for (int i = 0; i < needAttribute.Count; i++)
            {
                string str =DataName.Get_Name(needAttribute[i].Type, needAttribute[i].EnumType, true)+":";
                float data = needAttribute[i].value;
                SetGame(parent, str, data);
            }
        }
        protected void OnUpdateNeedAttribute(AdditionalAttribute needAttribute)
        {
            for (int index = 0; index < needAttribute.data.Count; index++)
            {
                SetGame(NeedAttributeParent,"需要"+DataName.GetDataName(needAttribute.data[index].Type, true), needAttribute.data[index].value.ToString());
            }
            for (int index = 0; index < needAttribute.attribute.Count; index++)
            {
                SetGame(NeedAttributeParent, "需要" + DataName.GetAttributeName(needAttribute.attribute[index].Type, true), needAttribute.attribute[index].value.ToString());
            }
            for (int index = 0; index < needAttribute.resistance.Count; index++)
            {
                SetGame(NeedAttributeParent, "需要" + DataName.GetResistanceName(needAttribute.resistance[index].Type, true), needAttribute.resistance[index].value.ToString());
            }
            for (int index = 0; index < needAttribute.skill.Count; index++)
            {
                SetGame(NeedAttributeParent, "需要" + DataName.GetSkillName(needAttribute.skill[index].Type, true), needAttribute.skill[index].value.ToString());
            }
        }
        protected void SetName(string name,string effect, string describe)
        {
            NameText.text = name;
            DescribeText.text = describe;
            EffectText.text = effect;
        }

        protected void SetGame(Transform parent, string str, float data)
        {
            if (data <= 0) return;
            SetGame(parent, str + data.ToString());
        }
        protected void SetGame(Transform parent, string data)
        {
            EquipemtAttributeGame game = GetAttributeGame(parent);
            game.OnUpdate(data);
            BriskGame.Enqueue(game);
        }
        void SetGame(Transform parent, string name, string value)
        {
            EquipemtAttributeGame game = GetAttributeGame(parent);
            game.OnUpdate(name, value);
            BriskGame.Enqueue(game);
        }
        void SetGame(Transform parent, string name, string value, bool up)
        {
            EquipemtAttributeGame game = GetAttributeGame(parent);
            game.OnUpdate(name, value, up);
            BriskGame.Enqueue(game);
        }
        

        public override void mAwake()
        {
            base.mAwake();
            if (transform.Find("name/close"))
                transform.Find("name/close").GetComponent<Button>().onClick.AddListener(() => { Close(); });
            NameText = transform.Find("name/Text").GetComponent<Text>();
            DescribeText = transform.Find("Describe/Describe").GetComponent<Text>();
            EffectText= transform.Find("effect/effect").GetComponent<Text>();
            AttributeParent = transform.Find("Attribute");
            APParent = transform.Find("AP");
            NeedAttributeParent = transform.Find("NeedAttribute");
            
        }
        public override void OnUpdate()
        {
            while (BriskGame.Count > 0)
            {
                EquipemtAttributeGame game = BriskGame.Dequeue();
                game.Close();
                IdelGame.Enqueue(game);
            }
            AttributeParent.gameObject.SetActive(false);
            APParent.gameObject.SetActive(false) ;
            NeedAttributeParent.gameObject.SetActive(false);
        }


        EquipemtAttributeGame GetAttributeGame(Transform parent)
        {
            EquipemtAttributeGame con = null;
            if (IdelGame.Count > 0)
            {
                con = IdelGame.Dequeue();
            }
            else
            {
                GameObject game = Instantiate<GameObject>(AttributeGamePrefab);
                game.transform.localScale = Vector3.one;
                con = game.AddComponent<EquipemtAttributeGame>();
                con.mAwake();
            }
            if (parent.gameObject.activeSelf == false) parent.gameObject.SetActive(true);
            con.transform.SetParent(parent, false);
            return con;
        }
    }
}
