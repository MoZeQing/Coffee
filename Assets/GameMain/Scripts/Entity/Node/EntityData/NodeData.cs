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
        CoffeeLiquid,//����Һ
        Sugar,//��
        Ice,//��
        LowFoamingMilk,//����ţ��
        HighFoamingMilk,//����ţ��
        //����
        ManualGrinder,//�ֶ���ĥ��
        ElectricGrinder,//�綯��ĥ��
        Kettle,//���Ⱥ�
        FilterBowl,//��ֽʽ�˱�
        Cup,//���ȱ�
        Stirrer,//������
        //����
        Espresso,//Ũ������
        Coffee,//����
        HotCafeAmericano,//����ʽ
        WhiteCoffee,//�׿���
        HotLatte,//������
        HotMocha,//��Ħ��
        ConPanna,//������
        Kapuziner,//������ŵ
        FlatWhite,//�İ�
        //������
        IceEspresso,//��Ũ������
        IceWhiteCoffee,//���׿���
        IceConPanna,//��������
        IceLatte,//������
        IceCafeAmericano,//����ʽ����
        IceMocha,//��Ħ��
        //����
        SweetEspresso,//����Ũ������
        SweetWhiteCoffee,//���ǰ׿���
        SweetConPanna,//���ǿ�����
        SweetLatte,//��������
        SweetCafeAmericano,//������ʽ����
        SweetMocha,//����Ħ��
        None
    }
}

