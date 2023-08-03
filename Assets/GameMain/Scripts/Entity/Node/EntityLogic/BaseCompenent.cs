using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class BaseCompenent : Entity, IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler,IPointerUpHandler
    {
        public bool Follow
        {
            get;
            protected set;
        } = false;
        public BaseCompenent Parent
        {
            get;
            set;
        }
        public BaseCompenent Child
        {
            get;
            set;
        } = null;
        public bool Producing
        {
            get;
            set;
        } = false;
        public bool Touch
        {
            get;
            set;
        } = false;
        public bool Completed
        {
            get;
            set;
        } = false;
        public NodeTag NodeTag
        {
            get;
            private set;
        }
        public NodeState NodeState
        {
            get;
            private set;
        }
        protected SpriteRenderer SpriteRenderer
        {
            get;
            set;
        }
        protected SpriteRenderer Shader
        {
            get;
            set;
        }
        //��ץȡʱ��������ĵ�Ĳ��
        private Vector3 mMouseGap;

        protected List<BaseCompenent>  mCompenents= new List<BaseCompenent>();
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            CompenentData data = (CompenentData)userData;
            NodeTag = data.NodeData.NodeTag;
            SpriteRenderer = this.transform.Find("Sprite").GetComponent<SpriteRenderer>();
            Shader = this.transform.Find("Shader").GetComponent<SpriteRenderer>();
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (!Input.GetMouseButton(0))
            {
                Follow = false;
            }
            switch (NodeState)
            {
                case NodeState.Idle:
                    SpriteRenderer.gameObject.transform.DOLocalMove(Vector3.zero, 0.2f);
                    break;
            }
            if (Follow)
            {
                //��������û�����⣬��������Ҫ��������ƶ���������
                this.transform.DOMove(MouseToWorld(Input.mousePosition) - mMouseGap, 0.05f);
                //this.transform.position=MouseToWorld(Input.mousePosition);
                //���Ƶ��ƶ��Ϳ��Ʊ���������Ч���Ƿ��ڲ�һ���Ĳ㼶�����
                Producing = false;
            }
            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, -8.8f, 8.8f), Mathf.Clamp(this.transform.position.y, -8f, -1.6f), 0);//���Ʒ�Χ
            //if (Parent == null)
            //    SpriteRenderer.sortingOrder = 0;
            if (Parent != null && !Follow)
            {
                this.transform.DOMove(Parent.transform.position+Vector3.up*0.5f, 0.1f);//�����ڵ�
                SpriteRenderer.sortingOrder = Parent.SpriteRenderer.sortingOrder+1;
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
            GameEntry.Sound.PlaySound("Assets/GameMain/Audio/Sounds/Pick_up.mp3", "Sound");

            Follow = true;
            GameEntry.Utils.pickUp = true;
            SpriteRenderer.sortingOrder = GameEntry.Utils.CartSort;
            NodeState = NodeState.PickUp;
            //�������������
            //̧�߿�Ƭ
            mMouseGap = MouseToWorld(Input.mousePosition)-this.transform.position;
            PickUp();
        }
        public void OnPointerUp(PointerEventData pointerEventData)
        {
            Follow = false;
            GameEntry.Utils.pickUp = false;
            NodeState = NodeState.PitchOn;
            PitchOn();
            if (mCompenents.Count == 0)
                return;
            BaseCompenent bestCompenent = mCompenents[0];
            foreach (BaseCompenent baseCompenent in mCompenents)
            {
                if ((baseCompenent.transform.position - this.transform.position).magnitude < (bestCompenent.transform.position - this.transform.position).magnitude)
                { 
                    bestCompenent= baseCompenent;
                }
            }
            mCompenents.Clear();
            Parent = bestCompenent;
            Parent.Child= this;
        }
        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            if (GameEntry.Utils.pickUp)
                return;
            NodeState = NodeState.PitchOn;
            PitchOn();
        }
        public void OnPointerExit(PointerEventData pointerEventData)
        {
            if (GameEntry.Utils.pickUp)
                return;
            NodeState = NodeState.Idle;
            PutDown();
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!Follow)
                return;
            BaseCompenent baseCompenent = null;
            if (!collision.TryGetComponent<BaseCompenent>(out baseCompenent))
                return;
            //�������ѭ��
            BaseCompenent parent = baseCompenent;
            //���������ѭ��
            int block = 1000;
            while (parent != null)
            {
                parent = parent.Parent;
                if (parent == this)
                    return;
                block--;
                if (block < 0)
                    return;
            }

            if (!mCompenents.Contains(baseCompenent))
            {
                mCompenents.Add(baseCompenent);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!Follow) 
                return;
            BaseCompenent baseCompenent = null;
            if (!collision.TryGetComponent<BaseCompenent>(out baseCompenent))
                return;
            if (Parent == baseCompenent)
            {   
                Parent.Child= null;
                Parent = null; 
            }
            if (mCompenents.Contains(baseCompenent))
            {
                mCompenents.Remove(baseCompenent);
            }
        }
        /// <summary>
        /// ����״̬
        /// </summary>
        public void PickUp()
        {
            SpriteRenderer.gameObject.transform.DOLocalMove(Vector3.up * 0.16f, 0.2f);
            Shader.gameObject.transform.DOLocalMove(Vector3.down * 0.08f, 0.2f);
            if (Child != null)
                Child.PickUp();
        }
        /// <summary>
        /// ѡ��
        /// </summary>
        public void PitchOn()
        {
            SpriteRenderer.gameObject.transform.DOLocalMove(Vector3.up * 0.08f, 0.2f);
            Shader.gameObject.transform.DOLocalMove(Vector3.down * 0.04f, 0.2f);
            if (Child != null)
                Child.PitchOn();
        }
        /// <summary>
        /// ����
        /// </summary>
        public void PutDown()
        {
            SpriteRenderer.gameObject.transform.DOLocalMove(Vector3.zero, 0.016f);
            Shader.gameObject.transform.DOLocalMove(Vector3.zero, 0.08f);
            if (Child != null)
                Child.PutDown();
        }
        public void Remove()
        { 
            if(Parent!=null)
                Parent.Child=null;
            if(Child!=null)
                Child.Parent=null;
            GameEntry.Entity.HideEntity(this.transform.parent.GetComponent<BaseNode>().NodeData.Id);
        }
    }

    public enum NodeState
    { 
        //δ����
        InActive,
        //����
        Idle,
        //������
        PickUp,
        //��ѡ��
        PitchOn
    }
}
