using Dialog;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVSerializeHelper : IDialogSerializeHelper
{
    public DialogData Serialize( object data)
    {
        DialogData dialogData=new DialogData();
        Dictionary<string, BaseData> mapsDialogData = new Dictionary<string, BaseData>();
        string dialogText = data.ToString();
        string[] dialogRows = dialogText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        string[] startDialogs = dialogRows[2].Split(',');
        StartData startData = new StartData()
        { 
            dialogName= startDialogs[4]
        };
        dialogData.DialogName = startData.dialogName;
        dialogData.DialogDatas.Add(startData);

        for (int i = 3; i < dialogRows.Length - 1; i++) // ���ӣ�1��1����ʼ
        {
            string[] dialogs = dialogRows[i].Split(',');
            if (dialogs[0] == "#")
                continue;

            try
            {
                BaseData baseData = dialogs[1] switch
                {
                    "0" => ChatSerialize(dialogs),
                    "1" => OptionSerialize(dialogs),
                    "2" => BackgroundSerialize(dialogs),
                    "3" => BlackSerialize(dialogs),
                    _ => throw new CSVParseException(i, $"{dialogData.DialogName} Unknown type '{dialogs[1]}'")
                };

                string identifier = dialogs[2];
                baseData.Identifier = identifier; // ���ñ�ʶ���� BaseData
                mapsDialogData.Add(identifier, baseData); // ʹ�ñ�ʶ����Ϊ��
                dialogData.DialogDatas.Add(baseData);
            }
            catch (Exception ex) when (ex is FormatException || ex is ArgumentException)
            {
                // ����ʹ�� CSVParseException ������ǰ�кŴ��ݽ�ȥ
                throw new CSVParseException(i + 1, $"{dialogData.DialogName} Error processing row {i + 1}: {ex.Message}");
            }
        }

        // �������ݵ��߼����ֲ���
        BaseData fore = startData;
        for (int i = 3; i < dialogRows.Length - 1; i++)
        {
            string[] dialogs = dialogRows[i].Split(',');
            if (dialogs[0] == "#")
                continue;

            string chatTag = dialogs[2];
            if (!mapsDialogData.TryGetValue(chatTag, out var baseData))
            {
                continue; // ����δ�ҵ������
            }

            if (baseData.Fore.Count == 0)
            {
                if (!fore.After.Contains(baseData))
                    fore.After.Add(baseData);
                if (!baseData.Fore.Contains(fore))
                    baseData.Fore.Add(fore);
            }

            string[] tags = dialogs[15].Split('-');
            foreach (string tag in tags)
            {
                if (tag == "0")
                    continue;

                if (mapsDialogData.TryGetValue(tag, out var nextBaseData))
                {
                    if (!nextBaseData.Fore.Contains(baseData))
                        nextBaseData.Fore.Add(baseData);
                    if (!baseData.After.Contains(nextBaseData))
                        baseData.After.Add(nextBaseData);
                }
            }

            fore = baseData;
        }
        return dialogData;
    }

    private BaseData BackgroundSerialize(string[] csvString)
    {
        return new BackgroundData
        {
            backgroundTag = (BackgroundTag)int.Parse(csvString[10]),
            parmOne = int.Parse(csvString[11]),
            parmTwo = int.Parse(csvString[12]),
            parmThree = csvString[13],
            backgroundSpr = Resources.Load<Sprite>("Background/" + csvString[14])
        };
    }

    public BaseData ChatSerialize(string[] csvString)
    {
        ChatData chatData = new ChatData();
        chatData.chatPos = (DialogPos)int.Parse(csvString[12]);
        chatData.charName = csvString[13];
        chatData.text = csvString[14];
        chatData.leftAction = new ActionData();
        if (csvString[3] != string.Empty)
        {
            chatData.leftAction.charSO = Resources.Load<CharSO>($"CharSO/{csvString[3]}");
            chatData.leftAction.diffTag = (DiffTag)int.Parse(csvString[4]);
            chatData.leftAction.actionTag = (ActionTag)int.Parse(csvString[5]);
        }
        chatData.middleAction = new ActionData();
        if (csvString[6] != string.Empty)
        {
            chatData.middleAction.charSO = Resources.Load<CharSO>($"CharSO/{csvString[6]}");
            chatData.middleAction.diffTag = (DiffTag)int.Parse(csvString[7]);
            chatData.middleAction.actionTag = (ActionTag)int.Parse(csvString[8]);
        }

        chatData.rightAction = new ActionData();
        if (csvString[9] != string.Empty)
        {
            chatData.rightAction.charSO = Resources.Load<CharSO>($"CharSO/{csvString[9]}");
            chatData.rightAction.diffTag = (DiffTag)int.Parse(csvString[10]);
            chatData.rightAction.actionTag = (ActionTag)int.Parse(csvString[11]);
        }
        return chatData;
    }
    public BaseData BlackSerialize(string[] csvString)
    {
        BlackData blackData = new BlackData();
        blackData.text = csvString[14];
        return blackData;
    }
    public BaseData OptionSerialize(string[] csvString)
    {
        return new OptionData { text = csvString[14] };
    }
}

/// <summary>
/// �Զ����쳣�࣬���� CSV �����еĴ���λ
/// </summary>
/// <summary>
/// �Զ����쳣�࣬���� CSV ��������
/// </summary>
public class CSVParseException : Exception
{
    public int Row { get; }

    // ȷ��ֻ����һ�����캯��
    public CSVParseException(int row, string message) : base(message)
    {
        Row = row;
    }
}

