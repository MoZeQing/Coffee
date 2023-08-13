using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    [CreateAssetMenu]
    public class CharSO : ScriptableObject
    {
        [SerializeField]
        public CharEntityData charData;
    }
    [System.Serializable]
    public class CharData
    {
        public int favor;//�øж�
        public int mood;//����
        public int hope;//ϣ��
        public int love;//����
        public int family;//����

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
    [System.Serializable]
    public class CharEntityData
    {
        public string charName;//��ɫ����
        public List<Sprite> diffs = new List<Sprite>();//���
        public List<int> audios = new List<int>();
    }
}

