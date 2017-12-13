using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

using System.Collections.Generic;

namespace UI
{
    /// <summary>
    /// 角色界面
    /// </summary>
    public class CharacterPanel : Panel
    {
        UnitModel attribute { get { return CharacterAry[CharacterIndex]; } }
        public List<UnitModel> CharacterAry = new List<UnitModel>();
        int CharacterIndex;


        CharacterAttributePanel attributePanel;
        SkillPanel skillPanel;
        Image HeadImage;
        Text NameText;
        List<Toggle> tabToggle=new List<Toggle>();
        public override void mAwake()
        {
            base.mAwake();
            transform.Find("Title/close").GetComponent<Button>().onClick.AddListener(Close);
            transform.Find("Title/name/left").GetComponent<Button>().onClick.AddListener(LeftCharacter);
            transform.Find("Title/name/right").GetComponent<Button>().onClick.AddListener(RightCharacter);
            HeadImage = transform.Find("Title/icon").GetComponent<Image>();
            NameText = transform.Find("Title/name/name").GetComponent<Text>();

            attributePanel = transform.Find("Down/AttributePanel").gameObject.AddComponent<CharacterAttributePanel>();
            skillPanel = transform.Find("Down/SkillPanel").gameObject.AddComponent<SkillPanel>();
            attributePanel.mAwake();
            skillPanel.mAwake();
            foreach (Transform item in transform.Find("Middle"))
            {
                tabToggle.Add(item.GetComponent<Toggle>());
            }
            tabToggle[0].onValueChanged.AddListener(OpenAttributePanel);
            tabToggle[1].onValueChanged.AddListener(OpenSkillPanel);
        }

        public override void Open(float delay = 0)
        {
            CharacterIndex = 0;
            OpenAttributePanel(true);
            base.Open(delay);
        }
        public override void OnUpdate()
        {
            if (!gameObject.activeSelf) return;
            NameText.text = attribute.dataModel.name;
            attributePanel.Character = attribute;
            //TODO 
            {
                attributePanel.Character.dataModel.skillArray = new global::SkillSaveModel[(int)skillEnum.length];
                for (int i = 0; i < attributePanel.Character.dataModel.skillArray.Length; i++)
                {
                    attributePanel.Character.dataModel.skillArray[i] = new SkillSaveModel(i);
                }
                var _array = Manage.Instance.Data.GetObjAry<SkillAttribute>();
                for (int i = 0; i < _array.Count; i++)
                {
                    SkillAttribute _skill = _array[i];
                    attributePanel.Character.dataModel.skillArray[(int)_skill.skillType].skillAry.Add(_skill.id);
                }
            }
            skillPanel.Character = attribute;
        }
        void ResetToggle(int index)
        {
            for (int i = 0; i < tabToggle.Count; i++)
            {
                tabToggle[i].isOn = i==index;
            }
        }
        void OpenAttributePanel(bool b)
        {
            if (!b) return;
            attributePanel.Open();
            skillPanel.Close();
            ResetToggle(0);
        }
        void OpenSkillPanel(bool b)
        {
            if (!b) return;
            attributePanel.Close();
            skillPanel.Character = attribute;
            skillPanel.Open();
            ResetToggle(1);
        }
        void LeftCharacter() { ChangeCharacter(-1); }
        void RightCharacter() { ChangeCharacter(1); }
        void ChangeCharacter(int value)
        {
            CharacterIndex += value;
            if (CharacterIndex < 0)
                CharacterIndex = CharacterAry.Count - 1;
            else
                if (CharacterIndex >= CharacterAry.Count)
                CharacterIndex = 0;
            OnUpdate();
        }
     
    }
}