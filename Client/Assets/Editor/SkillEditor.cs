using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillEditor : EditorWindow
{

    [MenuItem("Tools/SkillWindow")]
    static void window()
    {
        //创建窗口
        Rect wr = new Rect(0, 0, 500, 500);
        SkillEditor window = (SkillEditor)EditorWindow.GetWindow(typeof(SkillEditor));
        window.Show();
    }
}
