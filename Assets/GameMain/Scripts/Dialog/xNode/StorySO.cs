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
    public bool isRemove;
    [SerializeField]
    public OutingSceneState outingSceneState;
    [SerializeField]
    public bool outingBefore;
    [SerializeField]
    public TimeTag timeTag;
    [SerializeField]
    public ParentTrigger trigger;
    [SerializeField]
    public DialogueGraph dialogueGraph;
    [SerializeField]
    public List<EventData> eventDatas= new List<EventData>();

#if UNITY_EDITOR
    [MenuItem("Data/Story/���������ΪCSV")]
    public static void MainStoryToCSV()
    {
        try
        {
            //StorySO[] storySOs = Resources.LoadAll<StorySO>("StoryData");
            //StringBuilder storiesTag = new StringBuilder();
            //StringBuilder triggers = new StringBuilder();
            //StringBuilder dialogues = new StringBuilder();
            //StringBuilder events = new StringBuilder();
            //foreach (StorySO story in storySOs)
            //{
            //    storiesTag.Append(story.name);
            //    storiesTag.Append(",");
            //    triggers.Append("����");
            //    triggers.Append(",");
            //    dialogues.Append(story.dialogueGraph.name);
            //    dialogues.Append(",");
            //    foreach (EventData eventData in story.eventDatas)
            //    {
            //        events.Append(eventData.eventTag.ToString() + " = " + eventData.value.ToString());
            //        events.Append(",");
            //    }
            //    events.Append(",");
            //}
            //storiesTag.Remove(storiesTag.Length - 1, 1);
            //triggers.Remove(triggers.Length - 1, 1);
            //dialogues.Remove(dialogues.Length - 1, 1);
            //events.Remove(events.Length - 1, 1);
            //StreamWriter sw = new StreamWriter(new FileStream(Application.dataPath + "/mainStory.csv", FileMode.OpenOrCreate), Encoding.GetEncoding("GB2312"));
            //sw.WriteLine(storiesTag.ToString());
            //sw.WriteLine(triggers.ToString());
            //sw.WriteLine(dialogues.ToString());
            //sw.WriteLine(events.ToString());
            //sw.Close();
            //Debug.Log("mainStory������");

            StorySO[] storySOs = Resources.LoadAll<StorySO>("StoryData");
            StreamWriter sw = new StreamWriter(new FileStream(Application.dataPath + "/mainStory.txt", FileMode.OpenOrCreate), Encoding.GetEncoding("UTF-8"));
            foreach (StorySO story in storySOs)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("StoryTag��" + story.name + ",");
                sb.Append("����������" + story.dialogueGraph.name + ",");
                sb.Append("�����¼���");
                foreach (EventData eventData in story.eventDatas)
                {
                    sb.Append(eventData.eventTag.ToString() + " = " + eventData.value.ToString());
                    sb.Append(" ");
                }
                sw.WriteLine(sb.ToString());
                sw.WriteLine("����������\n" + TriggerToString(story.trigger, new StringBuilder(), 0));
            }
            sw.Close();
            Debug.Log("mainStory������");
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
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
#endif
}
