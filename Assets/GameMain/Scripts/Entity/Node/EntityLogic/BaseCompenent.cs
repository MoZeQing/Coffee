using DG.Tweening;
using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class BaseCompenent : Entity, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
    {
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
        public NodeTag NodeTag
        {
            get;
            protected set;
        }
        public bool Sugar
        {
            get;
            protected set;
        }
        public bool CondensedMilk
        {
            get;
            protected set;
        }
        public bool Salt
        {
            get;
            protected set;
        }
        public List<NodeTag> Materials { get; protected set; } = new List<NodeTag>();

        protected SpriteRenderer mSpriteRenderer = null;
        protected SpriteRenderer mShader = null;
        protected SpriteRenderer mProgressBarRenderer = null;
        protected BoxCollider2D mBoxCollider2D = null;
        //��ץȡʱ��������ĵ�Ĳ��
        protected Vector3 mMouseGap;
        protected NodeData mNodeData = null;
        protected CompenentData mCompenentData = null;
        protected Transform mProgressBar = null;
        protected float mLength = 2f;
        protected List<BaseCompenent> mCompenents = new List<BaseCompenent>();
        protected IDataTable<DRRecipe> dtRecipe = GameEntry.DataTable.GetDataTable<DRRecipe>();
        protected List<NodeTag> mMaterials = new List<NodeTag>();
        protected List<NodeTag> mRecipe = new List<NodeTag>();
        protected List<NodeTag> mProduct = new List<NodeTag>();
        protected List<NodeTag> mcheckRecipe = new List<NodeTag>();
        protected NodeTag Tool = NodeTag.None;
        protected float mProducingTime = 0f;
        protected float mTime = 0f;
        protected DRRecipe drRecipe = null;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            mCompenentData = (CompenentData)userData;
            mNodeData = mCompenentData.NodeData;
            Materials = mCompenentData.materials;

            NodeTag = mCompenentData.NodeData.NodeTag;
            mSpriteRenderer = this.transform.Find("Sprite").GetComponent<SpriteRenderer>();
            mSpriteRenderer.size = new Vector2(1.6f, 2.7f);
            mShader = this.transform.Find("Shader").GetComponent<SpriteRenderer>();
            mProgressBar = this.transform.Find("ProgressBar").GetComponent<Transform>();
            mProgressBarRenderer = this.transform.Find("ProgressBar").GetComponent<SpriteRenderer>();

            mBoxCollider2D = this.GetComponent<BoxCollider2D>();
            mBoxCollider2D.size = mSpriteRenderer.size;

            if (mNodeData.Follow)
            {
                GameEntry.Utils.pickUp = true;
                mBoxCollider2D.isTrigger = true;
                mShader.sortingOrder = GameEntry.Utils.CartSort;
                mSpriteRenderer.sortingOrder = GameEntry.Utils.CartSort;

                this.transform.position = MouseToWorld(Input.mousePosition);
                mMouseGap = Vector3.zero;
                PickUp();
            }
            if (mNodeData.Jump)
            {
                Vector3 newPos = (Vector3)UnityEngine.Random.insideUnitCircle;
                this.transform.DOMove(mNodeData.Position + newPos * mLength, 0.5f).SetEase(Ease.OutExpo);
            }
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (Parent != null)
            {
                mBoxCollider2D.isTrigger = true;

            }
            if (Child == null)
            {
                mBoxCollider2D.size = mSpriteRenderer.size;
                mBoxCollider2D.offset = new Vector2(0f, 0f);
            }
            if (Child != null)
            {
                mBoxCollider2D.size = new Vector2(1.6f, 0.45f);
                mBoxCollider2D.offset = new Vector2(0f, -1.1f);
            }

            if (!Input.GetMouseButton(0))
            {
                mNodeData.Follow = false;
                GameEntry.Utils.pickUp = false;
            }
            if (mNodeData.Follow)
            {
                //��������û�����⣬��������Ҫ��������ƶ���������
                this.transform.DOMove(MouseToWorld(Input.mousePosition) - mMouseGap, 0.05f);
                //this.transform.position=MouseToWorld(Input.mousePosition);
                //���Ƶ��ƶ��Ϳ��Ʊ���������Ч���Ƿ��ڲ�һ���Ĳ㼶�����
                Producing = false;
                Tool = NodeTag.None;
                mProducingTime = 0;
                mTime = 0f;
                mRecipe.Clear();
                mProduct.Clear();
                mcheckRecipe.Clear();
                mProgressBar.gameObject.SetActive(false);
                mProgressBar.transform.SetLocalScaleX(1);
            }

            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, -8.8f, 8.8f), Mathf.Clamp(this.transform.position.y, -8f, -1.6f), 0);//���Ʒ�Χ
            //if (Parent == null)
            //    SpriteRenderer.sortingOrder = 0;
            if (Parent != null && !mNodeData.Follow)
            {
                this.transform.DOMove(Parent.transform.position + Vector3.up * 0.5f, 0.1f);//�����ڵ�
            }

            mMaterials = GenerateMaterialList();
            Compound();
            mProgressBarRenderer.sortingOrder = mSpriteRenderer.sortingOrder + 1;
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

            if (Parent != null)
            {
                Parent.Child = null;
                Parent = null;
            }
            mNodeData.Follow = true;
            GameEntry.Utils.pickUp = true;
            mBoxCollider2D.isTrigger = true;
            mShader.sortingOrder = GameEntry.Utils.CartSort;
            mSpriteRenderer.sortingOrder = GameEntry.Utils.CartSort;
            mSpriteRenderer.sortingLayerName = "Controller";
            mShader.sortingLayerName = "Controller";
            //�������������
            //̧�߿�Ƭ
            mMouseGap = MouseToWorld(Input.mousePosition) - this.transform.position;
            PickUp();
        }
        public void OnPointerUp(PointerEventData pointerEventData)
        {
            mSpriteRenderer.sortingLayerName = "GamePlay";
            mShader.sortingLayerName = "GamePlay";
            mBoxCollider2D.isTrigger = false;
            PitchOn();
            if (mCompenents.Count == 0)
                return;
            if (Parent != null)
                return;

            BaseCompenent bestCompenent = mCompenents[0];
            foreach (BaseCompenent baseCompenent in mCompenents)
            {
                if ((baseCompenent.transform.position - this.transform.position).magnitude < (bestCompenent.transform.position - this.transform.position).magnitude)
                {
                    if (baseCompenent.Child != null)
                        continue;
                    bestCompenent = baseCompenent;
                }
            }
            mCompenents.Clear();

            //�������ѭ��
            BaseCompenent parent = bestCompenent;
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
            Parent = bestCompenent;
            Parent.Child = this;
        }
        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            if (GameEntry.Utils.pickUp)
                return;
            if (Parent != null)
                return;
            PitchOn();
        }
        public void OnPointerExit(PointerEventData pointerEventData)
        {
            if (GameEntry.Utils.pickUp)
                return;
            if (Parent != null)
                return;
            PutDown();
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!mNodeData.Follow)
                return;
            BaseCompenent baseCompenent = null;
            if (!collision.TryGetComponent<BaseCompenent>(out baseCompenent))
                return;
            if (!mCompenents.Contains(baseCompenent))
            {
                mCompenents.Add(baseCompenent);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!mNodeData.Follow)
                return;
            BaseCompenent baseCompenent = null;
            if (!collision.TryGetComponent<BaseCompenent>(out baseCompenent))
                return;
            if (Parent == baseCompenent)
            {
                Parent.Child = null;
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
            mSpriteRenderer.gameObject.transform.DOPause();
            mShader.gameObject.transform.DOPause();
            mSpriteRenderer.gameObject.transform.DOLocalMove(Vector3.up * 0.16f, 0.2f);
            mShader.gameObject.transform.DOLocalMove(Vector3.down * 0.08f, 0.2f);
            mShader.sortingOrder = -99;
            mSpriteRenderer.sortingOrder = GameEntry.Utils.CartSort;
            mSpriteRenderer.sortingLayerName = "Controller";
            mShader.sortingLayerName = "Controller";
            if (Child != null)
                Child.PickUp();
        }
        /// <summary>
        /// ѡ��
        /// </summary>
        public void PitchOn()
        {
            mSpriteRenderer.gameObject.transform.DOPause();
            mShader.gameObject.transform.DOPause();
            mSpriteRenderer.gameObject.transform.DOLocalMove(Vector3.up * 0.08f, 0.2f);
            mShader.gameObject.transform.DOLocalMove(Vector3.down * 0.04f, 0.2f);
            mShader.sortingOrder = -99;
            mSpriteRenderer.sortingOrder = GameEntry.Utils.CartSort;
            mSpriteRenderer.sortingLayerName = "GamePlay";
            mShader.sortingLayerName = "GamePlay";
            if (Child != null)
                Child.PitchOn();
        }
        /// <summary>
        /// ����
        /// </summary>
        public void PutDown()
        {
            mSpriteRenderer.gameObject.transform.DOPause();
            mShader.gameObject.transform.DOPause();
            mSpriteRenderer.gameObject.transform.DOLocalMove(Vector3.zero, 0.016f);
            mShader.gameObject.transform.DOLocalMove(Vector3.zero, 0.08f);
            mShader.sortingOrder = -99;
            mSpriteRenderer.sortingOrder = GameEntry.Utils.CartSort;
            mSpriteRenderer.sortingLayerName = "GamePlay";
            mShader.sortingLayerName = "GamePlay";
            if (Child != null)
                Child.PutDown();
        }
        public void Remove()
        {
            if (Parent != null)
                Parent.Child = null;
            if (Child != null)
                Child.Parent = null;
            GameEntry.Entity.HideEntity(this.transform.parent.GetComponent<BaseNode>().NodeData.Id);
        }
        public NodeTag TransToEnum(string value)
        {
            return (NodeTag)Enum.Parse(typeof(NodeTag), value);
        }

        public List<NodeTag> TransToEnumList(List<string> valueList)
        {
            List<NodeTag> temp = new List<NodeTag>();
            foreach (var VarIAble in valueList)
            {
                temp.Add((NodeTag)Enum.Parse(typeof(NodeTag), VarIAble));
            }
            return temp;
        }
        public void Compound()
        {
            if (!Producing)
            {
                for (int i = 0; i < 8; i++)
                {
                    drRecipe = dtRecipe.GetDataRow(i);
                    mRecipe = TransToEnumList(drRecipe.Recipe);
                    Tool = TransToEnum(drRecipe.Tool);
                    if (Parent == null && Child != null)
                    {
                        if (NodeTag == Tool)
                        {
                            if (mRecipe.SequenceEqual(mMaterials))
                            {
                                Producing = true;
                                mProduct = TransToEnumList(drRecipe.Product);
                                mcheckRecipe = mRecipe;
                                mProducingTime = drRecipe.ProducingTime;
                                mTime = drRecipe.ProducingTime;
                            }
                        }
                    }
                }
            }
            else
            {
                mProgressBar.gameObject.SetActive(true);
                mProgressBar.transform.SetLocalScaleX(1 - (1 - mProducingTime / mTime));
                mProducingTime -= Time.deltaTime;
                if (mcheckRecipe.Count != mMaterials.Count)
                {
                    Tool = NodeTag.None;
                    mProducingTime = 0;
                    mTime = 0f;
                    mRecipe.Clear();
                    mProduct.Clear();
                    mcheckRecipe.Clear();
                    mProgressBar.gameObject.SetActive(false);
                    mProgressBar.transform.SetLocalScaleX(1);
                    Producing = false;
                    return;
                }
                if (!(Parent == null && Child != null))
                {
                    Tool = NodeTag.None;
                    mProducingTime = 0;
                    mTime = 0f;
                    mRecipe.Clear();
                    mProduct.Clear();
                    mcheckRecipe.Clear();
                    mProgressBar.gameObject.SetActive(false);
                    mProgressBar.transform.SetLocalScaleX(1);
                    Producing = false;
                    return;
                }

                if (mProducingTime <= 0)
                {


                    for (int i = 0; i < mProduct.Count; i++)
                    {
                        Debug.Log(mProduct.Count);
                        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, mProduct[i])
                        {
                            Position = this.transform.position,
                            Follow = true
                        });
                    }
                    if(this.NodeTag==NodeTag.Cup)
                    {
                        this.Remove();
                    }
                    if (Child != null)
                    {
                        List<BaseCompenent> mMaterialBaseCompenet = new List<BaseCompenent>();
                        BaseCompenent child = Child;
                        while (child != null)
                        {
                            mMaterialBaseCompenet.Add(child);
                            child = child.Child;
                        }
                        for (int i = 0; i < mMaterialBaseCompenet.Count; i++)
                        {
                            mMaterialBaseCompenet[i].Remove();
                        }
                    }
                    Tool = NodeTag.None;
                    mProducingTime = 0;
                    mTime = 0f;
                    mRecipe.Clear();
                    mProduct.Clear();
                    mProgressBar.gameObject.SetActive(false);
                    Producing = false;
                    return;
                }
            }
        }
        public List<NodeTag> GenerateMaterialList()
        {
            List<NodeTag> Material = new List<NodeTag>();
            BaseCompenent child = Child;
            while (child != null)
            {
                Material.Add(child.NodeTag);
                child = child.Child;
            }
            return Material;
        }

        public void GenerateCoffeeLevel(CoffeeLevel coffeeLevel, NodeTag nodeTag)
        {
            if(LuckyDraw(1000,5))
            {

            }
            else
            {
                if(coffeeLevel==CoffeeLevel.C)
                {
                    if(LuckyDraw(100, 5))
                    {
                        if(LuckyDraw(100, 2))
                        {

                        }
                        else
                        {

                        }
                    }
                    else
                    {

                    }
                }
                else if(coffeeLevel == CoffeeLevel.B)
                {
                    if (LuckyDraw(100, 2))
                    {

                    }
                    else
                    {

                    }
                }
                else if (coffeeLevel == CoffeeLevel.A)
                {

                }
            }
        }
        public bool LuckyDraw(int rangeNum, int checkNum)
        {
            int randomNum = 0;
            randomNum = UnityEngine.Random.Range(0, rangeNum);
            if (randomNum<checkNum)
            {
                return true;
            }
            else
            {
                return false;
            }
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
