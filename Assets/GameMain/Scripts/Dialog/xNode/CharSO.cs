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
        public int favor;//�øж�
        public int mood;//����
        public int hope;//ϣ��
        public int love;//����
        public int family;//����
        public int ability;

        public CharData() { }
        public CharData(int favor, int mood, int hope, int love, int family)
        { 
            this.favor = favor;
            this.mood = mood;
            this.hope = hope;
            this.love = love;
            this.family = family;
        }
    }
}

