using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class CompenentData : AccessoryObjectData
    {
        public NodeData NodeData
        {
            get;
            set;
        }
        public bool Sugar
        {
            get;
            set;
        }
        public bool CondensedMilk
        {
            get;
            set;
        }
        public bool Salt
        {
            get;
            set;
        }
        /// <summary>
        /// �ϳ�·���ϵ�ȫ��ԭ����
        /// </summary>
        public List<NodeTag> materials= new List<NodeTag>();

        public CompenentData(int entityId, int typeId, int ownerId, NodeData nodeData)
            : base(entityId, typeId, ownerId)
        { 
            NodeData= nodeData;
        }
    }
}
