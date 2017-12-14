
using UI;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SkillStatsPanel : StatsPanel
{
    UnitModel Character;
    Transform StudyNeedParent;
    private SkillAttribute _attribute;
    public SkillAttribute Attribute
    {
        get { return _attribute; }
        set
        {
            _attribute = value;
            OnUpdate();
        }
    }

    public override void mAwake()
    {
        base.mAwake();
        StudyNeedParent = transform.Find("StudyNeed");
        AttributeParent.gameObject.SetActive(false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!gameObject.activeSelf) return;

        SetName(_attribute.name, _attribute.GetEffect(Character.finalAttribute), _attribute.GetDescribe);
        if (_attribute.triggerType == 1)
        {
            SetGame(APParent, _attribute.GetAPText(Character.finalAttribute));
            OnUpdateAttribute(_attribute.NeedAttribute, APParent);
            SetGame(NeedAttributeParent, _attribute.GetCDText(Character.finalAttribute));
            if (_attribute.releaseRange > 1)
                SetGame(NeedAttributeParent, "施法范围:", _attribute.releaseRange);
            if (_attribute.targetRange > 1)
                SetGame(NeedAttributeParent, "有效范围:", _attribute.targetRange);
            if (_attribute.needEquipment.Length > 0)
            {
                string s = "需要";
                switch (_attribute.needEquipment[0])
                {
                    case 1:
                        s += (_attribute.needEquipment[1] == 1 ? "近战" : "远程") + "武器";
                        break;
                    case 2:
                        s += DataName.GetName(((EquipmentEnum)_attribute.needEquipment[1]).ToString(), true);
                        break;
                    case 3:
                        s += DataName.GetName(((weaponEnum)_attribute.needEquipment[1]).ToString(), true);
                        break;
                }
                SetGame(NeedAttributeParent, s);
            }
        }
        else
        {
            SetGame(AttributeParent, "被动技能");
        }

        //技能是否已经学习，没有就需要显示学习条件
        bool have = Character.dataModel.HaveSkill(Attribute);
        StudyNeedParent.gameObject.SetActive(!have);
        if (!have)
        {
            OnUpdateAttribute(_attribute.StudyNeedAttribute, StudyNeedParent);
            SetGame(StudyNeedParent, "等级:", _attribute.needLevel);
        }
    }
    public void Open(SkillAttribute attribute)
    {
        Attribute = attribute;
        base.Open();
    }
    public void Open(SkillAttribute attribute, UnitModel character)
    {
        Character = character;
        Open(attribute);
    }
}
