using System.Reflection;
using UnityEngine;
using UnityEditor;
using XNode;

[CreateAssetMenu(fileName ="DialogueGraph")]
public class DialogueGraph : NodeGraph
{
    public string dialogTag;
    [TextArea(5,10)]
    public string dialogInfo;

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
