using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

using UnityEngine.Events;

namespace UI
{
    /// <summary>
    /// 角色属性展示
    /// </summary>
    public class CharacterAttributePanel : Panel
    {
        #region Class

        public class BaseGame
        {
            public GameObject _game;
            protected BaseGame(Transform transform) { _game = transform.gameObject; }
            public void destroy()
            {
                Destroy(_game);
            }
            public void Close()
            {
                _game.SetActive(false);
            }
            public void Open()
            {
                _game.SetActive(true);
            }
        }

        public class AttributeStatsGame : BaseGame
        {
            Text _nameText;
            Text _dataText;
            GameObject _addBtn;
            GameObject _cutBtn;
            public Action<bool, int> BtnEvent;
            int type = 0;

            public AttributeStatsGame(Transform transform, int index) : base(transform)
            {
                _nameText = transform.Find("name").GetComponent<Text>();
                _dataText = transform.Find("data").GetComponent<Text>();
                _addBtn = transform.Find("addBtn/button").gameObject;
                _cutBtn = transform.Find("cutBtn/button").gameObject;
                this.type = index;
                _addBtn.GetComponent<Button>().onClick.AddListener(delegate () { if (BtnEvent != null) { BtnEvent(true, type); } });
                _cutBtn.GetComponent<Button>().onClick.AddListener(delegate () { if (BtnEvent != null) { BtnEvent(false, type); } });
                SetAddBtn(false);
                SetCutBtn(false);
            }
            public void SetName(string name)
            {
                _nameText.text = name;
            }
            public void SetValue(float value, Color color)
            {
                SetValue(value.ToString(),color);
            }
            public void SetValue(string data, Color color)
            {
                _dataText.text = data.ToString();
                _dataText.color = color;
            }
            public void SetAddBtn(bool show) {  _addBtn.SetActive(show); }
            public void SetCutBtn(bool show) { _cutBtn.SetActive(show); }
        }

        #endregion


        Text AttribueCountText;
        Text skillCountText;
        Button SaveAttribute;
        Button SaveSkill;
        UnitModel character;
        public UnitModel Character
        {
            get { return character; }
            set { character = value; OnUpdate(); }
        }
        List<AttributeStatsGame> _dataStats = new List<AttributeStatsGame>();
        List<AttributeStatsGame> _attributeStats = new List<AttributeStatsGame>();
        List<AttributeStatsGame> _skillStats = new List<AttributeStatsGame>();
        List<AttributeStatsGame> _resistanceStats = new List<AttributeStatsGame>();
        PropStatsGame[] _equipmentStats;
        List<PropStatsGame> _propStats = new List<PropStatsGame>();
        Transform _PropParent;
        BaseAttribute AddAttributeModel = new BaseAttribute();
        int[] AddSkillAry = new int[(int)skillEnum.length];
        int AttributeCount;
        int SkillCount;
        
        GameObject StatsGamePrefab
        {
            get
            {
                return Manage.Instance.Resources.GetObj<GameObject>(ResourcesEnum.UIPrefab, Config.PropStatsGame, true);                
            }
        }
        public override void mAwake()
        {
            base.mAwake();
            _dataStats.Clear();
            _attributeStats.Clear();
            _skillStats.Clear();
            _resistanceStats.Clear();

                        
            GameObject prefab = Manage.Instance.Resources.GetObj<GameObject>(ResourcesEnum.UIPrefab,Config.AttributeStatsGame,true);
            StartAttributeStats("attribute", 1, prefab, (int)AttributeEnum.length, ref _attributeStats);
            StartAttributeStats("skill", 2, prefab, (int)skillEnum.length, ref _skillStats);
            //距离和攻击有最大最小
            StartAttributeStats("data", 3, prefab, (int)DataEnum.showLength, ref _dataStats);
            StartAttributeStats("resistance", 4, prefab, (int)ResistanceEnum.length, ref _resistanceStats);
      
            
            Transform EquipmentParent = transform.Find("Left/Equipment/Grid");
            _equipmentStats = new PropStatsGame[EquipmentParent.childCount];
            for (int i = 0; i < _equipmentStats.Length; i++)
            {
                PropStatsGame p = NetStatsGame(EquipmentParent);
                p.gameObject.name = ((EquipmentEnum)i).ToString();
                p.OnInit(OnClickEquipment, i);
                _equipmentStats[i] = p;
            }
            _PropParent = transform.Find("Left/Prop/Grid");


        }

        void StartAttributeStats(string parent, int keyType,GameObject prefab, int leng, ref List<AttributeStatsGame> ary)
        {
            ary.Clear();
            Transform p = transform.Find("Right/Attribute/Mask/" + parent + "/panel");
            for (int index = 0; index < leng; index++)
            {
                GameObject _game = Instantiate<GameObject>(prefab);               
                _game.transform.SetParent(p,false);
                _game.transform.localScale = Vector3.one;
                AttributeStatsGame game = new AttributeStatsGame(_game.transform, index);
                string _name = "";
                switch (keyType)
                {
                    case 1:
                        AttribueCountText = p.Find("Count/name").GetComponent<Text>();
                        SaveAttribute = p.Find("Count/Save").GetComponent<Button>();
                        SaveAttribute.onClick.AddListener(SaveAddAttribute);
                        _name = DataName.GetAttributeName(index,false);
                        game.BtnEvent = OnClickAttributeBtn;
                        break;
                    case 2:
                        skillCountText = p.Find("Count/name").GetComponent<Text>();
                        SaveSkill = p.Find("Count/Save").GetComponent<Button>();
                        SaveSkill.onClick.AddListener(SaveAddSkill);
                        _name = DataName.GetSkillName(index, false);
                        game.BtnEvent = OnClickSkillBtn;
                        break;
                    case 3:
                        _name = DataName.GetDataName(index, false);
                        break;
                    case 4:
                        _name = DataName.GetResistanceName(index, false);
                        break;
                }
                game.SetName(_name);
                ary.Add(game);
            }
        }
        PropStatsGame NetStatsGame(Transform parent)
        {
            GameObject game = Instantiate<GameObject>(StatsGamePrefab);
            game.transform.SetParent(parent, false);
            game.transform.localScale = Vector3.one;
            return game.AddComponent<PropStatsGame>();
        }
        void OnUpdatePropCount()
        {
            int index = 0;
            int off = Character.dataModel.propCount - _propStats.Count;
            if (off > 0)
            {
                GameObject prefab = Manage.Instance.Resources.GetObj<GameObject>(ResourcesEnum.UIPrefab, Config.PropStatsGame, true);
                for (index = 0; index < off; index++)
                {
                    PropStatsGame p = NetStatsGame(_PropParent);
                    p.OnInit(OnClickProp, index);
                    _propStats.Add(p);
                }
            }
            index = 0;
            for (int i = 0; i < Character.dataModel.propCount; i++,index++)
            {
                
                BaseAdditionalAttribute _att = Manage.Instance.Data.GetBaseProp(Character.dataModel.prop[i]);
                _propStats[i].Attribute = _att;
            }
            for (int i = index; i < _propStats.Count; i++)
            {
                _propStats[i].gameObject.SetActive(false);
            }
        }
        void OnUpdateEquipment()
        {
            for (int i = 0; i < character.dataModel.equipment.Length; i++)
            {
                EquipmentAttribute _att =
                    Manage.Instance.Data.GetObj<EquipmentAttribute>(character.dataModel.equipment[i]);
                _equipmentStats[i].Attribute = _att;
            }
            int index = _equipmentStats.Length - character.dataModel.weapon.Length;
            for (int i = 0; i < character.dataModel.weapon.Length; i++, index++)
            {
                EquipmentAttribute _att =
                    Manage.Instance.Data.GetObj<EquipmentAttribute>(character.dataModel.weapon[i]);
                _equipmentStats[index].Attribute = _att;
            }
        }

        void OnClickEquipment(BaseAdditionalAttribute eq,int type)
        {
            if (eq == null) return;
            PanelManager.Instantiate.PropStatsPanel.Open(eq, PropStatsPanel.OpenType.character,character.dataModel,-1);
        }

        void OnClickProp(BaseAdditionalAttribute prop,int index)
        {
            if (prop == null) return;
            PanelManager.Instantiate.PropStatsPanel.Open(prop,PropStatsPanel.OpenType.characterBackpack,Character.dataModel,index);

        }

        void OnClickAttributeBtn(bool add, int type)
        {
            if (add)
            {
                AttributeCount--;
                AddAttributeModel[(AttributeEnum)type]++;
            }
            else
            {
                AttributeCount++;
                AddAttributeModel[(AttributeEnum)type]--;
            }
            AddAttributeModel.Update();
            AddAttributeModel.OnUpdateAttribute();
            OnUpddateAddAttribute();
        }
        void OnClickSkillBtn(bool add, int type)
        {
            if (add)
            {
                SkillCount--;
                AddSkillAry[type]++;
            }
            else
            {
                SkillCount++;
                AddSkillAry[type]--;
            }
            OnUpdateAddSkill();
        }

        void SaveAddAttribute()
        {
            character.dataModel.userAttributeCount += character.dataModel.attributeCount - AttributeCount;
            character.dataModel.baseAttribute += AddAttributeModel;
            character.dataModel.attributeCount = AttributeCount;
            AddAttributeModel.Clear();
            OnUpddateAddAttribute();
            SaveSprite.SetCharacterAttribute(character.dataModel);
        }

        void OnUpddateAddAttribute()
        {
            AttribueCountText.text = AttributeCount.ToString();
            bool b = false;
            for (int i = 0; i < (int)AttributeEnum.length; i++)
            {
                _attributeStats[i].SetAddBtn(AttributeCount > 0);
                _attributeStats[i].SetCutBtn(AddAttributeModel[(AttributeEnum)i] > 0);
                if (AddAttributeModel[(AttributeEnum)i] > 0 && b==false)
                {
                    b = true;
                }
            }
            
            SaveAttribute.gameObject.SetActive(b);            
            OnUpdateData();
        }        

        void SaveAddSkill()
        {
            character.dataModel.userSkillCount += character.dataModel.skillCount - SkillCount;
            character.dataModel.baseAttribute.AddSkill(AddSkillAry);
            character.dataModel.skillCount = SkillCount;
            OnUpdateAddSkill();
            SaveSprite.SetCharacterAttribute(character.dataModel);
        }

        void OnUpdateAddSkill()
        {
            skillCountText.text = SkillCount.ToString();
            bool b = false;
            for (int i = 0; i < _skillStats.Count; i++)
            {
                _skillStats[i].SetAddBtn(SkillCount > 0);
                _skillStats[i].SetCutBtn(AddSkillAry[i] > 0);
                if (AddSkillAry[i] > 0 && b == false)
                {
                    b = true;
                }
            }
            SaveSkill.gameObject.SetActive(b);
        }

        void OnUpdateData()
        {
            character.OnUpdate();
            for (int i = 0; i < _attributeStats.Count; i++)
            {
                _attributeStats[i].SetValue(character[(AttributeEnum)i] + AddAttributeModel[(AttributeEnum)i], character.GetColor(1, i));
            }
            for (int i = 0; i < _skillStats.Count; i++)
            {
                _skillStats[i].SetValue(character[(skillEnum)i] + AddSkillAry[i], character.GetColor(2, i));
            }
            for (int i = 0; i < _dataStats.Count; i++)
            {
                _dataStats[i].SetValue(character[(DataEnum)i] + AddAttributeModel[(DataEnum)i], character.GetColor(3, i));
            }
            for (int i = 0; i < _resistanceStats.Count; i++)
            {
                _resistanceStats[i].SetValue(character[(ResistanceEnum)i], character.GetColor(4, i));
            }

            int _atk = character[DataEnum.Atk] + AddAttributeModel[DataEnum.Atk];
            int _maxatk= character[DataEnum.maxAtk] + AddAttributeModel[DataEnum.maxAtk];

            _dataStats[(int)(DataEnum.Atk)].SetValue(_atk + "-" + _maxatk, character.GetColor(3, (int)(DataEnum.Atk)));

            _dataStats[(int)(DataEnum.minatkRange)].SetValue(
                (character[DataEnum.minatkRange] + AddAttributeModel[DataEnum.minatkRange])
                + "-" +
                (character[DataEnum.maxatkRange] + AddAttributeModel[DataEnum.maxatkRange])
                , character.GetColor(3, (int)(DataEnum.minatkRange)));

        }

       

        public override void OnUpdate()
        {
            if (gameObject.activeSelf == false) return;
            if (character == null)
            {
                //Debug.LogError("Attribute is null");
                return;
            }

            //Left
            OnUpdatePropCount();
            OnUpdateEquipment();



            //Right
            character.OnUpdate();
            AddAttributeModel.Clear();
            AddSkillAry = new int[(int)skillEnum.length];

            AttributeCount = character.dataModel.attributeCount;
            AttribueCountText.text = AttributeCount.ToString();
            SkillCount = character.dataModel.skillCount;
            skillCountText.text = SkillCount.ToString();

            OnUpddateAddAttribute();
            OnUpdateAddSkill();

        }
    }
    
}
