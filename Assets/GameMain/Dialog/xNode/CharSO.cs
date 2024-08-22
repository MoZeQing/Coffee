using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    [CreateAssetMenu]
    public class CharSO : ScriptableObject
    {
        public bool isMain;
        public bool friend;
        public Vector3 offset;
        public float scale;
        public string charName;
        public int favor;
        public Sprite sprite;
        public Sprite orderSprite;
        public NodeTag favorCoffee;
        public List<Sprite> diffs = new List<Sprite>();
        public List<int> audios = new List<int>();
        [TextArea(5,10)]
        public string text;
        public CharAward[] charAwards= new CharAward[4];
    }
    [System.Serializable]
    public class CharAward
    {
        public int favor;//Ŀ������
        public string text;//����
        public List<EventData> awards= new List<EventData>();//Ч��
    }
    [System.Serializable]
    public class CharData
    {
        public int closet;
        public int favor;//�øж�
        public int stamina;//����
        public int wisdom;//�ǻ�
        public int charm;//����
        public int StaminaLevel 
        {
            get
            {
                return Mathf.Min(stamina / 40 + 1,4);
            }
        }//����
        public int WisdomLevel
        {
            get
            {
                return Mathf.Min(wisdom / 40 + 1, 4);
            }
        }//�ǻ�
        public int CharmLevel
        {
            get
            {
                return Mathf.Min(charm / 40 + 1, 4);
            }
        }//����

        public CharData() { }

        public Dictionary<ValueTag, int> GetValueTag(Dictionary<ValueTag, int> dic)
        {
            if (favor != 0)
                dic.Add(ValueTag.Favor, favor);
            if (stamina != 0)
                dic.Add(ValueTag.Stamina, stamina);
            if (wisdom != 0)
                dic.Add(ValueTag.Wisdom, wisdom);
            if (charm != 0)
                dic.Add(ValueTag.Charm, charm);
            return dic;
        }
    }
}

