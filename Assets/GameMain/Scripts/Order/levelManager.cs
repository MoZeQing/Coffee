using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework.DataTable;
using UnityEngine.UI;
using System.Xml;

namespace GameMain
{
    public class levelManager : MonoBehaviour
    {
        private void Start()
        {
            
        }
    }
    //�ؿ�����
    public enum LevelTag
    {
        Normal,//��ͨ
        Rain,//����
        Urgent,//�Ӽ�
        Bad,//���ӵĿ���
        Damage//�𻵵Ĺ���̨
    }

    [System.Serializable]
    public class NewOrderData
    { 
        /// <summary>
        /// �÷ݶ����ı�ǩ
        /// </summary>
        public int nodeNodeTag;
        /// <summary>
        /// �÷ݶ������ֵ�ʱ��
        /// </summary>
        public int orderTime;

        public OrderData GetOrderData(LevelTag levelTag,int orderTime,bool isCoarse,bool notCoarse)
        { 
            OrderData orderData=new OrderData();
            DRTag dRTag=GameEntry.DataTable.GetDataTable<DRTag>().GetDataRow(nodeNodeTag);
            string[] tagsText = dRTag.NodeTags.Split('-');
            int[] tags=new int[tagsText.Length];
            for (int i = 0; i < tagsText.Length; i++)
            {
                int tag = 0;
                if (int.TryParse(tagsText[i], out tag))
                {
                    tags[i] = tag;
                }
            }
            return new OrderData((NodeTag)tags[Random.Range(0, tags.Length)], levelTag,orderTime,isCoarse,notCoarse);
        }
    }
}