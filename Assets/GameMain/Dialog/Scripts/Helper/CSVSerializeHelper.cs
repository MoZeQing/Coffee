using Dialog;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVSerializeHelper : IDialogSerializeHelper
{
    /// <summary>
    /// ���ݽ���
    /// </summary>
    /// <param name="baseDatas"></param>
    /// <param name="data">Excel���</param>
    public void Serialize(DialogData dialogData, object data)
    {
        Dictionary<string,BaseData> mapsDialogData= new Dictionary<string,BaseData>();
        string dialogText = data.ToString();
        string[] dialogRows = dialogText.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);

        StartData startData = new StartData();
        dialogData.DialogDatas.Add(startData);
        /*��ʽҪ��
         * �ӵڶ��п�ʼ
         *  �����ͣ�����0Ϊ�ĶԻ���1Ϊѡ��2Ϊ�жϣ�1
         *  ����ţ��ڼ����飩2
         *  ��ѡ���ڵ�ǰ���е����3
         *  ���ɫ��ID ��� ��Ч ��Ч�����ڶԻ�������Ч4 5 6
         *  �н�ɫ��ID ��� ��Ч ��Ч�����ڶԻ�������Ч7 8 9
         *  �ҽ�ɫ��ID ��� ��Ч ��Ч�����ڶԻ�������Ч10 11 12
         *  ��ɫ���ƣ�ʵ�ʶԻ��е����ƣ�13
         *  �ı�����ѡ���У����жϿ�����������ж��߼������Ƽ���14
         *  ����15
         *  ��ת����ǰ��ĳ��飬��Ϊ����Ĭ���˳��Ի���16
         *  �¼���ʹ��|�ַ����зָ17
         */
        for (int i = 2; i < dialogRows.Length-1; i++)//���ӣ�1��1����ʼ
        {
            string[] dialogs = dialogRows[i].Split(',');
            if (dialogs[0]=="#")
                continue;
            BaseData baseData = null;
            if (dialogs[1] == "0")
            {
                baseData = ChatSerialize(dialogs);
            }
            if (dialogs[1] == "1")
            {
                baseData = OptionSerialize(dialogs);
            }
            if (dialogs[1] == "2")
            {
                baseData = BackgroundSerialize(dialogs);
            }
            mapsDialogData.Add($"{dialogs[2]}_{dialogs[3]}", baseData);
            dialogData.DialogDatas.Add(baseData);
        }
        BaseData fore = startData;
        for (int i = 2; i < dialogRows.Length-1; i++)
        {
            string[] dialogs = dialogRows[i].Split(',');
            if (dialogs[0] == "#")
                continue;
            string chatTag = $"{dialogs[2]}_{dialogs[3]}";
            BaseData baseData = mapsDialogData[chatTag];
            if (baseData != null)
            {
                if (baseData.Fore.Count == 0)
                {
                    if (!fore.After.Contains(baseData))
                        fore.After.Add(baseData);
                    if (!baseData.Fore.Contains(fore))
                        baseData.Fore.Add(fore);
                }
                string[] tags = dialogs[16].Split('-');
                foreach (string tag in tags)
                {
                    if (tag == "0")
                        continue;
                    BaseData nextBaseData = mapsDialogData[tag];
                    if (nextBaseData != null)
                    {
                        if (!nextBaseData.Fore.Contains(baseData))
                            nextBaseData.Fore.Add(baseData);
                        if (!baseData.Fore.Contains(nextBaseData))
                            baseData.After.Add(nextBaseData);
                    }
                }
            }
            else
            {

            }
            fore = baseData;
        }
        Debug.Log(0);
    }

    private BaseData BackgroundSerialize(string[] csvString)
    {
        BackgroundData backgroundData = new BackgroundData();
        backgroundData.backgroundTag = (BackgroundTag)int.Parse(csvString[11]);
        backgroundData.parmOne = int.Parse(csvString[12]);
        backgroundData.parmTwo = int.Parse(csvString[13]);
        backgroundData.parmThree = csvString[14];
        backgroundData.backgroundSpr = Resources.Load<Sprite>("Background/" + csvString[15]);
        return backgroundData;
    }

    public BaseData ChatSerialize(string[] csvString)
    { 
        ChatData chatData=new ChatData();
        chatData.chatPos = (DialogPos)int.Parse(csvString[13]);
        chatData.charName = csvString[14];
        chatData.text= csvString[15];
        chatData.leftAction = new ActionData();
        if (csvString[4] != string.Empty)
        {
            chatData.leftAction.charSO = Resources.Load<CharSO>($"CharSO/{csvString[4]}");
            chatData.leftAction.diffTag = (DiffTag)int.Parse(csvString[5]);
            chatData.leftAction.actionTag= (ActionTag)int.Parse(csvString[6]);
        }
        chatData.middleAction = new ActionData();
        if (csvString[7] != string.Empty)
        {
            chatData.middleAction.charSO = Resources.Load<CharSO>($"CharSO/{csvString[7]}");
            chatData.middleAction.diffTag = (DiffTag)int.Parse(csvString[8]);
            chatData.middleAction.actionTag = (ActionTag)int.Parse(csvString[9]);
        }
        chatData.rightAction = new ActionData();
        if (csvString[10] != string.Empty)
        {
            chatData.rightAction.charSO = Resources.Load<CharSO>($"CharSO/{csvString[10]}");
            chatData.rightAction.diffTag = (DiffTag)int.Parse(csvString[11]);
            chatData.rightAction.actionTag = (ActionTag)int.Parse(csvString[12]);
        }
        return chatData;
    }

    public BaseData OptionSerialize(string[] csvString)
    { 
        OptionData optionData=new OptionData();
        optionData.text = csvString[15];
        return optionData;
    }
}
