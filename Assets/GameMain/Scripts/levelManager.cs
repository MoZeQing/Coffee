using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework.DataTable;
using UnityEngine.UI;

namespace GameMain
{
    public class levelManager : MonoBehaviour
    {
        private void Start()
        {
            
        }
    }
    [System.Serializable]
    public class LevelData
    {
        public CharSO charSO;
        public DialogueGraph foreWork;
        public DialogueGraph afterWork;
        public DialogueGraph failWork;
        public List<OrderData> orderDatas;
        public int favor;//���嶩���øжȣ�ÿ���һ�����ձ������Ӷ�Ӧ��favor���Ի�ҲӦ�÷����Ӧ�ĺøж�
        public int orderTime;
    }
}