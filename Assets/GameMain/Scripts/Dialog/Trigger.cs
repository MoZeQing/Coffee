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
