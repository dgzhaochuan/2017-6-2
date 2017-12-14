using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using System;

public class SkillStatsGame : MonoBehaviour
{
    Action<SkillAttribute> OnClick;

    Image iconImage;
    Text nameText;

    SkillAttribute model;
    public void mAwake(Action<SkillAttribute> OnClick)
    {
        this.OnClick = OnClick;
        iconImage = transform.Find("icon").GetComponent<Image>();
        nameText = transform.Find("describe/name").GetComponent<Text>();
        transform.Find("describe/button/use").GetComponent<Button>().onClick.AddListener(Use);
        transform.Find("describe/button/Forget").GetComponent<Button>().onClick.AddListener(Forget);
        GetComponent<Button>().onClick.AddListener(Click);
    }

    void Click()
    {
        if (OnClick != null) OnClick(model);
    }

    void Use()
    {
        print("Use");
    }
    void Forget()
    {
        print("Forget");
    }
    public void OnUpdate(SkillAttribute model)
    {
        Open();
        this.model = model;
        iconImage.sprite = Manage.Instance.AB.GetGame<Sprite>(AssetbundleEnum.SkillIcon,model.icon);
        nameText.text = model.name;
    }
    public void Open()
    {
        if (gameObject.activeSelf == false) gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }

}
