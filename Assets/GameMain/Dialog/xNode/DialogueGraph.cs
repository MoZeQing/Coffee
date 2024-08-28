using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using System;
using System.IO;
using OfficeOpenXml;//Epplus
using System.Runtime.InteropServices.ComTypes;

namespace GameMain
{
    //[CreateAssetMenu(fileName = "DialogueGraph")]
    public class DialogueGraph : NodeGraph
    {
        [TextArea(5, 10)]
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

        //[MenuItem("���뵼������/�Ի��ļ�����")]
        public static void SOToExcel()
        {
            Debug.Log(0);
        }

        [MenuItem("���뵼������/�Ի��ļ�ת��")]
        public static void ExcelToSO()
        {
            try
            {
                string path = EditorUtility.OpenFolderPanel("�򿪶�Ӧ���ļ�", "C://", "");//����һ���ű�����·��
                DirectoryInfo root = new DirectoryInfo(path);
                FileInfo[] fileInfos = root.GetFiles();
                Dictionary<string, CharSO> charPair = new Dictionary<string, CharSO>();
                foreach (CharSO charSO in Resources.LoadAll<CharSO>("CharData"))
                {
                    charPair.Add(charSO.name, charSO);
                }
                foreach (FileInfo fileInfo in fileInfos)
                {
                    ExcelPackage package = new ExcelPackage(fileInfo);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];//�����ļ�Ĭ��ֻʹ�ñ�1
                    string savePath = "Assets/GameMain/Resources/DialogData/" + Path.GetFileNameWithoutExtension(fileInfo.Name) + ".asset";
                    DialogueGraph dialogue = DialogueGraph.CreateInstance<DialogueGraph>();
                    AssetDatabase.CreateAsset(dialogue, savePath);

                    int index = 0;
                    int rowCount = worksheet.Dimension.Rows;

                    StartNode startNode = dialogue.AddNode<StartNode>() as StartNode;
                    startNode.name = "Start";
                    AssetDatabase.AddObjectToAsset(startNode, dialogue);
                    /*��ʽҪ��
                     * �ӵڶ��п�ʼ
                     *  �����ͣ�����0Ϊ�ĶԻ���1Ϊѡ��2Ϊ�жϣ�1
                     *  ����ţ��ڼ����飩2
                     *  ��ѡ���ڵ�ǰ���е����3
                     *  ���ɫ��ID ��� ��Ч�����ڶԻ�������Ч4 5 6
                     *  �н�ɫ��ID ��� ��Ч�����ڶԻ�������Ч7 8 9
                     *  �ҽ�ɫ��ID ��� ��Ч�����ڶԻ�������Ч10 11 12
                     *  ��ɫ���ƣ�ʵ�ʶԻ��е����ƣ�13
                     *  �ı�����ѡ���У����жϿ�����������ж��߼������Ƽ���14
                     *  ����15
                     *  �¼���ʹ��|�ַ����зָ16
                     *  ��ת����ǰ��ĳ��飬��Ϊ����Ĭ���˳��Ի���17
                     */
                    //�ȳ�ʼ���飬Ȼ��ȫ�����ز����ߣ�֮���ټ�������
                    for (int row = 3; row <= rowCount; row++)//���ӣ�1��1����ʼ
                    {
                        if (worksheet.Cells[row, 2].Value.ToString() != index.ToString())//�½��Ŀ����
                        {
                            index++;
                            if (worksheet.Cells[row, 1].Value.ToString() == "0")
                            {
                                ChatNode chatNode = dialogue.AddNode<ChatNode>() as ChatNode;
                                chatNode.name = "Chat";
                                AssetDatabase.AddObjectToAsset(chatNode, dialogue);
                            }

                            if (worksheet.Cells[row, 1].Value.ToString() == "1")
                            {
                                OptionNode optionNode = dialogue.AddNode<OptionNode>() as OptionNode;
                                optionNode.name = "Option";
                                AssetDatabase.AddObjectToAsset(optionNode, dialogue);
                            }
                        }
                    }
                    for (int row = 3; row <= rowCount; row++)//���ӣ�1��1����ʼ
                    {
                        //���Ի�
                        if (worksheet.Cells[row, 1].Value.ToString() == "0")
                        {
                            ChatData chatData = new ChatData();
                            chatData.charName = worksheet.Cells[row, 13].Value.ToString();
                            chatData.text = worksheet.Cells[row, 14].Value.ToString();
                            if (worksheet.Cells[row, 15].Value.ToString() != "0")
                            {
                                //chatData.background = Resources.Load<Sprite>("Image/Background/" + worksheet.Cells[row, 18].Value.ToString());
                            }
                            if (worksheet.Cells[row, 4].Value.ToString() != "0")
                            {
                                chatData.left = new CharData();
                                chatData.left.charSO = charPair[worksheet.Cells[row, 4].Value.ToString()];
                                chatData.left.diffTag = (DiffTag)int.Parse(worksheet.Cells[row, 5].Value.ToString());
                                chatData.left.actionTag = (ActionTag)int.Parse(worksheet.Cells[row, 6].Value.ToString());
                            }
                            if (worksheet.Cells[row, 7].Value.ToString() != "0")
                            {
                                chatData.middle = new CharData();
                                chatData.middle.charSO = charPair[worksheet.Cells[row, 7].Value.ToString()];
                                chatData.middle.diffTag = (DiffTag)int.Parse(worksheet.Cells[row, 8].Value.ToString());
                                chatData.middle.actionTag = (ActionTag)int.Parse(worksheet.Cells[row, 9].Value.ToString());
                            }
                            if (worksheet.Cells[row, 10].Value.ToString() != "0")
                            {
                                chatData.right = new CharData();
                                chatData.right.charSO = charPair[worksheet.Cells[row, 10].Value.ToString()];
                                chatData.right.diffTag = (DiffTag)int.Parse(worksheet.Cells[row, 11].Value.ToString());
                                chatData.right.actionTag = (ActionTag)int.Parse(worksheet.Cells[row, 12].Value.ToString());
                            }
                            ChatNode chatNode = dialogue.nodes[int.Parse(worksheet.Cells[row, 2].Value.ToString())] as ChatNode;
                            chatNode.chatDatas.Add(chatData);
                            if (worksheet.Cells[row, 17].Value.ToString() != "0")
                            {
                                string nextNode = worksheet.Cells[row, 17].Value.ToString();
                                string[] tags = nextNode.Split('-');
                                for (int j = 0; j < tags.Length; j++)
                                {
                                    NodePort nodePort = chatNode.GetPort($"chatDatas {worksheet.Cells[row, 3].Value}");
                                    nodePort.Connect(dialogue.nodes[int.Parse(tags[j])].GetInputPort("Input"));
                                }
                            }
                        }

                        if (worksheet.Cells[row, 1].Value.ToString() == "1")
                        {
                            OptionData optionData = new OptionData();
                            optionData.text = worksheet.Cells[row, 14].Value.ToString();
                            OptionNode optionNode = dialogue.nodes[int.Parse(worksheet.Cells[row, 2].Value.ToString())] as OptionNode;
                            optionNode.optionDatas.Add(optionData);
                            if (worksheet.Cells[row, 17].Value.ToString() != "0")
                            {
                                string nextNode = worksheet.Cells[row, 17].Value.ToString();
                                string[] tags = nextNode.Split('-');
                                for (int j = 0; j < tags.Length; j++)
                                {
                                    optionNode.GetOutputPort($"optionDatas {worksheet.Cells[row, 3].Value}").Connect(dialogue.nodes[int.Parse(tags[j])].GetInputPort("Input"));
                                }
                            }
                        }
                    }
                    EditorUtility.SetDirty(dialogue);
                }

            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        public static string GetOutPortName(int node, int index)
        {
            switch (node)
            {
                case 0:
                    return string.Format("chatDatas {0}", index);
                case 1:
                    return string.Format("optionDatas {0}", index);
                case 2:
                    return string.Format("triggerDatas {0}", index);
            }
            return string.Empty;
        }

        //[MenuItem("Data/Dialog/������")]
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

}