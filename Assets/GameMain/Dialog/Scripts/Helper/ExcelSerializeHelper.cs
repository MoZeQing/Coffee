using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialog;
using OfficeOpenXml;
using System.IO;
using UnityEditor;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using GameMain;
using System.Linq;

public class ExcelSerializeHelper : IDialogSerializeHelper
{
    public DialogData Serialize(object data)
    {
        DialogData dialogData = new DialogData();
        ExcelPackage package = data as ExcelPackage;
        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
        Dictionary<string, BaseData> mapsDialogData = new Dictionary<string, BaseData>();
        int rowCount = worksheet.Dimension.Rows;

        StartData startData = new StartData();
        dialogData.DialogDatas.Add(startData);

        for (int row = 3; row <= rowCount; row++) // �������ݴӵ�3�п�ʼ
        {
            // ����Ƿ���Ҫ������ǰ�У��жϵ�0���Ƿ���#��ͷ
            if (worksheet.Cells[row, 1].Value != null && worksheet.Cells[row, 1].Value.ToString().StartsWith("#"))
            {
                continue; // ������ "#" ��ͷ����
            }

            try
            {
                string dialogType = worksheet.Cells[row, 2].Value.ToString(); // ��ȡ���ͣ��±�[1]��
                string identifier = worksheet.Cells[row, 3].Value.ToString(); // ��ȡ��ʶ�����±�[2]��

                BaseData baseData = dialogType switch
                {
                    "0" => ChatSerialize(worksheet, row),
                    "1" => OptionSerialize(worksheet, row),
                    "2" => BackgroundSerialize(worksheet, row),
                    "3" => BlackSerialize(worksheet, row),
                    _ => throw new CSVParseException(row, $"Unknown type '{dialogType}'")
                };

                baseData.Id = int.Parse(identifier); // ���ñ�ʶ��
                mapsDialogData.Add(identifier, baseData); // �� baseData �� identifier Ϊ�������ֵ�
                dialogData.DialogDatas.Add(baseData);
            }
            catch (Exception ex) when (ex is FormatException || ex is ArgumentException)
            {
                // �׳��Զ����쳣����λ������
                throw new CSVParseException(row, $"Error processing row {row}: {ex.Message}");
            }
        }

        // ���������߼�
        LinkData(worksheet, dialogData, mapsDialogData, rowCount);
        return dialogData;
    }

    private BaseData ChatSerialize(ExcelWorksheet worksheet, int row)
    {
        ChatData chatData = new ChatData();

        string chatPosValue = worksheet.Cells[row, 13].Value?.ToString();
        if (string.IsNullOrEmpty(chatPosValue))
        {
            throw new CSVParseException(row, "Chat position value is null or empty.");
        }
        chatData.chatPos = (DialogPos)int.Parse(chatPosValue);

        chatData.charName = worksheet.Cells[row, 14].Value?.ToString();
        chatData.text = worksheet.Cells[row, 15].Value?.ToString();

        // �������С��ҽ�ɫ
        chatData.leftAction = CreateActionData(worksheet, row, 4, 5, 6);
        chatData.middleAction = CreateActionData(worksheet, row, 7, 8, 9);
        chatData.rightAction = CreateActionData(worksheet, row, 10, 11, 12);

        return chatData;
    }

    private ActionData CreateActionData(ExcelWorksheet worksheet, int row, int idCol, int diffTagCol, int actionTagCol)
    {
        ActionData actionData = new ActionData();
        string charId = worksheet.Cells[row, idCol].Value?.ToString();
        if (!string.IsNullOrEmpty(charId))
        {
            actionData.charSO = Resources.Load<CharSO>($"CharSO/{charId}");
            if (actionData.charSO == null)
            {
                throw new CSVParseException(row, $"Character SO not found for ID '{charId}' in column {idCol}");
            }
            actionData.diffTag = (DiffTag)int.Parse(worksheet.Cells[row, diffTagCol].Value.ToString());
            actionData.actionTag = (ActionTag)int.Parse(worksheet.Cells[row, actionTagCol].Value.ToString());
        }
        return actionData;
    }


    private BaseData BackgroundSerialize(ExcelWorksheet worksheet, int row)
    {
        BackgroundData backgroundData = new BackgroundData();

        string backgroundTagValue = worksheet.Cells[row, 11].Value?.ToString();
        if (string.IsNullOrEmpty(backgroundTagValue))
        {
            throw new CSVParseException(row, "Background tag value is null or empty.");
        }
        backgroundData.backgroundTag = (BackgroundTag)int.Parse(backgroundTagValue);

        string parmOneValue = worksheet.Cells[row, 12].Value?.ToString();
        if (string.IsNullOrEmpty(parmOneValue))
        {
            throw new CSVParseException(row, "Parameter One value is null or empty.");
        }
        backgroundData.parmOne = int.Parse(parmOneValue);

        string parmTwoValue = worksheet.Cells[row, 13].Value?.ToString();
        if (string.IsNullOrEmpty(parmTwoValue))
        {
            throw new CSVParseException(row, "Parameter Two value is null or empty.");
        }
        backgroundData.parmTwo = int.Parse(parmTwoValue);

        backgroundData.parmThree = worksheet.Cells[row, 14].Value?.ToString();

        string spriteName = worksheet.Cells[row, 15].Value?.ToString();
        if (string.IsNullOrEmpty(spriteName))
        {
            throw new CSVParseException(row, "Sprite name is null or empty.");
        }
        backgroundData.backgroundSpr = Resources.Load<Sprite>("Background/" + spriteName);
        if (backgroundData.backgroundSpr == null)
        {
            throw new CSVParseException(row, $"Background sprite not found for name '{spriteName}' in row {row}");
        }

        return backgroundData;
    }


    private BaseData BlackSerialize(ExcelWorksheet worksheet, int row)
    {
        return new BlackData
        {
            text = worksheet.Cells[row, 15].Value.ToString()
        };
    }

    private BaseData OptionSerialize(ExcelWorksheet worksheet, int row)
    {
        return new OptionData
        {
            text = worksheet.Cells[row, 15].Value.ToString()
        };
    }



    private void LinkData(ExcelWorksheet worksheet, DialogData dialogData, Dictionary<string, BaseData> mapsDialogData, int rowCount)
    {
        BaseData fore = dialogData.DialogDatas.OfType<StartData>().FirstOrDefault();

        for (int row = 3; row <= rowCount; row++)
        {
            // ����Ƿ���Ҫ������ǰ��
            if (worksheet.Cells[row, 1].Value != null && worksheet.Cells[row, 1].Value.ToString().StartsWith("#"))
            {
                continue; // ������ "#" ��ͷ����
            }

            string identifier = worksheet.Cells[row, 3].Value.ToString(); // ֱ��ʹ�� identifier
            if (mapsDialogData.TryGetValue(identifier, out var baseData))
            {
                if (baseData.Fore.Count == 0)
                {
                    if (!fore.After.Contains(baseData))
                        fore.After.Add(baseData);
                    if (!baseData.Fore.Contains(fore))
                        baseData.Fore.Add(fore);
                }

                string[] tags = worksheet.Cells[row, 16].Value.ToString().Split('-');
                foreach (string tag in tags)
                {
                    if (tag == "0")
                        continue;

                    if (mapsDialogData.TryGetValue(tag, out var linkedData))
                    {
                        if (!baseData.After.Contains(linkedData))
                            baseData.After.Add(linkedData);
                        if (!linkedData.Fore.Contains(baseData))
                            linkedData.Fore.Add(baseData);
                    }
                }

                fore = baseData;
            }
        }
    }
}
