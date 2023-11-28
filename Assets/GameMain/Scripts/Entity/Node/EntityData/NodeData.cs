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
        //���ϣ�1-100��
        CoffeeBean,//���ȶ�
        CoarseGroundCoffee,//�ֿ��ȷ�
        MidGroundCoffee,//�п��ȷ�
        FineGroundCoffee,//ϸ���ȷ�
        Water,//ˮ
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
        //���ߣ�101-200��
        ManualGrinder=101,//�ֶ���ĥ��
        Extractor=102,//�綯��ȡ
        ElectricGrinder=103,//�綯��ĥ��
        Heater=104,//������
        Syphon=105,//������
        FrenchPress=106,//��ѹ��
        Kettle=107,//���ݺ�
        FilterBowl=108,//��ֽʽ�˱�
        Cup=109,//���ȱ�
        Stirrer=110,//������
        //���ȣ�201-300��
        Espresso=201,//Ũ������
        HotCafeAmericano=202,//����ʽ
        IceCafeAmericano=203,//����ʽ
        Conpanna=204,//������
        Vienna=205,//άҲ�ɿ���
        HotLatte=206,//������
        IceLatte=207,//������
        HotMocha=208,//��Ħ��
        IceMocha=209,//��Ħ��
        Kapuziner=210,//������ŵ
        FlatWhite=211,//�İ�
        Dirty=212,//�࿧��
        Ole=213,//ŷ�ٿ���
        Cat=999,//èè����999��
        None=0
    }
}

