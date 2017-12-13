using System;

using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// 背包
    /// </summary>
    public class BackPackPanel : Panel
    {
        SaveSprite.SaveModel Model { get { return SaveSprite.Model; } }
        PropStatsGame[] StatsGameAry = new PropStatsGame[Config.BackPackCount];
        Text moneyText;
        
        public override void mAwake()
        {
            base.mAwake();
            transform.Find("Upper/close").GetComponent<Button>().onClick.AddListener(Close);
            Transform parent = transform.Find("Middle/Grid");
            GameObject prefab = Manage.Instance.Resources.GetObj<GameObject>(ResourcesEnum.UIPrefab, Config.PropStatsGame, true);
            StatsGameAry = new PropStatsGame[Config.BackPackCount];
            for (int i = 0; i < StatsGameAry.Length; i++)
            {
                PropStatsGame game = Instantiate<GameObject>(prefab).AddComponent<PropStatsGame>();
                game.transform.SetParent(parent,false);
                game.transform.localScale = Vector3.one;
                game.OnInit(ClickPropGame,i);
                StatsGameAry[i] = game;
            }

            moneyText = transform.Find("Lower/money").GetComponent<Text>();
            transform.Find("Lower/Arrangement").GetComponent<Button>().onClick.AddListener(Arrangement);
        }
        void Arrangement()
        {
            GameNumber[] Prop = new GameNumber[Model.Prop.Length];
            int index = 0;
            for (int i = 0; i < Model.Prop.Length; i++)
            {
                if (Model.Prop[i].IsNull) continue;
                Prop[index].SetData(Model.Prop[i]);
                index ++;
            }
            Model.Prop = Prop;
            OnUpdate();
            SaveSprite.Write();
        }
        void ClickPropGame(BaseAdditionalAttribute prop,int index)
        {
            PanelManager.Instantiate.PropStatsPanel.Open(prop, PropStatsPanel.OpenType.backpack,null,index);
        }
        void UpdateStatsGame(PropStatsGame statsGame, GameNumber prop)
        {
            BaseAdditionalAttribute attribute = Manage.Instance.Data.GetBaseProp(prop);
            if (attribute != null)
                attribute.number = prop.number;
            statsGame.Attribute = attribute;
        }
        public override void OnUpdate()
        {
            if (gameObject.activeSelf == false) return;
            moneyText.text = Model.money.ToString();
            for (int i = 0; i < StatsGameAry.Length; i++)
            {
                UpdateStatsGame(StatsGameAry[i],Model.Prop[i]);
            }
        }
    }
}
