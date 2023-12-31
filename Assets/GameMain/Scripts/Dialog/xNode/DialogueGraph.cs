using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using System;
using OfficeOpenXml;//Epplus

[CreateAssetMenu(fileName ="DialogueGraph")]
public class DialogueGraph : NodeGraph
{
    [TextArea(5,10)]
    public string dialogInfo;

    public override Node AddNode(Type type)
    {
        return base.AddNode(type);
    }

    public bool Check()
    {
        foreach (Node node in nodes)
        {
            if (node.GetType().ToString() == "StartNode")
            {
                return true;
            }
        }
        return false;
    }

    public void Init()
    {
        if (this.nodes.Count == 0)
        {
            StartNode startNode = AddNode(typeof(StartNode)) as StartNode;
            ChatNode chatNode = AddNode(typeof(ChatNode)) as ChatNode;
            startNode.name = "Start";
            chatNode.name = "Chat";
            startNode.position = Vector2.zero;
            chatNode.position = Vector2.zero;
            AssetDatabase.AddObjectToAsset(startNode, this);
            AssetDatabase.AddObjectToAsset(chatNode, this);
            List<ChatData> chatDatas = new List<ChatData>();
            ChatData chatData = new ChatData();
            chatData.charName = "����";
            chatData.text = string.Format("���ԣ���Դ��{0}", this.name);
            chatDatas.Add(chatData);
            chatNode.chatDatas = chatDatas;
            startNode.GetOutputPort("start").Connect(chatNode.GetInputPort("a"));
            AssetDatabase.SaveAssets();
        }
    }

    public Node GetStartNode()
    {
        foreach (Node node in nodes)
        {
            if (node.GetType().ToString() == "StartNode")
            {
                return node;
            }
        }
        return null;
    }

    [MenuItem("���뵼������/�Ի��ļ�����",false,1000)]
    public static void SOToExcel()
    {
        Debug.Log(0);
    }

    [MenuItem("���뵼���ļ�/�Ի��ļ�ת��",false,1001)]
    public static void ExcelToSO()
    {
        Debug.Log(0);
    }

    [MenuItem("Data/Dialog/������")]
    public static void DialogCheck()
    {
        DialogueGraph[] graphs = Resources.LoadAll<DialogueGraph>("DialogData");
        foreach (DialogueGraph graph in graphs)
        {
            if (!graph.Check())
                Debug.LogErrorFormat("������StartNode�ĶԻ����飬����{0}", graph.name);
        }
    }
}
