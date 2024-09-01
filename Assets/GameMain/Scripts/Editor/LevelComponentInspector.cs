using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelComponent))]
public class LevelComponentInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if (Application.isPlaying)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 18;
            style.normal.textColor = Color.white;

            LevelComponent levelComponent = (LevelComponent)target;
            GUILayout.Label($"�Ѿ����ص�Level����:{levelComponent.GetLoadedLevelCount}", style);
            GUILayout.Label($"ȫ����Level����:{levelComponent.GetAllLevelCount}", style);
        }
    }
}