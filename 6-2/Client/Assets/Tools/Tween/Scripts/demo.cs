

using UnityEngine;

public class demo : MonoBehaviour
{

    public Tweener tween;

    void OnGUI()
    {
        if (GUILayout.Button("  to  ", GUILayout.Width(100), GUILayout.Height(50)))
        {
            tween.OnOpen();
        }

        if (GUILayout.Button("  from  ", GUILayout.Width(100), GUILayout.Height(50)))
        {
            tween.OnClose();
        }

        if (GUILayout.Button("  pause  ", GUILayout.Width(100), GUILayout.Height(50)))
        {
            tween.OnPause();
        }
    }


}