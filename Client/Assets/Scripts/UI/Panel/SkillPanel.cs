using UnityEngine;
using System.Collections;
using System;

using System.Collections.Generic;
using UnityEngine.UI;

namespace UI
{
    public class SkillPanel : Panel
    {
        UnitModel character;
        public UnitModel Character
        {
            set
            {
                character = value;
                OnUpdate();
            }
        }
        GameObject _SkillStatsGame;
        GameObject SkillStatsGamePrefab
        {
            get
            {
                if (_SkillStatsGame == null)
                    _SkillStatsGame =
                        Manage.Instance.Resources.GetObj(ResourcesEnum.UIPrefab, Config.SkillStatsGame, false);
                return _SkillStatsGame;
            }
        }


        Transform skillGrid;
        List<SkillStatsGame> statsGame = new List<SkillStatsGame>();
        Text[] TabButtonLabel;
        Toggle[] TabToggle;
        public override void mAwake()
        {
            base.mAwake();
            StartLeft();
            StartRight();
        }

        public override void OnUpdate()
        {
            if (IsOpen == false) return;
            if (character == null)
            {
                Debug.LogError("Attribute is null");
                return;
            }
            OnUpdateTabBtnLabel();
            OnClick(true, "0");
        }

        void OnUpdateTabBtnLabel()
        {
            for (int i = 0; i < TabButtonLabel.Length; i++)
            {
                string s = "";
                //s = DataName.GetSkillName(i, true);
                //if (character.dataModel.skillId[i].Count > 0)
                s += "(" + character.dataModel.skillArray[i].Count.ToString() + ")";
                TabButtonLabel[i].text = s;
            }
        }
        void OnClick(bool ison, string name)
        {
            if (ison == false) return;
            int type = int.Parse(name);
            if (TabToggle[type].isOn == false) TabToggle[type].isOn=true;
            List<SkillAttribute> skill = new List<SkillAttribute>();
            for (int i = 0; i < character.dataModel.skillArray[type].skillAry.Count; i++)
            {
                int skillid = character.dataModel.skillArray[type].skillAry[i];
                SkillAttribute _skill = Manage.Instance.Data.GetObj<SkillAttribute>(skillid);
                skill.Add(_skill);
            }

            int count = skill.Count - statsGame.Count;
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(SkillStatsGamePrefab);
                obj.SetParent(skillGrid);
                SkillStatsGame game = obj.AddComponent<SkillStatsGame>();
                game.mAwake(OnClickSkillStats);
                statsGame.Add(game);
            }
            for (count = 0; count < skill.Count; count++)
            {
                statsGame[count].OnUpdate(skill[count]);
            }
            for (; count < statsGame.Count; count++)
            {
                statsGame[count].Close();
            }

        }
        void OnClickSkillStats(SkillAttribute model)
        {
            PanelManager.Instantiate.SkillStatsPanel.Open(model, character);
        }
        void StartLeft()
        {

            GameObject prefab = transform.Find("Down/Left/Mask/Toggle").gameObject;
            Transform parent = transform.Find("Down/Left/Mask");
            TabButtonLabel = new Text[(int)skillEnum.length];
            TabToggle = new Toggle[(int)skillEnum.length];
            for (int i = 0; i < (int)skillEnum.length; i++)
            {
                GameObject game = prefab.UIInstantiate(parent, i.ToString());
                TabButtonLabel[i] = game.transform.Find("Label").GetComponent<Text>();
                TabToggle[i] = game.GetComponent<Toggle>();
                TabToggle[i].onValueChanged.AddListener(delegate (bool b) { OnClick(b, game.name); });
            }
            Destroy(prefab);
        }
        void StartRight()
        {
            skillGrid = transform.Find("Down/Right/Middle/mask/Grid");
        }
    }
}
