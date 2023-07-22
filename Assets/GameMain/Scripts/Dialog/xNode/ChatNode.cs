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
    [Output] public float b;

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
    public CharSO charSO;//角色
    [SerializeField]
    public ActionData actionData;//动作数据
    [TextArea,SerializeField]
    public string text;
}

public enum ChatTag
{
    Start,//启动状态
    Chat,//标准状态
    Option,//选择支状态
    Trigger,//判断支状态
}