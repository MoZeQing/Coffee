using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Random = UnityEngine.Random;

namespace GameMain
{
    public class UtilsComponent : GameFrameworkComponent
    {
        public List<Sprite> nodeSprites = new List<Sprite>();
        //�涨����0��Ĭ���Ǻ����л�����ÿһ��֮����л�������1�ſ�ʼ����ÿһ������
        public List<Sprite> changeSprites= new List<Sprite>();

        //���ݹ���������
        private Dictionary<TriggerTag,string> _values= new Dictionary<TriggerTag,string>();
        private List<string> _flags= new List<string>();

        public void AddFlag(string flag)
        { 
            _flags.Add(flag);
        }

        public void Remove(string flag)
        { 
            _flags.Remove(flag);
        }
        public bool Check(TriggerData triggerData)
        {
            return Check(triggerData.trigger);
        }

        public bool Check(Trigger trigger)
        {
            if (trigger.key == TriggerTag.Flag)
                return _flags.Contains(trigger.value);
            if (trigger == null)
                return true;
            if (trigger.And.Count != 0)
            {
                foreach (Trigger tr in trigger.And)
                {
                    if (!Check(tr))
                    {
                        return false;
                    }
                }
            }
            if (trigger.OR.Count != 0)
            {
                foreach (Trigger tr in trigger.OR)
                {
                    if (Check(tr))
                    {
                        return true;
                    }
                }
                return false;
            }
            if (trigger.key == TriggerTag.None)
                return true;
            if (!_values.ContainsKey(trigger.key))
                return false;
            if (trigger.equals)
            {
                if (_values[trigger.key] == trigger.value)
                    return true;
                else
                    return false;
            }
            else
            {
                if (trigger.not)//�ж�����
                {
                    if (int.Parse(_values[trigger.key]) > int.Parse(trigger.value))
                        return false;
                    else
                        return true;
                }
                else
                {
                    if (int.Parse(_values[trigger.key]) < int.Parse(trigger.value))
                        return false;
                    else
                        return true;
                }
            }
        }

        private void Start()
        {
            _values.Add(TriggerTag.Davor, "5");
            _values.Add(TriggerTag.Money, "300");
        }

        private void OnDisable()
        {
            
        }
    }
}