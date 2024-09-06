using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XNode;
using DG.Tweening;
using XNode.Examples.LogicToy;
using System.Runtime.InteropServices.ComTypes;
using Dialog;

public class DialogBox : MonoBehaviour
{
    [SerializeField] protected BaseStage stage;
    [Header("UI�Ի�����")]
    [SerializeField] private UIBtnsControl uIBtnsControl;
    [SerializeField] protected Button dialogBtn;
    [SerializeField] protected Text dialogText;
    [SerializeField] protected Text nameText;
    [SerializeField] protected Transform optionCanvas;
    [SerializeField] protected GameObject btnPre;
    [Range(0.1f, 0.5f)]
    [SerializeField] protected float charSpeed = 0.05f;

    // �����Ͱ�ɫ�ı����
    [Header("�������ɫ�ı�")]
    [SerializeField] private CanvasGroup blackScreenCanvasGroup; // ���� CanvasGroup
    [SerializeField] private Text blackScreenText; // ��ʾ�����µİ�ɫ����

    protected DialogData m_DialogData = null;
    protected BaseData m_Data = null;

    protected ChatTag mChatTag;
    protected int mIndex;

    protected bool optionFlag;
    protected List<GameObject> m_Btns = new List<GameObject>();

    protected Action OnComplete;

    public bool IsSkip { get; set; } = false;
    public bool IsNext { get; set; } = true;
    protected float _time = 0f;
    public float SkipSpeed { get; set; } = 20f;
    private bool isBlackScreenActive = false; // ��ǵ�ǰ�Ƿ��ں���״̬
    public float autoPlayDelay = 1f; // �Զ����Ž���ʱ���ӳ�
    public bool isAutoPlay;
    public bool isSkipbtn;
    private bool isTextComplete = false; // ��ʶ��ǰ�Ի��ı��Ƿ񲥷����
    private float autoPlayTimer = 0f; // �Զ����ŵļ�ʱ��

    private void Start()
    {
        dialogBtn.onClick.AddListener(Next);
    }

    private void Update()
    {
        IsSkip = Input.GetKey(KeyCode.LeftControl);
        if(IsSkip)
        {
            uIBtnsControl.skipBtnImage.sprite = uIBtnsControl.skipUsing;
        }
        else
        {
            if(!uIBtnsControl.isSkip)
            {
                uIBtnsControl.skipBtnImage.sprite = uIBtnsControl.skipNormal;
            }
        }
        _time += Time.deltaTime;

        // �ֶ�������߰��� Space / Return ��������һ��
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            Next();

        // ���������߼�
        if (IsSkip || isSkipbtn)
        {
            if (_time > (1 / SkipSpeed))
            {
                Next();
                _time = 0;
            }
        }

        // �Զ������߼�
        if (isAutoPlay && isTextComplete)
        {
            autoPlayTimer += Time.deltaTime;
            if (autoPlayTimer >= autoPlayDelay)
            {
                Next();
                autoPlayTimer = 0f;
                isTextComplete = false; // ����״̬
            }
        }
    }

    public void ClearButtons()
    {
        foreach (GameObject go in m_Btns)
        {
            Destroy(go);
        }
        m_Btns.Clear();
    }

    protected virtual void Option_Onclick(object sender, EventArgs e)
    {
        BaseData optionData = sender as BaseData;
        if (optionData == null)
            return;
        optionFlag = false;
        ClearButtons();
        m_Data = optionData;
        Next();
    }

    public virtual void Next()
    {
        if (optionFlag || !IsNext || m_Data == null)
            return;

        try
        {
            if (m_Data.After.Count != 0)
            {
                foreach (BaseData after in m_Data.After)
                {
                    string dialogTag = after.GetType().Name;
                    switch (dialogTag)
                    {
                        case "StartData":
                            StartData startData = (StartData)after;
                            InitDailog(startData);
                            break;
                        case "ChatData":
                            ChatData chatData = (ChatData)after;
                            ShowChat(chatData);
                            break;
                        case "OptionData":
                            OptionData optionData = (OptionData)after;
                            ShowOption(optionData);
                            break;
                        case "BlackData":
                            BlackData blackData = (BlackData)after;
                            ShowBlackChat(blackData);
                            break;
                        default:
                            Debug.LogWarning($"Unknown dialog tag '{dialogTag}' encountered in Next().");
                            break;
                    }
                }
            }
            else
            {
                CompleteDialog();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error occurred in Next(): {ex.Message}\nBaseData identifier: {m_Data.Identifier}\nStack Trace: {ex.StackTrace}");
        }
        isTextComplete = false;
    }

    virtual public void CompleteDialog()
    {
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

    protected virtual void InitDailog(StartData startData)
    {
        Next();
    }

    protected virtual void ShowOption(OptionData optionData)
    {
        try
        {
            optionFlag = true;
            GameObject go = Instantiate(btnPre, optionCanvas);
            go.GetComponent<OptionItem>().OnInit(optionData, Option_Onclick);
            m_Btns.Add(go);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error showing option: {ex.Message}\nOptionData identifier: {optionData.Identifier}");
        }
    }

    protected virtual void ShowChat(ChatData chatData)
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
            Debug.LogError($"Error showing chat: {ex.Message}\nChatData identifier: {chatData.Identifier}");
        }
    }

    private void DisplayChat(ChatData chatData)
    {
        stage.ShowCharacter(chatData);
        nameText.text = chatData.charName == "0" ? string.Empty : chatData.charName;

        if (DOTween.IsTweening(dialogText))
        {
            dialogText.DOKill();
            dialogText.text = string.Empty;
            dialogText.text = chatData.text;
            m_Data = chatData;
            isTextComplete = true; // �ı�ֱ����ʾ���
        }
        else
        {
            dialogText.DOKill();
            dialogText.text = string.Empty;
            dialogText.DOText(chatData.text, charSpeed * chatData.text.Length, true)
                .OnComplete(() =>
                {
                    m_Data = chatData;
                    isTextComplete = true; // �ı�������ϣ����ñ�ʶ
                });
        }
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

    public virtual void SetDialog(DialogData dialogData)
    {
        IsSkip = false;
        OnComplete = null;
        m_DialogData = dialogData;
        m_Data = dialogData.GetStartData();
        mIndex = 0;
        Next();
    }
    public virtual void SetDialog(DialogueGraph dialogueGraph)
    {
        XNodeSerializeHelper helper = new XNodeSerializeHelper();
        DialogData dialogData = helper.Serialize(dialogueGraph);
        SetDialog(dialogData);
    }
    public virtual void SetComplete(Action action)
    {
        OnComplete = action;
    }

    public void ChangeAutoPlay()
    {
        if (isAutoPlay)
        {
            isAutoPlay = false;
        }
        else
        {
            isAutoPlay = true;
        }
    }
    public void ChangeSkipPlay()
    {
        if (isSkipbtn)
        {
            isSkipbtn = false;
        }
        else
        {
            isSkipbtn = true;
        }
    }
}
