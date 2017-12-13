using UnityEngine;
using System.Collections;

using UI;
using System.Collections.Generic;

public class UITest : MonoBehaviour
{
    public GUISkin skin;

    void Start()
    {
        Manage.Instance.mAwake();
    }
    private void OnGUI()
    {
        index = -1;
        if (GetButton("Character"))
        {
            List<UnitModel> modelary = new List<UnitModel>();
            foreach (var item in SaveSprite.CharacterAry.Values)
            {
                UnitModel model = new UnitModel();
                model.dataModel = item;
                modelary.Add(model);
            }
            PanelManager.Instantiate.Character.CharacterAry = modelary;
            PanelManager.Instantiate.Character.Change();
        }
        if (GetButton("Error"))
        {
            if (PanelManager.Instantiate.ErrorPanel.Tweener.IsOpen == false)
                PanelManager.Instantiate.ErrorPanel.Open("message");
            else
                PanelManager.Instantiate.ErrorPanel.Close();
        }
        if (GetButton("BackPack"))
        {
            PanelManager.Instantiate.BackPack.Change();
        }
        if (GetButton("Shop"))
        {
            PanelManager.Instantiate.ShopPanel.Change();
        }
        if (GetButton("SelectUnit"))
        {
            if (PanelManager.Instantiate.SelectBattleUnit.Tweener.IsOpen == false)
            {
                List<CharacterAttribute> ary = Manage.Instance.Data.GetObjAry<CharacterAttribute>();
                PanelManager.Instantiate.SelectBattleUnit.OnOpen(ary, 1, delegate (List<CharacterAttribute> unitAry) { });
            }
            else
                PanelManager.Instantiate.SelectedCharacterPanel.Close();
        }
    }


    int index = -1;
    int buttonWidth = 200;
    int off = 5;
    int buttonHeight = 80;
    Rect GetRect
    {
        get
        {
            index++;
            return new Rect(10, 10 + (buttonHeight + off) * index, buttonWidth, buttonHeight);
        }
    }

    bool GetButton(string msg)
    {
        return GUI.Button(GetRect, msg, skin.button);
    }

}
