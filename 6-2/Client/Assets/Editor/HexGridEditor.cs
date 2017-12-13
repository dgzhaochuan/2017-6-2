using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(HexGrid))]
public class HexGridEditor : Editor
{
   
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HexGrid hexGrid = target as HexGrid;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Awake"))
        {
            hexGrid.mAwake();
        }
        if (GUILayout.Button("Destory"))
        {
            hexGrid.destory();
        }
        if (GUILayout.Button("UpdateObstacle"))
        {
            hexGrid.UpdateObstacle();
        }
        if (GUILayout.Button("ClearState"))
        {
            hexGrid.ClearState();
        }
        GUILayout.EndHorizontal();
    }

}
