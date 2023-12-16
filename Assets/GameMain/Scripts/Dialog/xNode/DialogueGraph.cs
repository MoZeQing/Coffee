using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using System;

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

#if UNITY_EDITOR
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
#endif
}
