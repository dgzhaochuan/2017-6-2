using System;
using UnityEngine;
using UnityEngine.UI;

public class DownMenuSkillItem : MonoBehaviour
{
    public Action<DownMenuSkillItem> OnClick;
    public int data { get; set; }
    /// <summary>
    /// 1 end
    /// 2 atk
    /// 3 skill
    /// </summary>
    public int type { get; set; }
    Text number;
    Image icon;
    GameObject mask;
    Button button;
    SkillAttribute model;

    public SkillAttribute Model
    {
        get
        {
            return model;
        }
    }

    public void Init(Action<DownMenuSkillItem> click)
    {
        OnClick = click;
        mask = transform.Find("mask").gameObject;
        icon = transform.Find("Icon").GetComponent<Image>();
        number = transform.Find("number").GetComponent<Text>();
        button = GetComponent<Button>();
        button.gameObject.AddComponent<LongPress>().Init(LongPress, EndLongPress, Click);
        mask.SetActive(false);
    }

    public void update(Sprite _icon, int data, int number = 1, int maxNumber = 1)
    {
        this.data = data;
        icon.sprite = _icon;
        this.number.text = string.Format("{0}/{1}", number, maxNumber);
        this.number.gameObject.SetActive(maxNumber > 1);
    }
    public void UpdateSkill(SkillAttribute skill, Unit unit)
    {
        type = 3;
        model = skill;
        icon.sprite = Manage.Instance.AB.GetGame<Sprite>(AssetbundleEnum.SkillIcon, skill.icon);
        int needAp = skill.GetAP(unit.DataModel.finalAttribute);
        Interactable(unit.CurrentAP >= needAp);
        number.color = unit.CurrentAP >= needAp ? Color.white : Color.red;
        number.text = needAp.ToString();

        if (skill.IsNeedEquipment(unit.DataModel.dataModel) == false)
        {
            mask.SetActive(true);
            return;
        }
        Open();
    }
    public void Interactable(bool b)
    {
        mask.SetActive(!b);
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    void Click()
    {
        if (mask.activeSelf) return;
        if (OnClick != null) OnClick(this);
    }
    void LongPress()
    {
        string name = "";
        string[] msg = new string[0];
        switch (type)
        {
            case 1:
                name = "结束";
                msg = new string[] {"结束行动,行动点会积累到下回合"};
                break;
            case 2:
                name = "攻击";
                msg = new string[] { "普通攻击" };
                break;
            case 3:
                name = model.name;
                string msg1 = model.GetEffect(Manage.Instance.Battle.CurrentUnit.DataModel.finalAttribute);
                string msg2 = "消耗：" + model.GetAP(Manage.Instance.Battle.CurrentUnit.DataModel.finalAttribute);
                msg = new string[] { msg1, msg2 };
                break;
        }
        PanelManager.Instantiate.GetPanel<DownItemStatsPanel>().Open(name, msg);
    }
    void EndLongPress()
    {
        PanelManager.Instantiate.GetPanel<DownItemStatsPanel>().Close();
    }
}