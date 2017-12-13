
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UI
{
    public class ShopPanel : Panel
    {
        PropStatsGame[] StatsGameAry = new PropStatsGame[Config.BackPackCount];
        BaseAdditionalAttribute[] addition;
        public override void mAwake()
        {
            base.mAwake();
            addition = new BaseAdditionalAttribute
                [
                Manage.Instance.Data.GetDictionary<PropAttribute>().Count +
                Manage.Instance.Data.GetDictionary<EquipmentAttribute>().Count
                ];
            int index = 0;
            for (int i = 0; i < Manage.Instance.Data.GetDictionary<PropAttribute>().Count; i++)
            {
                addition[index] = Manage.Instance.Data.GetDictionary<PropAttribute>().Values.ToList()[i]as BaseAdditionalAttribute;
                addition[index].number = index + 1;
                index++;
            }
            for (int i = 0; i < Manage.Instance.Data.GetDictionary<EquipmentAttribute>().Count; i++)
            {
                addition[index] = Manage.Instance.Data.GetDictionary<EquipmentAttribute>().Values.ToList()[i] as BaseAdditionalAttribute;
                addition[index].number = 1;
                index++;
            }



            StatsGameAry = new PropStatsGame[addition.Length];
            Transform parent = transform.Find("Middle/Grid");
            GameObject prefab = Manage.Instance.Resources.GetObj<GameObject>(ResourcesEnum.UIPrefab, Config.PropStatsGame, true);
            for (int i = 0; i < StatsGameAry.Length; i++)
            {
                PropStatsGame game = Instantiate<GameObject>(prefab).AddComponent<PropStatsGame>();
                game.transform.SetParent(parent, false);
                game.transform.localScale = Vector3.one;
                game.OnInit(ClickPropGame,i);
                StatsGameAry[i] = game;
            }
        }
        void ClickPropGame(BaseAdditionalAttribute prop,int index)
        {
            PanelManager.Instantiate.PropStatsPanel.Open(prop, PropStatsPanel.OpenType.shop, null,index);
        }
        void UpdateStatsGame(PropStatsGame statsGame, GameNumber prop)
        {
            BaseAdditionalAttribute attribute = null;
            switch ((AdditionalAttributeEnum)prop.PropType)
            {
                case AdditionalAttributeEnum.equipment:
                    attribute =
                    Manage.Instance.Data.GetObj<EquipmentAttribute>(prop.id);
                    break;
                case AdditionalAttributeEnum.prop:
                    attribute =
                    Manage.Instance.Data.GetObj<PropAttribute>(prop.id);
                    break;
            }
            if (attribute != null)
                attribute.number = prop.number;
            statsGame.Attribute = attribute;
        }
        public override void OnUpdate()
        {
            if (gameObject.activeSelf == false) return;
            for (int i = 0; i < StatsGameAry.Length; i++)
            {
                UpdateStatsGame(StatsGameAry[i],new GameNumber(addition[i]));
            }
        }
    }
}
