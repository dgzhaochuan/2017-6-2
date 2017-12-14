using UnityEngine;
using UnityEditor;

    public class TweenerEditor : Editor
    {

        float Slider;
        bool preview;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Tweener tweener = target as Tweener;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("SetOpen"))
            {
                tweener.SetOpen();
            }
            if (GUILayout.Button("SetClose"))
            {
                tweener.SetClose();
            }
            if (GUILayout.Button("ToOpen"))
            {
                tweener.ToOpen();
            }
            if (GUILayout.Button("ToClose"))
            {
                tweener.ToClose();
            }
            GUILayout.EndHorizontal();
            preview = EditorGUILayout.Toggle("preview", preview);
            if (preview)
            {
                Slider = GUILayout.HorizontalSlider(Slider, 0, 1);
                tweener.UpdateSlider(Slider);
            }
        }
    }
