using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trigger 
{
    public TriggerTag key;//ʲô��ǩ
    public bool not;//�Ƿ�ȡ��
    public bool equals;
    public string value;//���ٵ���ֵ
    public virtual List<Trigger> GetAndTrigger()
    {
        return null;
    }

    public virtual List<Trigger> GetOrTrigger()
    {
        return null;
    }
}
[System.Serializable]
public class ParentTrigger : Trigger
{
    public List<FirstTrigger> and = new List<FirstTrigger>();
    public List<FirstTrigger> or = new List<FirstTrigger>();

    public override List<Trigger> GetAndTrigger()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (FirstTrigger firstTrigger in and)
        {
            Trigger trigger = firstTrigger;
            ans.Add(trigger);
        }
        return ans;
    }

    public override List<Trigger> GetOrTrigger()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (FirstTrigger firstTrigger in or)
        {
            Trigger trigger = firstTrigger;
            ans.Add(trigger);
        }
        return ans;
    }
}
[System.Serializable]
public class FirstTrigger : Trigger
{
    public List<SceondTrigger> and = new List<SceondTrigger>();
    public List<SceondTrigger> or=new List<SceondTrigger>();
    public override List<Trigger> GetAndTrigger()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (SceondTrigger firstTrigger in and)
        {
            Trigger trigger = firstTrigger;
            ans.Add(trigger);
        }
        return ans;
    }

    public override List<Trigger> GetOrTrigger()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (SceondTrigger firstTrigger in or)
        {
            Trigger trigger = firstTrigger;
            ans.Add(trigger);
        }
        return ans;
    }
}
[System.Serializable]
public class SceondTrigger : Trigger
{
    public List<ThirdTrigger> and = new List<ThirdTrigger>();
    public List<ThirdTrigger> or = new List<ThirdTrigger>();
    public override List<Trigger> GetAndTrigger()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (ThirdTrigger firstTrigger in and)
        {
            Trigger trigger = firstTrigger;
            ans.Add(trigger);
        }
        return ans;
    }

    public override List<Trigger> GetOrTrigger()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (ThirdTrigger firstTrigger in or)
        {
            Trigger trigger = firstTrigger;
            ans.Add(trigger);
        }
        return ans;
    }
}
[System.Serializable]
public class ThirdTrigger : Trigger
{
    public List<FourthTrigger> and = new List<FourthTrigger>();
    public List<FourthTrigger> or = new List<FourthTrigger>();
    public override List<Trigger> GetOrTrigger()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (FourthTrigger firstTrigger in or)
        {
            Trigger trigger = firstTrigger;
            ans.Add(trigger);
        }
        return ans;
    }

    public override List<Trigger> GetAndTrigger()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (FourthTrigger firstTrigger in and)
        {
            Trigger trigger = firstTrigger;
            ans.Add(trigger);
        }
        return ans;
    }
}
[System.Serializable]
public class FourthTrigger : Trigger
{
    public override List<Trigger> GetAndTrigger()
    {
        return new List<Trigger>();
    }

    public override List<Trigger> GetOrTrigger()
    {
        return new List<Trigger>();
    }
}

[System.Serializable]
public class TriggerData
{
    [SerializeField]
    public ParentTrigger trigger;
    [SerializeField]
    public List<EventData> events = new List<EventData>();
}

[System.Serializable]
public class EventData
{
    public EventTag eventTag;
    public string value;

    public EventData() { }
    public EventData(EventTag eventTag)
    { 
        this.eventTag= eventTag;
    }
    public EventData(EventTag eventTag, string value)
    {
        this.eventTag= eventTag;   
        this.value= value;
    }
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
    Ability,
    Money,//��ǰӵ�е�Ǯ
    Coffee,//�Ƿ���ڿ��ȣ�һ���п��ȣ�
    Day,//Ŀǰ������ʱ��
    Energy,//����
    MaxEnergy,//�������
    Ap,//�ж���
    MaxAp,//����ж���
    Location,//λ��
    TimeTag,//ʱ���ǩ
    Week,//����
    BehaviorTag,
    Index,
    Rent,//�������
}
[System.Serializable]
public enum EventTag
{
    Play,//���ž��飨��������ʱĬ�ϲ������Ӿ��飬���򲥷Ų���ָ���ľ��飩
    AddFavor,//���Ӻøжȣ�����Ϊ��ɫID��
    AddLove,
    AddHope,
    AddFamily,
    AddMood,
    AddAbility,
    AddEnergy,
    AddMaxEnergy,
    AddAp,
    AddMoney,//���ӽ�Ǯ������Ϊ��Ǯ������
    AddItem,//���ӵ���
    AddFlag,//��������
    RemoveFlag,//�Ƴ�����
    NextDay,//��ת����һ��
    PlayBgm,//����bgm
    EndGame,//������Ϸ
    AddDay,//����ʱ��
    AddAction,//����Action
    Rent,//�������
}