﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeWidth(400)]
public class TriggerNode : Node
{
    [Input] public float a;
    [Output] public float b;

    [SerializeField, Output(dynamicPortList = true)]
    public List<TriggerData> triggerDatas = new List<TriggerData>();
    protected override void Init()
    {
        base.Init();

    }

    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port)
    {
        return null; // Replace this
    }
}

[System.Serializable]
public class TriggerData
{
    [SerializeField]
    public Trigger trigger;
    [SerializeField]
    public List<EventData> events = new List<EventData>();
}

[System.Serializable]
public class EventData
{
    public EventTag eventTag;
    public string value;
}
[System.Serializable]
public enum TriggerTag
{ 
    None,
    Flag,
    Davor,//当前角色的好感度
    Money,//当前拥有的钱
}
[System.Serializable]
public enum EventTag
{
    Play,//播放剧情（在有链接时默认播放链接剧情，否则播放参数指定的剧情）
    AddFavor,//增加好感度（参数为角色ID）
    AddMoney,//增加金钱（参数为金钱数量）
    AddFlag,//增加旗子
    RemoveFlag//移除旗子
}

