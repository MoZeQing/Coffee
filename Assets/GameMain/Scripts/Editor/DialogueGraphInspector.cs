using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameMain;
using Dialog;

[CustomEditor(typeof(DialogueGraph))]
public class DialogueGraphInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("ת��Ϊ�ı�����"))
        {
            DialogueGraph dialogueGraph = serializedObject.targetObject as DialogueGraph;
            string path = EditorUtility.OpenFolderPanel("���Ŀ¼", "C://", "");
            string fileName = dialogueGraph.name;
            XNodeSerializeHelper helper1 = new XNodeSerializeHelper();
            CSVSerializeHelper helper2 = new CSVSerializeHelper();
            DialogData dialogData = helper1.Serialize(dialogueGraph);
            helper2.Deserialize(dialogData, path, fileName);
        }
    }
}