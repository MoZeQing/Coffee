using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework;


namespace GameMain
{
    public class NodeData : EntityData
    {
        /// <summary>
        /// ����������
        /// </summary>
        public NodeTag NodeTag
        {
            get;
            set;
        }

        public NodeGroup NodeGroup
        {
            get;
            set;
        }

        public Vector3 Scale
        {
            get;
            set;
        }

        public float ProducingTime
        {
            get;
            set;
        }

        public bool Follow
        {
            get;
            set;
        }
        public int MLevel
        {
            get;
            set;
        } = 0;
        public bool Jump
        {
            get;
            set;
        }

        public bool RamdonJump
        {
            get;
            set;
        }
        public List<NodeTag> M_Materials
        {
            get;
            set;
        } = new List<NodeTag>();
        public bool IsCoffee
        {
            get;
            set;
        } = false;

        public NodeData(int entityId, int typeId, NodeTag node)
            : base(entityId, typeId)
        {
            this.NodeTag = node;
        }
        public NodeData(int entityId, int typeId, NodeTag node,int level)
            : base(entityId, typeId)
        {
            this.NodeTag = node;
            this.MLevel = level;
        }
        public NodeData(int entityId, int typeId, NodeTag node, int level,bool isCoffee)
            : base(entityId, typeId)
        {
            this.NodeTag = node;
            this.MLevel = level;
            this.IsCoffee = isCoffee;
        }
        public NodeData(int entityId, int typeId, NodeTag node, int level, bool isCoffee,List<NodeTag> materials)
            : base(entityId, typeId)
        {
            this.NodeTag = node;
            this.MLevel = level;
            this.IsCoffee = isCoffee;
            this.M_Materials = materials;
        }
    }

    public enum NodeGroup
    { 
        Material,
        Tool,
        Coffee
    }

    public enum NodeTag
    {
        //����
        CoffeeBean=0,//���ȶ�
        CoarseGroundCoffee,//�ֿ��ȷ�
        MidGroundCoffee,//�п��ȷ�
        FineGroundCoffee,//ϸ���ȷ�
        Water=4,//ˮ
        HotWater,//��ˮ
        Milk,//ţ��
        HotMilk,//��ţ��
        Cream,//����
        ChocolateSyrup,//�ɿ�����
        Ice,//��
        Sugar,//��
        Salt,//��
        CondensedMilk,//����
        LowFoamingMilk,//����ţ��
        HighFoamingMilk,//����ţ��
        //����
        ManualGrinder,//�ֶ���ĥ��
        Extractor,//�綯��ȡ
        ElectricGrinder,//�綯��ĥ��
        Heater,//������
        Syphon,//������
        FrenchPress,//��ѹ��
        Kettle,//���ݺ�
        FilterBowl,//��ֽʽ�˱�
        Cup,//���ȱ�
        Stirrer,//������
        //����
        Espresso,//Ũ������
        HotCafeAmericano,//����ʽ
        IceCafeAmericano,//����ʽ
        HotLatte,//������
        IceLatte,//������
        HotMocha,//��Ħ��
        IceMocha,//��Ħ��
        Kapuziner,//������ŵ
        FlatWhite,//�İ�
        None
    }
}

