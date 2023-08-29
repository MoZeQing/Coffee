using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework;


namespace GameMain
{
    public class CharacterData : EntityData
    {
        public string CharName
        {
            get;
            set;
        }//��ɫ����

        public DialogPos DialogPos
        {
            get;
            set;
        }
        public List<Sprite> diffs = new List<Sprite>();//���

        public List<int> audios = new List<int>();

        public CharacterData(int entityId, int typeId,CharSO charSO)
            : base(entityId, typeId)
        {
            CharName = charSO.charName;
            diffs = charSO.diffs;
            audios= charSO.audios;
        }
    }
}

