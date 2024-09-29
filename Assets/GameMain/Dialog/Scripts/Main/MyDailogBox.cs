using DG.Tweening;
using Dialog;
using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MyDailogBox : DialogBox
{
    [SerializeField] protected Transform canvas;
    [SerializeField] protected BaseBackground background;

    public bool IsCG { get; set; }
    public bool IsBackground { get; set; }

    // �����Ͱ�ɫ�ı����
    [Header("�������ɫ�ı�")]
    [SerializeField] private CanvasGroup blackScreenCanvasGroup; // ���� CanvasGroup
    [SerializeField] private Text blackScreenText; // ��ʾ�����µİ�ɫ����
    [SerializeField] private bool isBlackScreenActive = false; // ��ǵ�ǰ�Ƿ��ں���״̬

    public override void Next()
    {
        if (optionFlag || IsCG || IsBackground || !IsNext || m_Data == null)
            return;

        if (m_Data.After.Count != 0)
        {
            // �����ǰ BaseData �ڵ����Ϣ
            Debug.Log($"Current BaseData ID: {m_Data.Id}");
            Debug.Log($"Fore Count: {m_Data.Fore.Count}, After Count: {m_Data.After.Count}");
            foreach (BaseData after in m_Data.After)
            {
                Debug.Log($"After BaseData ID: {after.Id}, Type: {after.GetType().Name}");
            }
            foreach (BaseData after in m_Data.After)
            {
 
                string dialogTag = after.GetType().Name;
                switch (dialogTag)
                {
                    case "StartData":
                        InitDailog((StartData)after);
                        break;
                    case "ChatData":
                        ShowChat((ChatData)after);
                        break;
                    case "OptionData":
                        ShowOption((OptionData)after);
                        break;
                    case "CGData":
                        ShowCG((CGData)after);
                        break;
                    case "BackgroundData":
                        ShowBackground((BackgroundData)after);
                        break;
                    case "BlackData":
                        ShowBlackChat((BlackData)after); // ֱ�ӵ��ø����ShowBlack����
                        break;
                    case "VoiceData":
                        ShowVoice((VoiceData)after);
                        break;
                }
            }
        }
        else
        {
            CompleteDialog();
        }
    }

    protected override void Option_Onclick(object sender, EventArgs e)
    {
        base.Option_Onclick(sender, e);
        OptionData optionData = sender as OptionData;
        GameEntry.Utils.RunEvent(optionData.eventData);
    }

    public override void CompleteDialog()
    {
        Debug.Log($"CompleteDialog called for BaseData ID: {m_Data?.Id ?? -1}");
        ClearButtons();
        nameText.text = string.Empty;
        dialogText.text = string.Empty;
        blackScreenText.text = string.Empty; // ��պ�������
        blackScreenCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            blackScreenCanvasGroup.gameObject.SetActive(false);
            isBlackScreenActive = false; // ��������״̬
        });
        mIndex = 0;
        m_DialogData = null;
        m_Data = null;
        OnComplete?.Invoke();
        OnComplete = null;
    }

    protected override void ShowChat(ChatData chatData)
    {
        try
        {
            if (isBlackScreenActive)
            {
                // �����ǰ�Ǻ���״̬����Ҫ�˳�����
                blackScreenCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
                {
                    blackScreenCanvasGroup.gameObject.SetActive(false);
                    isBlackScreenActive = false;
                    DisplayChat(chatData);
                });
            }
            else
            {
                DisplayChat(chatData);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error showing chat: {ex.Message}\nChatData identifier: {chatData.Id}");
        }
    }
    public virtual void ShowVoice(VoiceData voiceData)
    {
        m_Data = voiceData;
        GameEntry.Sound.PlaySound(voiceData.voice);
        Next();
    }
    public virtual void ShowBackground(BackgroundData backgroundData)
    {
        m_Data = backgroundData;
        IsBackground = true;
        background.SetBackground(backgroundData, this);
    }

    protected virtual void ShowCG(CGData cgData)
    {
        m_Data = cgData;
        GameEntry.SaveLoad.AddCGFlag(cgData.cgFlag);
        BackgroundData backgroundData = new BackgroundData
        {
            backgroundTag = BackgroundTag.Fade,
            parmOne = cgData.parmOne,
            parmTwo = cgData.parmTwo,
            parmThree = cgData.parmThree,
            backgroundSpr = cgData.cgSpr
        };
        IsBackground = true;
        background.SetBackground(backgroundData, this);
    }

    protected virtual void HideDialogBox()
    {
        canvas.gameObject.SetActive(true);
        IsCG = false;
    }
    protected virtual void ShowBlackChat(BlackData blackData)
    {
        // �����ͨ�Ի�������ݣ���ֹ������������ʾ֮ǰ������
        dialogText.text = string.Empty;
        nameText.text = string.Empty;

        if (!isBlackScreenActive)
        {
            // ���ν������״̬��ִ�к�������Ч��
            blackScreenCanvasGroup.gameObject.SetActive(true);
            blackScreenCanvasGroup.DOFade(1, 0.5f).OnComplete(() =>
            {
                // ������ȫ��ʾ�󣬿�ʼ��ʾ����
                UpdateBlackScreenText(blackData);
            });
            isBlackScreenActive = true;
        }
        else
        {
            // ����Ѿ����ں���״̬��ֱ�Ӹ�������
            UpdateBlackScreenText(blackData);
        }

        m_Data = blackData;
    }



    private void UpdateBlackScreenText(BlackData blackData)
    {
        blackScreenText.DOKill(); // ȷ��û��֮ǰ�Ķ�������
        blackScreenText.text = string.Empty;

        // ʹ�� DOText ����������ʾ����
        blackScreenText.DOText(blackData.text, charSpeed * blackData.text.Length, true)
            .OnComplete(() =>
            {
                // �ı�������ϣ����ñ�ʶ
                isTextComplete = true;
            });


    }
}
