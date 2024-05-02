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

        public BaseCompenent Adsorb
        {
            get;
            set;
        }

        public bool RamdonJump
        {
            get;
            set;
        }
        public List<NodeTag> Materials
        {
            get;
            set;
        } = new List<NodeTag>();
        public bool IsCoffee
        {
            get;
            set;
        } = false;
        public bool Grind
        {
            get;
            set;
        } = false;
        public bool Ice
        {
            get;
            set;
        } = false;
        public NodeData(int entityId, int typeId, NodeTag node)
            : base(entityId, typeId)
        {
            this.NodeTag = node;
        }
        public NodeData(int entityId, int typeId, NodeTag node,int level=0)
            : base(entityId, typeId)
        {
            this.NodeTag = node;
            this.MLevel = level;
        }
        public NodeData(int entityId, int typeId, NodeTag node, bool isCoffee, int level=0)
            : base(entityId, typeId)
        {
            this.NodeTag = node;
            this.MLevel = level;
            this.IsCoffee = isCoffee;
        }
        public NodeData(int entityId, int typeId, NodeTag node, bool isCoffee, List<NodeTag> materials, int level=0)
            : base(entityId, typeId)
        {
            this.NodeTag = node;
            this.MLevel = level;
            this.IsCoffee = isCoffee;
            this.Materials = materials;
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
        HHeater=111,
        HStirrer=112,
        //���ȣ�200-300��
        EspressoG =200,//�ֿ��ȷ�
        Espresso =201,//Ũ������
        CafeAmericano =202,//����ʽ
        IceCafeAmericano =203,//����ʽ
        Kapuziner = 204,//������ŵ
        IceKapuziner = 205,//��������ŵ
        Latte = 206,//����
        IceLatte = 207,//������
        Conpanna =208,//������
        IceConpanna =209,//��������
        Ole = 210,//ŷ��
        IceOle = 211,//��ŷ��
        Mocha = 212,//Ħ��
        IceMocha = 213,//��Ħ��
        Vienna =214,//άҲ��
        IceVienna = 215,//��άҲ��
        Macchiato=216,//�����
        IceMacchiato =217,//�������
        SaltCoffee =218,//����
        Mediterranean =219,//���к�
        Cat =999,//èè����999��
        None=0
    }
}

