using UnityEngine;
using System.Collections.Generic;

public class HudManager : MonoBehaviour
{
    GameObject hudPrefab;
    Queue<HUD> idelHud = new Queue<HUD>(); 

    private void Awake()
    {
        hudPrefab = Manage.Instance.Resources.GetObj(ResourcesEnum.UIPrefab, "HUD");
    }

    public HUD GetHud()
    {
        HUD hud = null;
        if (idelHud.Count > 0) hud = idelHud.Dequeue();
        else
            hud = hudPrefab.UIInstantiate(transform).AddComponent<HUD>();
        hud.Open();
        return hud;
    }
    public void ResetHud(HUD hud)
    {
        hud.Close();
        idelHud.Enqueue(hud);
    }

   
}