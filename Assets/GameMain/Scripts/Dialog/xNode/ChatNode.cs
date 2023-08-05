using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using GameMain;

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
    public CharSO charSO;//��ɫ
    [SerializeField]
    public ActionData actionData;//��������
    [TextArea,SerializeField]
    public string text;
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