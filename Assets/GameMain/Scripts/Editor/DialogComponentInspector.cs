using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogComponent))]
public class DialogComponentInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if (Application.isPlaying)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize= 18;
            style.normal.textColor = Color.white;

            DialogComponent dialogComponent= (DialogComponent)target;
            GUILayout.Label($"�Ѿ����ص�Dialog����:{dialogComponent.GetLoadedDialogCount}", style);
            GUILayout.Label($"�Ѿ����ص�Story����:{dialogComponent.GetLoadedStoryCount}", style);
            GUILayout.Label($"ȫ����Story����:{dialogComponent.GetAllStoryCount}", style);
        }
    }
}
