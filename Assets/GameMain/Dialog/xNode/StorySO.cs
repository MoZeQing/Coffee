using GameFramework.DataTable;
using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class StorySO : ScriptableObject
{
    [SerializeField]
    public bool unLoad;
    [SerializeField]
    public bool isRemove;
    [SerializeField]
    public OutingSceneState outingSceneState;
    [SerializeField]
    public GameState gameState;
    [SerializeField]
    public ParentTrigger trigger;
    [SerializeField]
    public DialogueGraph dialogueGraph;
    [SerializeField]
    public List<EventData> eventDatas= new List<EventData>();
    [SerializeField,TextArea]
    public string content;

    //[MenuItem("Data/StoryToCSV")]
    public static void StoryToCSV()
    {
        try
        {
            StorySO[] storySOs = Resources.LoadAll<StorySO>("StoryData");
            StreamWriter sw = new StreamWriter(new FileStream(Application.dataPath + "/Config/mainStory.csv", FileMode.OpenOrCreate), Encoding.GetEncoding("UTF-8"));
            sw.WriteLine("���±�ǩ,��������,����ʱ��,�����¼�,����������");
            foreach (StorySO story in storySOs)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(story.name + ",");
                sb.Append(story.dialogueGraph.name + ",");
                sb.Append(story.gameState + ",");
                if (story.eventDatas.Count != 0)
                {
                    foreach (EventData eventData in story.eventDatas)
                    {
                        sb.Append(eventData.eventTag.ToString());
                        sb.Append(" ");
                    }
                }
                else
                {
                    sb.Append("NULL");
                }
                sb.Append("," + story.trigger.TriggerToString());
                sw.WriteLine(sb.ToString());
            }
            sw.Close();
            Debug.Log("mainStory������");
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    //[MenuItem("Data/StoryCheck")]
    public static void Check()
    { 
        
    }

    public static StringBuilder TriggerToString(Trigger trigger,StringBuilder sb,int sort)
    {
        if (trigger == null)
            return sb;
        sb.Append('\t',sort);
        if (trigger.not)
        {
            if (trigger.equals)
                sb.Append(trigger.key.ToString() + "������" + trigger.value.ToString());
            else
                sb.Append(trigger.key.ToString() + "С��" + trigger.value.ToString());
        }
        else
        {
            if (trigger.equals)
                sb.Append(trigger.key.ToString() + "����" + trigger.value.ToString());
            else
                sb.Append(trigger.key.ToString() + "����" + trigger.value.ToString());
        }
        sb.Append('\n');
        if (trigger.GetAndTrigger().Count != 0)
        {
            sb.Append("���µ�Ҫ��ȫ������:\n");
            foreach (Trigger tr in trigger.GetAndTrigger())
            {
                TriggerToString(tr, sb,sort+1);
            }
        }
        if (trigger.GetOrTrigger().Count != 0)
        {
            sb.Append("����һ������Ҫ��:\n");
            foreach (Trigger tr in trigger.GetAndTrigger())
            {
                TriggerToString(tr, sb, sort + 1);
            }
        }
        return sb;
    }
}
