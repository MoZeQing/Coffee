using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace GameMain
{
    public class CupNode : BaseCompenent, IPointerDownHandler
    {
        private CompenentData m_CompenentData;
        private NodeData m_NodeData;
        private SpriteRenderer m_SpriteRenderer;
        private BoxCollider2D m_BoxCollider2D;

        private List<BoxCollider2D> m_FilterBoxCollider2DList = new List<BoxCollider2D>();

        private List<BaseCompenent> m_AdsorbNodeList = new List<BaseCompenent>();

        private BaseCompenent m_AdsorbNode;
        private BaseCompenent m_AdsorbNode1;
        private BaseCompenent m_AdsorbNod2;
        private BaseCompenent m_AdsorbNode3;

        private Transform m_ProgressBar = null;
        private float m_ProducingTime = 0f;



        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_CompenentData = (CompenentData)userData;
            m_NodeData = m_CompenentData.NodeData;
            GameEntry.Entity.AttachEntity(this.Id, m_CompenentData.OwnerId);

            //��ȡ����
            IDataTable<DRNode> dtNode = GameEntry.DataTable.GetDataTable<DRNode>();


            DRNode drNode = dtNode.GetDataRow(10);
            m_NodeData.ProducingTime = drNode.ProducingTime;
            m_ProducingTime = m_NodeData.ProducingTime;

            m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
            m_SpriteRenderer.sprite = GameEntry.Utils.nodeSprites[(int)m_NodeData.NodeTag];

            m_BoxCollider2D = this.GetComponent<BoxCollider2D>();
            m_BoxCollider2D.size = m_SpriteRenderer.size;

            m_ProgressBar = this.transform.Find("ProgressBar").transform;//��ȡ������
            m_ProgressBar.gameObject.SetActive(false);

            m_FilterBoxCollider2DList.AddRange(this.transform.Find("Cup").GetComponents<BoxCollider2D>());

            m_AdsorbNodeList.Add(m_AdsorbNode);
            m_AdsorbNodeList.Add(m_AdsorbNode1);
            //m_AdsorbNodeList.Add(m_AdsorbNode2);
            m_AdsorbNodeList.Add(m_AdsorbNode3);

        }

        private bool Contain(NodeTag nodeTag)
        {
            foreach (BaseCompenent item in m_AdsorbNodeList)
            {
                if (item.transform.parent.GetComponent<BaseNode>().NodeData.NodeTag == nodeTag)
                    return true;
            }
            return false;
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
                Follow = false;
            }
            if (Follow)
            {
                this.transform.position = MouseToWorld(Input.mousePosition);
                m_ProgressBar.gameObject.SetActive(false);
                m_ProgressBar.transform.SetLocalScaleX(1);
                m_ProducingTime = 0f;
                m_AdsorbNode = null;
                m_AdsorbNode1 = null;
                Producing = false;
            }
            if (m_AdsorbNode != null && m_AdsorbNode1 != null)
            {
                Debug.Log("������");
                //����Ч��
                //��������㾺��ʱ��Ѱ�����������������
                if (m_AdsorbNode.Follow != false)
                    return;
                if (Contain(NodeTag.HotWater) && Contain(NodeTag.GroundCoffee))
                    return;

                foreach (var item in m_AdsorbNodeList)
                {
                    item.ProducingTool = NodeTag.FilterBowl;
                    item.Producing = true;
                    //item.transform.DOMove(m_FilterBowlBoxCollider2D2.transform.position, 0.1f);
                }
                Producing = true;
                if (Producing)
                {
                    //���������
                    m_ProgressBar.gameObject.SetActive(true);
                    m_ProgressBar.transform.SetLocalScaleX(1 - (1 - m_ProducingTime / m_NodeData.ProducingTime));
                    m_ProducingTime -= Time.deltaTime;

                    Debug.Log(m_ProducingTime);
                    if (m_ProducingTime <= 0)
                    {
                        Producing = false;
                        foreach (var item in m_AdsorbNodeList)
                        {
                            item.Producing = false;
                            item.Completed = true;
                        }
                        if (Contain(NodeTag.HotWater) || Contain(NodeTag.GroundCoffee))
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.CoffeeLiquid)
                            {
                                Position = this.transform.position
                            });
                        }
                    }
                }
            }
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            Follow = true;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            BaseCompenent baseCompenent = null;
            if (collision.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (!baseCompenent.Follow)
                    return;
                //if (!m_FilterBowlBoxCollider2D.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                //    return;
                foreach (var item in m_FilterBoxCollider2DList)
                {
                    if (!item.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                    {
                        return;
                    }
                }
                if (m_FilterBoxCollider2DList[0].IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode = baseCompenent;
                }
                if (m_FilterBoxCollider2DList[1].IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode1 = baseCompenent;
                }
                Debug.Log("��⵽����");
            }
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            BaseCompenent baseCompenent = null;
            if (collision.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (!baseCompenent.Follow)
                    return;
                foreach (var item in m_FilterBoxCollider2DList)
                {
                    if (item.IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                    {
                        return;
                    }
                }
                if (m_FilterBoxCollider2DList[0].IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode = null;
                }
                if (m_FilterBoxCollider2DList[1].IsTouching(baseCompenent.GetComponent<BoxCollider2D>()))
                {
                    m_AdsorbNode1 = null;
                }
                Debug.Log("��⵽�뿪����");

            }
        }

        private Vector3 MouseToWorld(Vector3 mousePos)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.z = screenPosition.z;
            return Camera.main.ScreenToWorldPoint(mousePos);
        }
    }
}
