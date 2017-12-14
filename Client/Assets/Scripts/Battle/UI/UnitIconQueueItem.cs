

using UnityEngine;
using UnityEngine.UI;

public class UnitIconQueueItem : PoolObj
{

    public Tweener tweener;

    Color[] kuangColor = new Color[] { Color.green, Color.red };
    Image icon;
    Image kuang;
    Unit model;
    HUD hud;
    bool close_Disable;
    public override void Init()
    {
        icon = transform.Find("icon").GetComponent<Image>();
        kuang = transform.Find("kuang").GetComponent<Image>();
        hud = transform.GetComponentInChildren<HUD>();
        hud.Init();
        tweener = GetComponent<Tweener>();
        if (tweener == null) tweener = gameObject.AddComponent<TweenerAlpha>();
        tweener.OnCloseEndEvent = Disable;
    }
    public void OnUpdate(Unit unit)
    {
        model = unit;
        if (unit == null)
        {
            tweener.ToClose();
            return;
        }
        icon.sprite = Manage.Instance.AB.GetGame<Sprite>(AssetbundleEnum.UnitIcon, model.DataModel.dataModel.icon);
        kuang.color = kuangColor[unit.ranks - 1];
        Open();
    }
    public void UpdateValue(float max, float current)
    {
        hud.UpdateValue(max, current);
    }
    public void Close(bool Disable)
    {
        close_Disable = Disable;
        if (Disable) transform.SetParent(transform.parent.parent);
        tweener.OnClose();
    }
    public void Open()
    {
        gameObject.SetActive(true);
        tweener.OnOpen();
    }
    void Disable()
    {
        if (close_Disable)
            gameObject.SetActive(false);
        close_Disable = false;
    }

}