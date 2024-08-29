using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

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
        string[] triggers = triggerText.Split('[', ']' );
        foreach (string trigger in triggers)
        { 
            
        }
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
                sb.Append(")");
            }
        }
        else
        {
            if (trigger.GetTriggers().Count != 0)
                sb.Append("(&");
            if (trigger.not)
            {
                if (trigger.equals)
                    sb.Append($"[{trigger.key}!={trigger.value}]");
                else
                    sb.Append($"[{trigger.key}<{trigger.value}]");
            }
            else
            {
                if (trigger.equals)
                    sb.Append($"[{trigger.key}={trigger.value}]");
                else
                    sb.Append($"[{trigger.key}>{trigger.value}]");
            }
            if (trigger.GetTriggers().Count != 0)
            {
                List<Trigger> andTriggers = trigger.GetTriggers();
                for (int i = 0; i < andTriggers.Count; i++)
                {
                    TriggerToString(andTriggers[i], sb);
                }
                sb.Append(")");
            }
        }
        return sb;
    }
}
[System.Serializable]
public class ParentTrigger : Trigger
{
    public List<FirstTrigger> and = new List<FirstTrigger>();

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
}
[System.Serializable]
public class ThirdTrigger : Trigger
{
    public override List<Trigger> GetTriggers()
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
    public string[] values;

    public EventData() { }
    public EventData(EventTag eventTag)
    { 
        this.eventTag= eventTag;
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
}