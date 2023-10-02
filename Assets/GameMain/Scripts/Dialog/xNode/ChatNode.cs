using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using GameMain;
using System.IO;
using UnityEditor;
using ExcelDataReader;

[NodeWidth(400)]
public class ChatNode : Node
{

    [Input] public float a;

    public string dialogId;

    [SerializeField,Output(dynamicPortList = true)]
    public List<ChatData> chatDatas= new List<ChatData>();

    protected override void Init()
    {
        base.Init();
    }

    public override object GetValue(NodePort port)
    {
        return base.GetValue(port);
    }
}

[Serializable]
public class ChatData
{
    [SerializeField]
    public string charName;
    [SerializeField]
    public CharData left;
    [SerializeField]
    public CharData middle; 
    [SerializeField]
    public CharData right;
    [SerializeField]
    public Sprite background;
    [SerializeField]
    public List<EventData> eventDatas;
    [TextArea,SerializeField]
    public string text;

#if UNITY_EDITOR
    [MenuItem("Data/Plot/Excelת��Ϊ����")]
    public static void ExcelToPlot()
    {
        //string path = EditorUtility.OpenFilePanel("ѡ�������ļ�Ŀ¼", Application.dataPath, " ");
        //string info = new FileInfo(path).Name;//��ȡ��ǰ�籾����
        //FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
        //try
        //{
        //    var excelData = ExcelReaderFactory.CreateOpenXmlReader(stream);
            
        //    System.Data.DataSet sheets = excelData.AsDataSet();

        //    foreach (System.Data.DataTable sheet in sheets.Tables)//�����еı����
        //    {
        //        string plotPath = "Assets/Resources/Data/PlotData/" + info;
        //        //Ѱ�ҵ�ǰ���ڵ��ļ��У�����������򴴽�
        //        if (!Directory.Exists(plotPath))
        //        {
        //            Directory.CreateDirectory(plotPath);
        //        }

        //        var cols = sheet.Columns.Count;//ExcelFile�ı�Sheet������
        //        var count = sheet.Rows.Count;//Sheet�е�����

        //        for (int i = 0; i < cols; i += 3)
        //        {
        //            if (sheet.Rows[0][i].ToString() == "")//�������Ϊ�գ�����
        //                break;
        //            PlotScriptableObject plot = ScriptableObject.CreateInstance("PlotScriptableObject") as PlotScriptableObject;
        //            PlotData plotData = new PlotData();
        //            plot.plot = plotData;
        //            plot.plot.plotTag = sheet.Rows[0][i].ToString();
        //            string assetPath = string.Format("{0}/{1}.asset", plotPath, sheet.Rows[0][i].ToString());
        //            for (int j = 2; j < count; j++)
        //            {
        //                if (sheet.Rows[j][i + 1].ToString() == "")//�������Ϊ�գ�����
        //                    break;

        //                DialogueData dialogue = new DialogueData();
        //                if (sheet.Rows[j][i].ToString() == "")
        //                    dialogue.charData = "char_null";
        //                else
        //                    dialogue.charData = sheet.Rows[j][i].ToString();//ͨ���ַ�ָ����ɫ����
        //                dialogue.text = sheet.Rows[j][i + 1].ToString();
        //                if (sheet.Rows[j][i + 2].ToString() != "")
        //                {
        //                    string[] events = sheet.Rows[j][i + 2].ToString().Split(' ', '=');//��'��=�з��ַ���
        //                    for (int k = 0; k < events.Length; k += 2)
        //                    {
        //                        EventData data = new EventData
        //                        {
        //                            eventData = (EventEnum)Enum.Parse(typeof(EventEnum), events[k]),
        //                            eventValue = events[k + 1]
        //                        };
        //                        dialogue.eventDatas.Add(data);
        //                    }
        //                }
        //                plot.plot.dialogues.Add(dialogue);
        //            }
        //            AssetDatabase.CreateAsset(plot, assetPath);
        //        }
        //    }
        //    stream.Close();
        //    Debug.Log("ת�����");
        //}
        //catch (Exception e)
        //{
        //    stream.Close();
        //}
    }

    [MenuItem("Data/Plot/����ת��ΪCSV")]
    public static void PlotToCSV()
    {
        StorySO[] storySOs = Resources.LoadAll<StorySO>("StoryData");

        //string path = EditorUtility.OpenFilePanel("ѡ�������ļ�Ŀ¼", Application.dataPath, " ");
        //string info = new FileInfo(path).Name;//��ȡ��ǰ�籾����
        //FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
        //try
        //{
        //    var excelData = ExcelReaderFactory.CreateOpenXmlReader(stream);
        //    System.Data.DataSet sheets = excelData.AsDataSet();

        //    foreach (System.Data.DataTable sheet in sheets.Tables)//�����еı����
        //    {
        //        string plotPath = "Assets/Resources/Data/PlotData/" + info;
        //        //Ѱ�ҵ�ǰ���ڵ��ļ��У�����������򴴽�
        //        if (!Directory.Exists(plotPath))
        //        {
        //            Directory.CreateDirectory(plotPath);
        //        }

        //        var cols = sheet.Columns.Count;//ExcelFile�ı�Sheet������
        //        var count = sheet.Rows.Count;//Sheet�е�����

        //        for (int i = 0; i < cols; i += 3)
        //        {
        //            if (sheet.Rows[0][i].ToString() == "")//�������Ϊ�գ�����
        //                break;
        //            PlotScriptableObject plot = ScriptableObject.CreateInstance("PlotScriptableObject") as PlotScriptableObject;
        //            PlotData plotData = new PlotData();
        //            plot.plot = plotData;
        //            plot.plot.plotTag = sheet.Rows[0][i].ToString();
        //            string assetPath = string.Format("{0}/{1}.asset", plotPath, sheet.Rows[0][i].ToString());
        //            for (int j = 2; j < count; j++)
        //            {
        //                if (sheet.Rows[j][i + 1].ToString() == "")//�������Ϊ�գ�����
        //                    break;

        //                DialogueData dialogue = new DialogueData();
        //                if (sheet.Rows[j][i].ToString() == "")
        //                    dialogue.charData = "char_null";
        //                else
        //                    dialogue.charData = sheet.Rows[j][i].ToString();//ͨ���ַ�ָ����ɫ����
        //                dialogue.text = sheet.Rows[j][i + 1].ToString();
        //                if (sheet.Rows[j][i + 2].ToString() != "")
        //                {
        //                    string[] events = sheet.Rows[j][i + 2].ToString().Split(' ', '=');//��'��=�з��ַ���
        //                    for (int k = 0; k < events.Length; k += 2)
        //                    {
        //                        EventData data = new EventData
        //                        {
        //                            eventData = (EventEnum)Enum.Parse(typeof(EventEnum), events[k]),
        //                            eventValue = events[k + 1]
        //                        };
        //                        dialogue.eventDatas.Add(data);
        //                    }
        //                }
        //                plot.plot.dialogues.Add(dialogue);
        //            }
        //            AssetDatabase.CreateAsset(plot, assetPath);
        //        }
        //    }
        //    stream.Close();
        //    Debug.Log("ת�����");
        //}
        //catch (Exception e)
        //{
        //    stream.Close();
        //}
    }
#endif
}

public class StoryCSVData
{
    public string storyName = "storyName";
    public int foreDay=-1;
    public int afterDay=-1;
    public string plotTag="NULL";
}
[Serializable]
public class CharData
{
    [SerializeField]
    public CharSO charSO;
    [SerializeField]
    public ActionData actionData;
}

public enum ChatTag
{
    Start,//����״̬
    Chat,//��׼״̬
    Option,//ѡ��֧״̬
    Trigger,//�ж�֧״̬
}

public enum SoundTag
{
    None,
    Doubt_S,
    Doubt_M,
    Doubt_L,
    Reluctantly,
    Hesitate,
    Speechless,
    Happy,
    Approve
}