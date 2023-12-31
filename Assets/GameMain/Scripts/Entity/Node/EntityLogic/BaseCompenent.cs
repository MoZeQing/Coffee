using DG.Tweening;
using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;

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
        } = false;
        public bool CondensedMilk
        {
            get;
            protected set;
        } = false;
        public bool Salt
        {
            get;
            protected set;
        } = false;

        public int Level
        {
            get;
            protected set;
        }
        public List<NodeTag> Materials 
        {   get; 
            protected set; 
        } = new List<NodeTag>();

        protected SpriteRenderer mSpriteRenderer = null;
        protected SpriteRenderer mShader = null;
        protected SpriteRenderer mProgressBarRenderer = null;
        protected BoxCollider2D mBoxCollider2D = null;

        protected Vector3 mMouseGap;
        protected NodeData mNodeData = null;
        protected CompenentData mCompenentData = null;
        protected Transform mProgressBar = null;
        protected float mLength = 2f;
        protected List<BaseCompenent> mCompenents = new List<BaseCompenent>();
        protected List<NodeTag> mMaterials = new List<NodeTag>();
        protected List<NodeTag> mRecipe = new List<NodeTag>();
        protected List<NodeTag> mProduct = new List<NodeTag>();
        protected List<NodeTag> mCheckRecipe = new List<NodeTag>();
        protected List<NodeTag> mProMaterials = new List<NodeTag>();
        protected NodeTag tool = NodeTag.None;
        protected float mProducingTime = 0f;
        protected float mTime = 0f;
        protected DRRecipe drRecipe = null;

        private SpriteRenderer mCondensedMilkPoint = null;
        private SpriteRenderer mSugarPoint = null;
        private SpriteRenderer mSaltPoint = null;

        private SpriteRenderer mRangerC = null;
        private SpriteRenderer mRangerB = null;
        private SpriteRenderer mRangerA = null;
        private SpriteRenderer mRangerS = null;

        private float mAddMaterialsTime = 0f;
        private float mAddTime = 0f;
        private bool flag = false;
        private int mLevel = 0;
        private int mEspressoLevel = 0;
        private bool isCoffee = false;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            mCompenentData = (CompenentData)userData;
            mNodeData = mCompenentData.NodeData;
            Materials = mCompenentData.materials;
            NodeTag = mCompenentData.NodeData.NodeTag;
            mSpriteRenderer = this.transform.Find("Sprite").GetComponent<SpriteRenderer>();
            mSpriteRenderer.sortingLayerName = "GamePlay";
            mShader = this.transform.Find("Shader").GetComponent<SpriteRenderer>();
            mProgressBar = this.transform.Find("ProgressBar").GetComponent<Transform>();
            mProgressBarRenderer = this.transform.Find("ProgressBar").GetComponent<SpriteRenderer>();

            mBoxCollider2D = this.GetComponent<BoxCollider2D>();

            mCondensedMilkPoint = mSpriteRenderer.gameObject.transform.Find("CondensedMilk").GetComponent<SpriteRenderer>();
            mSugarPoint = mSpriteRenderer.gameObject.transform.Find("Sugar").GetComponent<SpriteRenderer>();
            mSaltPoint = mSpriteRenderer.gameObject.transform.Find("Salt").GetComponent<SpriteRenderer>();

            mRangerC = mSpriteRenderer.gameObject.transform.Find("RangeC").GetComponent<SpriteRenderer>();
            mRangerB = mSpriteRenderer.gameObject.transform.Find("RangeB").GetComponent<SpriteRenderer>();
            mRangerA = mSpriteRenderer.gameObject.transform.Find("RangeA").GetComponent<SpriteRenderer>();
            mRangerS = mSpriteRenderer.gameObject.transform.Find("RangeS").GetComponent<SpriteRenderer>();

            mAddMaterialsTime = 5f;
            Level = mNodeData.MLevel;

            Materials = mNodeData.M_Materials;
            GameEntry.Entity.AttachEntity(this.Id, mCompenentData.OwnerId);
        }
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            //if (mNodeData.Follow)
            //{
            //    GameEntry.Utils.pickUp = true;
            //    mBoxCollider2D.isTrigger = true;
            //    mShader.sortingOrder = GameEntry.Utils.CartSort;
            //    mSpriteRenderer.sortingOrder = GameEntry.Utils.CartSort;
            //    firstFollow = true;
            //    this.transform.position = MouseToWorld(Input.mousePosition);
            //    mMouseGap = Vector3.zero;
            //    PickUp();
            //}
            mCompenentData = (CompenentData)userData;
            mNodeData = mCompenentData.NodeData;
            Materials = mCompenentData.materials;
            NodeTag = mCompenentData.NodeData.NodeTag;

            Salt = false;
            Sugar= false;
            CondensedMilk= false;

            mRangerC.gameObject.SetActive(false);
            mRangerB.gameObject.SetActive(false);
            mRangerA.gameObject.SetActive(false);
            mRangerS.gameObject.SetActive(false);

            mSpriteRenderer.sprite = Resources.Load<Sprite>(GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)mNodeData.NodeTag).SpritePath);

            mCondensedMilkPoint.gameObject.SetActive(false);
            mSugarPoint.gameObject.SetActive(false);
            mSaltPoint.gameObject.SetActive(false);
            GameEntry.Entity.AttachEntity(this.Id, mCompenentData.OwnerId);
            this.transform.position = mNodeData.Position;
            if (mNodeData.RamdonJump)
            {
                Vector3 newPos = UnityEngine.Random.insideUnitCircle;
                this.transform.DOMove(mNodeData.Position + newPos * 2f, 0.5f).SetEase(Ease.OutExpo);
            }
            if (mNodeData.Jump)
            {
                Vector3 newPos = -(mNodeData.Position - Vector3.down * 4.2f).normalized;
                this.transform.DOMove(mNodeData.Position + newPos * 3f, 0.5f).SetEase(Ease.OutExpo);
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
                mBoxCollider2D.offset = new Vector2(0, 0.04449272f);
            }
            if (Child != null)
            {
                mBoxCollider2D.size = new Vector2(1.36f, 0.47594f);
                mBoxCollider2D.offset = new Vector2(0f, -0.7279919f);
            }

            if (!Input.GetMouseButton(0))
            {
                mNodeData.Follow = false;
                GameEntry.Utils.pickUp = false;
                /*if (firstFollow)
                
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

                    BaseCompenent parent = bestCompenent;

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
                    firstFollow = false;
                }*/
            }
            if (mNodeData.Follow)
            {
                this.transform.DOMove(MouseToWorld(Input.mousePosition) - mMouseGap, 0.05f);
                //this.transform.position=MouseToWorld(Input.mousePosition);
                Producing = false;
                tool = NodeTag.None;
                mProducingTime = 0;
                mTime = 0f;
                mRecipe.Clear();
                mProduct.Clear();
                mCheckRecipe.Clear();
                mProgressBar.gameObject.SetActive(false);
                mProgressBar.transform.SetLocalScaleX(1);
            }

            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, -8f, 8f), Mathf.Clamp(this.transform.position.y, -10f, -4f), 0);//���Ʒ�Χ
            //if (Parent == null)
            //    SpriteRenderer.sortingOrder = 0;
            if (Parent != null && !mNodeData.Follow)
            {
                this.transform.DOMove(Parent.transform.position + Vector3.up * 0.5f, 0.1f);//�����ڵ�
            }

            mMaterials = GenerateMaterialList();
            Compound();
            if(mNodeData.IsCoffee==true)
            {
                AddMaterials();
            }
            for (int i = 0; i < Materials.Count; i++)
            {
                Debug.Log(Materials[i]);
            }
            ShowMyLevel();
            for (int i = 0; i < Materials.Count; i++)
            {
                Debug.Log(Materials[i]);
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

            mMouseGap = MouseToWorld(Input.mousePosition) - this.transform.position;
            PickUp();
        }
        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            if (Parent != null)
            {
                Parent = null;
            }
            if (Child != null)
            {
                Child = null;
            }
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

            BaseCompenent parent = bestCompenent;

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
            if(bestCompenent.Child==null)
            {
                Parent = bestCompenent;
                Parent.Child = this;
            }
           
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
            GameEntry.Entity.HideEntity(mNodeData.Id);
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
            mProgressBarRenderer.sortingOrder = mSpriteRenderer.sortingOrder + 1;
            mProgressBarRenderer.sortingLayerName = mSpriteRenderer.sortingLayerName;
            if (!Producing)
            {
                for (int i = 0; i < GameEntry.DataTable.GetDataTable<DRRecipe>().Count; i++)
                {
                    drRecipe = GameEntry.DataTable.GetDataTable<DRRecipe>().GetDataRow(i);
                    if (!GameEntry.Player.HasRecipe(drRecipe.Id))
                        continue;

                    mRecipe = TransToEnumList(drRecipe.Recipe);
                    tool = TransToEnum(drRecipe.Tool);
                    if (Parent == null && Child != null)
                    {
                        if (NodeTag == tool)
                        {
                            if (mRecipe.SequenceEqual(mMaterials))
                            {
                                Producing = true;
                                mProduct = TransToEnumList(drRecipe.Product);
                                mProMaterials = TransToEnumList(drRecipe.Materials);
                                mCheckRecipe = mRecipe;
                                mProducingTime = drRecipe.ProducingTime;
                                mTime = drRecipe.ProducingTime;
                                mLevel = drRecipe.CoffeeLevel;
                                isCoffee = drRecipe.IsCoffee;
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
                if (mCheckRecipe.Count != mMaterials.Count)
                {
                    tool = NodeTag.None;
                    mProducingTime = 0;
                    mTime = 0f;
                    mRecipe.Clear();
                    mProduct.Clear();
                    mCheckRecipe.Clear();
                    mProgressBar.gameObject.SetActive(false);
                    mProgressBar.transform.SetLocalScaleX(1);
                    Producing = false;
                    return;
                }
                if (!(Parent == null && Child != null))
                {
                    tool = NodeTag.None;
                    mProducingTime = 0;
                    mTime = 0f;
                    mRecipe.Clear();
                    mProduct.Clear();
                    mCheckRecipe.Clear();
                    mProgressBar.gameObject.SetActive(false);
                    mProgressBar.transform.SetLocalScaleX(1);
                    Producing = false;
                    return;
                }

                if (mProducingTime <= 0)
                {
                    for (int i = 0; i < mProduct.Count; i++)
                    {
                        if (mProduct[i] == NodeTag.Espresso)
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, mProduct[i],mLevel,true,mProMaterials)
                            {
                                Position = this.transform.position + new Vector3(0.5f, 0, 0),
                                RamdonJump = true
                            });
                        }
                        else if(isCoffee)
                        {
                            FindMyEspressoLevel();
                            GenerateCoffeeLevel();
                            MixMaterials();
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, mProduct[i], mLevel,true,mProMaterials)
                            {
                                Position = this.transform.position + new Vector3(0.5f, 0, 0),
                                RamdonJump = true
                            });
                        }
                        else
                        {
                            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, mProduct[i])
                            {
                                Position = this.transform.position + new Vector3(0.5f, 0, 0),
                                RamdonJump = true
                            });
                        }
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
                    Materials.Clear();
                    if (this.NodeTag == NodeTag.Cup)
                    {
                        this.Remove();
                    }
                    tool = NodeTag.None;
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

        public void GenerateCoffeeLevel()
        {
            if (LuckyDraw(1000, 5))
            {
                mLevel = 4;
            }
            else
            {
                if (mLevel==1)
                {
                    if (LuckyDraw(100, 5))
                    {
                        if (LuckyDraw(100, 2))
                        {
                            mLevel = 3;
                        }
                        else
                        {
                            mLevel = 2;
                        }
                    }
                    else
                    {
                        mLevel = 1;
                    }
                }
                else if (mLevel == 2)
                {
                    if (LuckyDraw(100, 2))
                    {
                        mLevel = 3;

                    }
                    else
                    {
                        mLevel = 2;
                    }
                }
                else if (mLevel == 3)
                {
                    mLevel = 3;
                }
            }
        }
        public void AddMaterials()
        {
            if(Child!=null)
            {
                mCondensedMilkPoint.sortingOrder = Child.mSpriteRenderer.sortingOrder - 1;
                mSaltPoint.sortingOrder = Child.mSpriteRenderer.sortingOrder - 1;
                mSugarPoint.sortingOrder = Child.mSpriteRenderer.sortingOrder - 1;
            }
            if(Child==null)
            {
                mCondensedMilkPoint.sortingOrder = mSpriteRenderer.sortingOrder + 1;
                mSaltPoint.sortingOrder = mSpriteRenderer.sortingOrder + 1;
                mSugarPoint.sortingOrder = mSpriteRenderer.sortingOrder + 1;
            }
            mCondensedMilkPoint.sortingLayerName = mSpriteRenderer.sortingLayerName;
            mSaltPoint.sortingLayerName = mSpriteRenderer.sortingLayerName;
            mSugarPoint.sortingLayerName = mSpriteRenderer.sortingLayerName;
            if (Parent == null && Child != null && Child.Child == null && flag == false)
            {
                if (Child.NodeTag == NodeTag.CondensedMilk)
                {
                    flag = true;
                    CondensedMilk = true;
                    mAddTime = mAddMaterialsTime;
                }
                else if (Child.NodeTag == NodeTag.Sugar)
                {
                    flag = true;
                    Sugar = true;
                    mAddTime = mAddMaterialsTime;
                }
                else if (Child.NodeTag == NodeTag.Salt)
                {
                    flag = true;
                    Salt = true;
                    mAddTime = mAddMaterialsTime;
                }
            }
            if (flag == true)
            {
                if (CondensedMilk ==true&&!mCondensedMilkPoint.gameObject.activeSelf)
                {
                    mProgressBar.gameObject.SetActive(true);
                    mProgressBar.transform.SetLocalScaleX(1 - (1 - mAddTime / mAddMaterialsTime));
                    mAddTime -= Time.deltaTime;
                    if (Parent != null || Child == null||Child.Child!=null)
                    {
                        flag = false;
                        CondensedMilk = false;
                        mProgressBar.gameObject.SetActive(false);
                        mProgressBar.transform.SetLocalScaleX(1);
                        return;
                    }
                    if (mAddTime <= 0)
                    {
                        mProgressBar.gameObject.SetActive(false);
                        mCondensedMilkPoint.gameObject.SetActive(true);
                        if (Child != null)
                        {
                            Child.Remove();
                        }
                        CondensedMilk = true;
                        flag = false;
                    }
                }
                else if (Sugar==true&&!mSugarPoint.gameObject.activeSelf)
                {
                    mProgressBar.gameObject.SetActive(true);
                    mProgressBar.transform.SetLocalScaleX(1 - (1 - mAddTime / mAddMaterialsTime));
                    mAddTime -= Time.deltaTime;
                    if (Parent != null || Child == null || Child.Child != null)
                    {
                        flag = false;
                        Sugar = false; ;
                        mProgressBar.gameObject.SetActive(false);
                        mProgressBar.transform.SetLocalScaleX(1);
                        return;
                    }
                    if (mAddTime <= 0)
                    {
                        mProgressBar.gameObject.SetActive(false);
                        mSugarPoint.gameObject.SetActive(true);
                        if (Child != null)
                        {
                            Child.Remove();
                        }
                        Sugar = true;
                        flag = false;
                    }
                }
                else if (Salt == true && !mSaltPoint.gameObject.activeSelf)
                {
                    mProgressBar.gameObject.SetActive(true);
                    mProgressBar.transform.SetLocalScaleX(1 - (1 - mAddTime / mAddMaterialsTime));
                    mAddTime -= Time.deltaTime;
                    if (Parent != null || Child == null || Child.Child != null)
                    {
                        flag = false;
                        Salt = false; ;
                        mProgressBar.gameObject.SetActive(false);
                        mProgressBar.transform.SetLocalScaleX(1);
                        return;
                    }
                    if (mAddTime <= 0)
                    {
                        mProgressBar.gameObject.SetActive(false);
                        mSaltPoint.gameObject.SetActive(true);
                        if (Child != null)
                        {
                            Child.Remove();
                        }
                        Salt = true;
                        flag = false;
                    }
                }
            }
        }
        public bool LuckyDraw(int rangeNum, int checkNum)
        {
            int randomNum = 0;
            randomNum = UnityEngine.Random.Range(0, rangeNum);
            if (randomNum < checkNum)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void MixMaterials()
        {
            BaseCompenent child = Child;
            while (child != null)
            {
                if (child.NodeTag == NodeTag.Espresso)
                {
                    for (int i = 0; i < child.Materials.Count; i++)
                    {
                        mProMaterials.Add(child.Materials[i]);
                    }
                }
                child = child.Child;
            }
        }
        public void FindMyEspressoLevel()
        {
            BaseCompenent child = Child;
            List<int> levelList = new List<int>();
            while (child != null)
            {
                if(child.NodeTag==NodeTag.Espresso)
                {
                    levelList.Add(child.Level);
                }
                child = child.Child;
            }
            levelList.Sort();
            mLevel = levelList[0];
        }

        public void ShowMyLevel()
        {
            if (Child != null)
            {
                mRangerC.sortingOrder = Child.mSpriteRenderer.sortingOrder - 1;
                mRangerB.sortingOrder = Child.mSpriteRenderer.sortingOrder - 1;
                mRangerA.sortingOrder = Child.mSpriteRenderer.sortingOrder - 1;
                mRangerS.sortingOrder = Child.mSpriteRenderer.sortingOrder - 1;
            }
            if (Child == null)
            {
                mRangerC.sortingOrder = mSpriteRenderer.sortingOrder + 1;
                mRangerB.sortingOrder = mSpriteRenderer.sortingOrder + 1;
                mRangerA.sortingOrder = mSpriteRenderer.sortingOrder + 1;
                mRangerS.sortingOrder = mSpriteRenderer.sortingOrder + 1;
            }
            mRangerC.sortingLayerName = mSpriteRenderer.sortingLayerName;
            mRangerB.sortingLayerName = mSpriteRenderer.sortingLayerName;
            mRangerA.sortingLayerName = mSpriteRenderer.sortingLayerName;
            mRangerS.sortingLayerName = mSpriteRenderer.sortingLayerName;
            if (Level==1)
            {
                mRangerC.gameObject.SetActive(true);
                mRangerB.gameObject.SetActive(false);
                mRangerA.gameObject.SetActive(false);
                mRangerS.gameObject.SetActive(false);
            }
            else if (Level == 2)
            {
                mRangerC.gameObject.SetActive(false);
                mRangerB.gameObject.SetActive(true);
                mRangerA.gameObject.SetActive(false);
                mRangerS.gameObject.SetActive(false);
            }
            else if (Level == 3)
            {
                mRangerC.gameObject.SetActive(false);
                mRangerB.gameObject.SetActive(false);
                mRangerA.gameObject.SetActive(true);
                mRangerS.gameObject.SetActive(false);
            }
            else if (Level == 4)
            {
                mRangerC.gameObject.SetActive(false);
                mRangerB.gameObject.SetActive(false);
                mRangerA.gameObject.SetActive(false);
                mRangerS.gameObject.SetActive(true);
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


