using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    [CreateAssetMenu]
    public class CharSO : ScriptableObject
    {
        [SerializeField]
        public CharData charData;
    }

    [System.Serializable]
    public class CharData
    {
        public string charName;//��ɫ����
        public int favour;//�øж�
        public List<Sprite> Diffs = new List<Sprite>();//��� 
    }
}

