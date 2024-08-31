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

public class ExcelSerializeHelper : IDialogSerializeHelper
{
    /// <summary>
    /// ���ݽ���
    /// </summary>
    /// <param name="baseDatas"></param>
    /// <param name="data">Excel���</param>
    public DialogData Serialize( object data)
    { 
        DialogData dialogData=new DialogData();
        ExcelPackage package = data as ExcelPackage;
        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];//�����ļ�Ĭ��ֻʹ�ñ�1
        Dictionary<string,BaseData> mapsDialogData= new Dictionary<string,BaseData>();
        int rowCount = worksheet.Dimension.Rows;

        StartData startData = new StartData()
        {
            dialogName = worksheet.Cells[3, 4].Value.ToString()
        };
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
         *  �¼���ʹ��|�ַ����зָ16
         *  ��ת����ǰ��ĳ��飬��Ϊ����Ĭ���˳��Ի���17
         */
        for (int row = 4; row <= rowCount; row++)//���ӣ�1��1����ʼ
        {
            //��׼���Ƹ�ʽ��1_1
            string chatTag = $"{worksheet.Cells[row, 2].Value}_{worksheet.Cells[row, 3].Value}";
            //�Ի�ģʽ
            if (worksheet.Cells[row, 1].Value.ToString() == "0")
            {
                ChatData chatData = new ChatData();
                chatData.charName = worksheet.Cells[row, 14].Value.ToString();
                chatData.text = worksheet.Cells[row, 15].Value.ToString();
                chatData.chatPos = (DialogPos)int.Parse(worksheet.Cells[row, 13].Value.ToString());
                chatData.leftAction = new ActionData();
                if (worksheet.Cells[row, 4].Value.ToString() != "0")
                {
                    chatData.leftAction.charSO = Resources.Load<CharSO>("CharSO/" + worksheet.Cells[row, 4].Value.ToString());
                    chatData.leftAction.diffTag = (DiffTag)int.Parse(worksheet.Cells[row, 5].Value.ToString());
                    chatData.leftAction.actionTag = (ActionTag)int.Parse(worksheet.Cells[row, 6].Value.ToString());
                }
                chatData.middleAction = new ActionData();
                if (worksheet.Cells[row, 7].Value.ToString() != "0")
                {
                    chatData.middleAction.charSO = Resources.Load<CharSO>("CharSO/" + worksheet.Cells[row, 7].Value.ToString());
                    chatData.middleAction.diffTag = (DiffTag)int.Parse(worksheet.Cells[row, 8].Value.ToString());
                    chatData.middleAction.actionTag = (ActionTag)int.Parse(worksheet.Cells[row, 9].Value.ToString());
                }
                chatData.rightAction = new ActionData();
                if (worksheet.Cells[row, 10].Value.ToString() != "0")
                {
                    chatData.rightAction.charSO = Resources.Load<CharSO>("CharSO/" + worksheet.Cells[row, 10].Value.ToString());
                    chatData.rightAction.diffTag = (DiffTag)int.Parse(worksheet.Cells[row, 11].Value.ToString());
                    chatData.rightAction.actionTag = (ActionTag)int.Parse(worksheet.Cells[row, 12].Value.ToString());
                }
                mapsDialogData.Add(chatTag, chatData);
                dialogData.DialogDatas.Add(chatData);
            }
            //ѡ��ģʽ
            if (worksheet.Cells[row, 1].Value.ToString() == "1")
            {
                OptionData optionData = new OptionData();
                optionData.text = worksheet.Cells[row, 15].Value.ToString();
                mapsDialogData.Add(chatTag, optionData);
                dialogData.DialogDatas.Add(optionData);
            }
            //����ģʽ
            if (worksheet.Cells[row, 1].Value.ToString() == "2")
            { 
                BackgroundData backgroundData=new BackgroundData();
                backgroundData.backgroundTag = (BackgroundTag)int.Parse(worksheet.Cells[row, 11].Value.ToString());
                backgroundData.parmOne = int.Parse(worksheet.Cells[row, 12].Value.ToString());
                backgroundData.parmTwo = int.Parse(worksheet.Cells[row, 13].Value.ToString());
                backgroundData.parmThree = worksheet.Cells[row, 14].Value.ToString();
                backgroundData.backgroundSpr= Resources.Load<Sprite>("Background/" + worksheet.Cells[row, 15].Value.ToString());
                mapsDialogData.Add(chatTag, backgroundData);
                dialogData.DialogDatas.Add(backgroundData);
            }
        }
        BaseData fore = startData;
        for (int row = 4; row <= rowCount; row++)
        {
            string chatTag = $"{worksheet.Cells[row, 2].Value}_{worksheet.Cells[row, 3].Value}";
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
                string[] tags = worksheet.Cells[row, 16].Value.ToString().Split('-');
                foreach (string tag in tags)
                {
                    if (worksheet.Cells[row, 16].Value.ToString() == "0")
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
        return dialogData;
    }
}
