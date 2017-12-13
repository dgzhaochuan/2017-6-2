using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public int value;
    public HudTextEnum type;
    public Transform target;
    private void OnGUI()
    {
        if (GUILayout.Button("111111111", GUILayout.MinHeight(50)))
        {
            BattlePanel.Instance.UpdateHudText(target,value,type);
        }
    }


}
