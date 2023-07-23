﻿using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace GameMain
{
    public class EspressoNode : Entity, IPointerDownHandler
    {
        private CompenentData m_CompenentData;
        private NodeData m_NodeData;
        private SpriteRenderer m_SpriteRenderer;
        private BoxCollider2D m_BoxCollider2D;

        private List<AdsorbSlot> m_AdsorbSlots = new List<AdsorbSlot>();//��λ1

        private Transform m_ProgressBar = null;
        private float m_ProducingTime = 0f;

        private List<RecipeData> m_RecipeDatas = new List<RecipeData>();

        private List<BaseCompenent> m_ChildDatas = new List<BaseCompenent>();
        private List<NodeTag> m_ChildNodes = new List<NodeTag>();

        private bool m_Follow = false;



        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_CompenentData = (CompenentData)userData;
            m_NodeData = m_CompenentData.NodeData;
            GameEntry.Entity.AttachEntity(this.Id, m_CompenentData.OwnerId);

            m_NodeData.ProducingTime = 2.5f;
            m_ProducingTime = m_NodeData.ProducingTime;

            m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
            m_SpriteRenderer.sprite = GameEntry.Utils.nodeSprites[(int)m_NodeData.NodeTag];

            m_BoxCollider2D = this.GetComponent<BoxCollider2D>();
            m_BoxCollider2D.size = m_SpriteRenderer.size;

            m_AdsorbSlots.Clear();
            m_AdsorbSlots.Add(this.transform.Find("Espresso").GetComponent<AdsorbSlot>());

            m_ProgressBar = this.transform.Find("ProgressBar").transform;//��ȡ������
            m_ProgressBar.gameObject.SetActive(false);

            RecipeData recipe1 = new RecipeData();
            recipe1.Materials.Add(NodeTag.HotWater);
            recipe1.Product = NodeTag.CafeAmericano;
            recipe1.ProductTime = 10f;
            m_RecipeDatas.Add(recipe1);

            RecipeData recipe2 = new RecipeData();
            recipe2.Materials.Add(NodeTag.Milk);
            recipe2.Product = NodeTag.WhiteCoffee;
            recipe2.ProductTime = 10f;
            m_RecipeDatas.Add(recipe2);

            RecipeData recipe3 = new RecipeData();
            recipe3.Materials.Add(NodeTag.ChocolateSyrup);
            recipe3.Materials.Add(NodeTag.Milk);
            recipe3.Materials.Add(NodeTag.Cream);
            recipe3.Product = NodeTag.Mocha;
            recipe3.ProductTime = 10f;
            m_RecipeDatas.Add(recipe3);

            RecipeData recipe4 = new RecipeData();
            recipe4.Materials.Add(NodeTag.HotMilk);
            recipe4.Product = NodeTag.Latte;
            recipe4.ProductTime = 10f;
            m_RecipeDatas.Add(recipe4);

            RecipeData recipe5 = new RecipeData();
            recipe5.Materials.Add(NodeTag.Cream);
            recipe5.Product = NodeTag.ConPanna;
            recipe5.ProductTime = 10f;
            m_RecipeDatas.Add(recipe5);

            RecipeData recipe6 = new RecipeData();
            recipe6.Materials.Add(NodeTag.Ice);
            recipe6.Product = NodeTag.IceEspresso;
            recipe6.ProductTime = 10f;
            m_RecipeDatas.Add(recipe6);

            RecipeData recipe7 = new RecipeData();
            recipe7.Materials.Add(NodeTag.Sugar);
            recipe7.Product = NodeTag.SweetEspresso;
            recipe7.ProductTime = 10f;
            m_RecipeDatas.Add(recipe7);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (!Input.GetMouseButton(0))
            {
                m_Follow = false;
            }
            if (m_Follow)
            {
                this.transform.position = MouseToWorld(Input.mousePosition);
            }
            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, -8.8f, 8.8f), Mathf.Clamp(this.transform.position.y, -8f, -1.6f), 0);//限制范围
            if (m_AdsorbSlots != null)
            {
                //检测空卡片上的物体是否为空，制作途中拉开卡片可以重置时间和Bar
                foreach (AdsorbSlot slot in m_AdsorbSlots)
                {
                    if (slot.Child == null)
                    {
                        m_ProgressBar.gameObject.SetActive(false);
                        m_ProducingTime = m_NodeData.ProducingTime;
                        m_ProgressBar.transform.SetLocalScaleX(1);
                    }
                }
                m_ChildDatas.Clear();
                m_ChildNodes.Clear();
                for (BaseCompenent child = m_AdsorbSlots[0].Child; child != null; child = child.Child)
                {
                    m_ChildDatas.Add(child);
                    m_ChildNodes.Add(child.NodeTag);
                }
                foreach (RecipeData recipe in m_RecipeDatas)
                {
                    bool flag = true;
                    //获得插槽的儿子的儿子等等

                    /*foreach (AdsorbSlot slot in m_AdsorbSlots)
                    {
                        /*if (slot.Child.Child != null)
                            return;
                        if (!recipe.Materials.Contains(slot.Child.NodeTag))
                            flag = false;
                    }*/
                    if (m_ChildDatas.Count != recipe.Materials.Count)
                        continue;
                    foreach (NodeTag child in recipe.Materials)
                    {
                        if (!m_ChildNodes.Contains(child))
                            flag = false;
                    }
                    if (flag)
                    {
                        m_ProgressBar.gameObject.SetActive(true);
                        m_ProgressBar.transform.SetLocalScaleX(1 - (1 - m_ProducingTime / m_NodeData.ProducingTime));
                        m_ProducingTime -= Time.deltaTime;

                        if (m_ProducingTime <= 0)
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, recipe.Product)
                            {
                                Position = this.transform.position
                            });
                            foreach (AdsorbSlot slot in m_AdsorbSlots)
                            {
                                BaseCompenent baseCompenent;
                                /*BaseCompenent baseCompenent = slot.Child;
                                slot.Child = null;
                                baseCompenent.Remove();*/
                                for (int i = 0; i < m_ChildDatas.Count; i++)
                                {
                                    baseCompenent = m_ChildDatas[i];
                                    baseCompenent.Remove();
                                }
                                m_ChildDatas.Clear();
                            }
                            m_ProducingTime = m_NodeData.ProducingTime;
                            GameEntry.Entity.HideEntity(this.Id);
                        }
                    }
                }
            }
        }
        protected Vector3 MouseToWorld(Vector3 mousePos)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.z = screenPosition.z;
            return Camera.main.ScreenToWorldPoint(mousePos);
        }
        public void OnPointerDown(PointerEventData pointerEventData)
        {
            //Debug.LogFormat("����¼�����Դ��{1}", this.gameObject.name);
            m_Follow = true;
        }
    }
}