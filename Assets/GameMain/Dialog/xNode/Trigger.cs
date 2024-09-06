using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

[System.Serializable]
public class Trigger 
{
    public TriggerTag key;//ʲô��ǩ
    public bool not;//�Ƿ�ȡ��
    public bool equals;
    public string value;//���ٵ���ֵ
    public virtual List<Trigger> GetTriggers()
    {
        return null;
    }

    public Trigger() { }

    public Trigger(string triggerText) 
    {
        key = TriggerTag.None;
        value = string.Empty;
        equals = false;
        not = false;
        GetTriggers().Clear();
        List<string> triggers = new List<string>(triggerText.Split('[', ']', '(','/'));
        InitTrigger(triggers, 1);
    }
    public virtual void InitTrigger(string triggerText)
    {
        key = TriggerTag.None;
        value=string.Empty;
        equals = false;
        not= false;
        GetTriggers().Clear();
        List<string> triggers = new List<string>(triggerText.Split('[', ']', '(', '/'));
        InitTrigger(triggers,1);
    }
    public virtual int InitTrigger(List<string> triggerTexts,int index)
    {
        for (int i= index; i<triggerTexts.Count;i++)
        {
            if (triggerTexts[i] == "|")
            {
                key = TriggerTag.Or;
                Trigger trigger = new Trigger();
                i = trigger.InitTrigger(triggerTexts, i + 1);
                GetTriggers().Add(trigger);
            }
            else if (triggerTexts[i] == "&")
            {

            }
            else if (triggerTexts[i] == ")")
                return i;
            else
            {
                if (key != TriggerTag.None)
                {
                    Trigger newTrigger = new Trigger();
                    string[] triggerTags = triggerTexts[i].Split(' ');
                    newTrigger.key = (TriggerTag)Enum.Parse(typeof(TriggerTag), triggerTags[0]);
                    if (triggerTags[1] == "<")
                    {
                        newTrigger.not = true;
                    }
                    if (triggerTags[1] == ">")
                    {

                    }
                    if (triggerTags[1] == "=")
                    {
                        newTrigger.equals = true;
                    }
                    if (triggerTags[1] == "!=")
                    {
                        newTrigger.not = true;
                        newTrigger.equals = true;
                    }
                    newTrigger.value = triggerTags[2];
                    GetTriggers().Add(newTrigger);
                }
                else
                {
                    string[] triggerTags = triggerTexts[i].Split(' ');
                    key = (TriggerTag)Enum.Parse(typeof(TriggerTag), triggerTags[0]);
                    if (triggerTags[1] == "<")
                    {
                        not = true;
                    }
                    if (triggerTags[1] == ">")
                    {

                    }
                    if (triggerTags[1] == "=")
                    {
                        equals = true;
                    }
                    if (triggerTags[1] == "!=")
                    {
                        not = true;
                        equals = true;
                    }
                    value = triggerTags[2];
                }
            }
        }
        return 0;
    }

    public string TriggerToString()
    {
        return TriggerToString(this, new StringBuilder()).ToString();
    }

    public static StringBuilder TriggerToString(Trigger trigger, StringBuilder sb)
    {
        if (trigger == null)
            return sb;
        if (trigger.key == TriggerTag.Or)
        {
            if (trigger.GetTriggers().Count != 0)
            {
                sb.Append("(|");
                List<Trigger> orTriggers = trigger.GetTriggers();
                for (int i = 0; i < orTriggers.Count; i++)
                {
                    TriggerToString(orTriggers[i], sb);
                }
                sb.Append("/)");
            }
        }
        else
        {
            if (trigger.GetTriggers().Count != 0)
                sb.Append("(&");
            if (trigger.not)
            {
                if (trigger.equals)
                    sb.Append($"[{trigger.key} != {trigger.value}]");
                else
                    sb.Append($"[{trigger.key} < {trigger.value}]");
            }
            else
            {
                if (trigger.equals)
                    sb.Append($"[{trigger.key} = {trigger.value}]");
                else
                    sb.Append($"[{trigger.key} > {trigger.value}]");
            }
            if (trigger.GetTriggers().Count != 0)
            {
                List<Trigger> andTriggers = trigger.GetTriggers();
                for (int i = 0; i < andTriggers.Count; i++)
                {
                    TriggerToString(andTriggers[i], sb);
                }
                sb.Append("/)");
            }
        }
        return sb;
    }
}
[System.Serializable]
public class ParentTrigger : Trigger
{
    public List<FirstTrigger> and = new List<FirstTrigger>();

    public ParentTrigger(string triggerText)
    {
        key = TriggerTag.None;
        value = string.Empty;
        equals = false;
        not = false;
        and.Clear();
        List<string> triggers = new List<string>(triggerText.Split('[', ']', '(', '/'));
        InitTrigger(triggers, 1);
    }
    public override List<Trigger> GetTriggers()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (FirstTrigger firstTrigger in and)
        {
            Trigger trigger = firstTrigger;
            ans.Add(trigger);
        }
        return ans;
    }

    public override int InitTrigger(List<string> triggerTexts, int index)
    {
        for (int i = index; i < triggerTexts.Count; i++)
        {
            if (triggerTexts[i] == "|")
            {
                if (key == TriggerTag.None)
                    key = TriggerTag.Or;
                else 
                {
                    FirstTrigger trigger = new FirstTrigger();
                    trigger.key = TriggerTag.Or;
                    i = trigger.InitTrigger(triggerTexts, i + 1);
                    and.Add(trigger);
                }
            }
            else if (triggerTexts[i] == "&")
            {
                if (key != TriggerTag.None)
                {
                    FirstTrigger trigger = new FirstTrigger();
                    i = trigger.InitTrigger(triggerTexts, i + 1);
                    and.Add(trigger);
                }
            }
            else if (triggerTexts[i] == ")")
                return i;
            else if (triggerTexts[i] == string.Empty)
            {

            }
            else
            {
                if (key != TriggerTag.None)
                {
                    FirstTrigger newTrigger = new FirstTrigger();
                    string[] triggerTags = triggerTexts[i].Split(' ');
                    newTrigger.key = (TriggerTag)Enum.Parse(typeof(TriggerTag), triggerTags[0]);
                    if (triggerTags[1] == "<")
                    {
                        newTrigger.not = true;
                    }
                    if (triggerTags[1] == ">")
                    {

                    }
                    if (triggerTags[1] == "=")
                    {
                        newTrigger.equals = true;
                    }
                    if (triggerTags[1] == "!=")
                    {
                        newTrigger.not = true;
                        newTrigger.equals = true;
                    }
                    newTrigger.value = triggerTags[2];
                    and.Add(newTrigger);
                }
                else
                {
                    string[] triggerTags = triggerTexts[i].Split(' ');
                    key = (TriggerTag)Enum.Parse(typeof(TriggerTag), triggerTags[0]);
                    if (triggerTags[1] == "<")
                    {
                        not = true;
                    }
                    if (triggerTags[1] == ">")
                    {

                    }
                    if (triggerTags[1] == "=")
                    {
                        equals = true;
                    }
                    if (triggerTags[1] == "!=")
                    {
                        not = true;
                        equals = true;
                    }
                    value = triggerTags[2];
                }
            }
        }
        return 0;
    }
}
[System.Serializable]
public class FirstTrigger : Trigger
{
    public List<SceondTrigger> and = new List<SceondTrigger>();
    public override List<Trigger> GetTriggers()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (SceondTrigger firstTrigger in and)
        {
            Trigger trigger = firstTrigger;
            ans.Add(trigger);
        }
        return ans;
    }

    public override int InitTrigger(List<string> triggerTexts, int index)
    {
        for (int i = index; i < triggerTexts.Count; i++)
        {
            if (triggerTexts[i] == "|")
            {
                if (key == TriggerTag.None)
                    key = TriggerTag.Or;
                else
                {
                    SceondTrigger trigger = new SceondTrigger();
                    trigger.key = TriggerTag.Or;
                    i = trigger.InitTrigger(triggerTexts, i + 1);
                    and.Add(trigger);
                }
            }
            else if (triggerTexts[i] == "&")
            {
                if (key != TriggerTag.None)
                {
                    SceondTrigger trigger = new SceondTrigger();
                    i = trigger.InitTrigger(triggerTexts, i + 1);
                    and.Add(trigger);
                }
            }
            else if (triggerTexts[i] == ")")
                return i;
            else if (triggerTexts[i] == string.Empty)
            { 
            
            }
            else
            {
                if (key != TriggerTag.None)
                {
                    SceondTrigger newTrigger = new SceondTrigger();
                    string[] triggerTags = triggerTexts[i].Split(' ');
                    newTrigger.key = (TriggerTag)Enum.Parse(typeof(TriggerTag), triggerTags[0]);
                    if (triggerTags[1] == "<")
                    {
                        newTrigger.not = true;
                    }
                    if (triggerTags[1] == ">")
                    {

                    }
                    if (triggerTags[1] == "=")
                    {
                        newTrigger.equals = true;
                    }
                    if (triggerTags[1] == "!=")
                    {
                        newTrigger.not = true;
                        newTrigger.equals = true;
                    }
                    newTrigger.value = triggerTags[2];
                    and.Add(newTrigger);
                }
                else
                {
                    string[] triggerTags = triggerTexts[i].Split(' ');
                    key = (TriggerTag)Enum.Parse(typeof(TriggerTag), triggerTags[0]);
                    if (triggerTags[1] == "<")
                    {
                        not = true;
                    }
                    if (triggerTags[1] == ">")
                    {

                    }
                    if (triggerTags[1] == "=")
                    {
                        equals = true;
                    }
                    if (triggerTags[1] == "!=")
                    {
                        not = true;
                        equals = true;
                    }
                    value = triggerTags[2];
                }
            }
        }
        return 0;
    }
}
[System.Serializable]
public class SceondTrigger : Trigger
{
    public List<ThirdTrigger> and = new List<ThirdTrigger>();
    public override List<Trigger> GetTriggers()
    {
        List<Trigger> ans = new List<Trigger>();
        foreach (ThirdTrigger firstTrigger in and)
        {
            Trigger trigger = firstTrigger;
            ans.Add(trigger);
        }
        return ans;
    }
    public override int InitTrigger(List<string> triggerTexts, int index)
    {
        for (int i = index; i < triggerTexts.Count; i++)
        {
            if (triggerTexts[i] == "|")
            {
                if (key == TriggerTag.None)
                    key = TriggerTag.Or;
                else
                {
                    ThirdTrigger trigger = new ThirdTrigger();
                    trigger.key = TriggerTag.Or;
                    i = trigger.InitTrigger(triggerTexts, i + 1);
                    and.Add(trigger);
                }
            }
            else if (triggerTexts[i] == "&")
            {
                if (key != TriggerTag.None)
                {
                    ThirdTrigger trigger = new ThirdTrigger();
                    i = trigger.InitTrigger(triggerTexts, i + 1);
                    and.Add(trigger);
                }
            }
            else if (triggerTexts[i] == ")")
                return i;
            else if (triggerTexts[i] == string.Empty)
            {

            }
            else
            {
                if (key != TriggerTag.None)
                {
                    ThirdTrigger newTrigger = new ThirdTrigger();
                    string[] triggerTags = triggerTexts[i].Split(' ');
                    newTrigger.key = (TriggerTag)Enum.Parse(typeof(TriggerTag), triggerTags[0]);
                    if (triggerTags[1] == "<")
                    {
                        newTrigger.not = true;
                    }
                    if (triggerTags[1] == ">")
                    {

                    }
                    if (triggerTags[1] == "=")
                    {
                        newTrigger.equals = true;
                    }
                    if (triggerTags[1] == "!=")
                    {
                        newTrigger.not = true;
                        newTrigger.equals = true;
                    }
                    newTrigger.value = triggerTags[2];
                    and.Add(newTrigger);
                }
                else
                {
                    string[] triggerTags = triggerTexts[i].Split(' ');
                    key = (TriggerTag)Enum.Parse(typeof(TriggerTag), triggerTags[0]);
                    if (triggerTags[1] == "<")
                    {
                        not = true;
                    }
                    if (triggerTags[1] == ">")
                    {

                    }
                    if (triggerTags[1] == "=")
                    {
                        equals = true;
                    }
                    if (triggerTags[1] == "!=")
                    {
                        not = true;
                        equals = true;
                    }
                    value = triggerTags[2];
                }
            }
        }
        return 0;
    }
}
[System.Serializable]
public class ThirdTrigger : Trigger
{
    public override List<Trigger> GetTriggers()
    {
        return new List<Trigger>();
    }

    public override int InitTrigger(List<string> triggerTexts, int index)
    {
        string[] triggerTags = triggerTexts[index].Split(' ');
        key = (TriggerTag)Enum.Parse(typeof(TriggerTag), triggerTags[0]);
        if (triggerTags[1] == "<")
        {
            not = true;
        }
        if (triggerTags[1] == ">")
        {

        }
        if (triggerTags[1] == "=")
        {
            equals = true;
        }
        if (triggerTags[1] == "!=")
        {
            not = true;
            equals = true;
        }
        value = triggerTags[2];
        return index;
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
    public string[] values;

    public EventData() { }
    public EventData(EventTag eventTag)
    { 
        this.eventTag= eventTag;
    }
    public EventData(string eventText)
    {
        if (string.IsNullOrEmpty(eventText)) return;
        string[] strings = eventText.Split(' ');
        eventTag = (EventTag)Enum.Parse(typeof(EventTag), eventText);
    }
    public override string ToString()
    { 
        string ans=eventTag.ToString();
        for(int i=0;i<values.Length;i++)
            ans += $" {values[i]}";
        return ans;
    }
}
[System.Serializable]
public enum TriggerTag
{
    None,
    Or,
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
    //����
    FDog,//����
    FRegular,//¶ϣ��
    FWitch,//�ϵ�
    FFiction,//С˵��
    FMoney,//��ծ��
    FCourier,//����Ա
    FlagCount,

    Wisdom,//�ǻ�
    Stamina,//����
    Charm//����
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
    AddRecipe,//�����䷽
    AddFriend,
    AddScene,//���ӳ���
    Test,
    Jump,
    ShowFlag,//չʾ���е�Flag����
    AddBuff,//����buff
    RemoveBuff,//ж��buff
    Weather,//����
    SetClothing,//���÷�װ
    ShowForm,//��ʾ����
    AddCharm,
    AddStamina,
    WorkTest
}