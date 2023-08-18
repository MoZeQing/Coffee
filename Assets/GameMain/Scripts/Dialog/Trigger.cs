using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trigger 
{
    public List<Trigger> OR = new List<Trigger>();
    public List<Trigger> And = new List<Trigger>();

    public TriggerTag key;//ʲô��ǩ
    public bool not;//�Ƿ�ȡ��
    public bool equals;
    public string value;//���ٵ���ֵ
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
    Flag,//�������
    Favor,//��ǰ��ɫ�ĺøж�
    Hope,//ϣ��
    Mood,//����
    Love,//����
    Family,//����
    Money,//��ǰӵ�е�Ǯ
    Coffee,//�Ƿ���ڿ��ȣ�һ���п��ȣ�
    Day,//Ŀǰ������ʱ��
    Energy,//����
    MaxEnergy,//�������
    Ap,//�ж���
    MaxAp,//����ж���
    Location,
    Index
}
[System.Serializable]
public enum EventTag
{
    Play,//���ž��飨��������ʱĬ�ϲ������Ӿ��飬���򲥷Ų���ָ���ľ��飩
    AddFavor,//���Ӻøжȣ�����Ϊ��ɫID��
    AddMoney,//���ӽ�Ǯ������Ϊ��Ǯ������
    AddFlag,//��������
    RemoveFlag//�Ƴ�����
}