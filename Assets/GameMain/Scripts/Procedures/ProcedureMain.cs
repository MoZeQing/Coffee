using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Procedure;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;

namespace GameMain
{
    public class ProcedureMain : ProcedureBase
    {
        private List<LevelData> m_LevelDatas = new List<LevelData>();
        private LevelData m_LevelData = null;
        private MaterialData mMaterialData = new MaterialData();

        private int mDay = 1;//��������
        private int mIndex = 0;//���ڹؿ���
        private bool m_BackGame = false;
        private bool m_ChangeDay = false;

        private MainState mMainState;
        private OrderManager mOrderManager;
        private OrderData mOrderData = null;
        //UI
        private TeachingForm mTeachingForm = null;
        private DialogForm mDialogForm = null;
        private MainForm mMainForm = null;
        private WorkForm mWorkForm = null;

        private Dictionary<string,bool> m_LoadingFlag= new Dictionary<string,bool>();

        public MainForm MainForm
        {
            get;
            set;
        }

        public void BackGame()
        { 
            m_BackGame=true;
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            // ��ԭ��Ϸ�ٶ�
            GameEntry.Base.ResetNormalGameSpeed();


            //��ʼ����Ϣ
            InitMain();

            GameEntry.Event.Subscribe(DialogEventArgs.EventId, DialogEvent);
            
            if(m_LevelData != null)
            {
                m_LevelData = null;
            }
            if(mDay != 1)
            {
                mDay = 1;
            }
            if (mIndex != 0)
            {
                mIndex = 0;
            }
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(2);
            //����������
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", 2.ToString());
                return;
            }
            Debug.Log("Start Load Scene");

            GameEntry.UI.CloseAllLoadedUIForms();
            GameEntry.Entity.HideAllLoadedEntities();
            GameEntry.Sound.StopAllLoadingSounds();
            GameEntry.Sound.StopAllLoadedSounds();

            GameEntry.Event.Unsubscribe(DialogEventArgs.EventId, DialogEvent);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (m_BackGame)
            {
                ChangeState<ProcedureMenu>(procedureOwner);
            }

            foreach (KeyValuePair<string, bool> loadedFlag in m_LoadingFlag)
            {
                if (!loadedFlag.Value)
                {
                    return;
                }
            }
        }
        private LevelData GetRandomLevel()
        {
            LevelData levelData = new LevelData();
            int total = Mathf.Clamp(Random.Range(0, mDay), 1, 3);
            for (int i = 0; i < total; i++)
            {
                int random = Random.Range(1, 6);
                switch (random)
                {
                    case 1:
                        levelData.OrderData.Latte++;
                        break;
                    case 2:
                        levelData.OrderData.WhiteCoffee++;
                        break;
                    case 3:
                        levelData.OrderData.ConPanna++;
                        break;
                    case 4:
                        levelData.OrderData.CafeAmericano++;
                        break;
                    case 5:
                        levelData.OrderData.Espresso++;
                        break;
                    case 6:
                        levelData.OrderData.Mocha++;
                        break;
                }
            }
            return levelData;
        }
        private void DialogEvent(object sender,GameEventArgs e)
        {
            DialogEventArgs dialog = (DialogEventArgs)e;
            switch (mMainState)
            {
                case MainState.Dialog:
                    mMainState= MainState.Teach;
                    break;
                case MainState.Text:
                    mMainState = MainState.Teach;
                    break;
                case MainState.Foreword:
                    mMainState = MainState.Game;
                    break;
            }
            UpdateLevel();
        }
        private void UpdateLevel()
        {
            switch (mMainState)
            {
                //��Ϸ�׶�
                case MainState.Foreword:
                    break;
                case MainState.Game:
                    break;
                case MainState.Settle:
                    Settle();
                    break;
                case MainState.Text:
                    break;
                //���ɽ׶�
                //����Ϸ���ڽ���ʱ�Զ��������ɻ��ڣ���ѡ��˯��ʱ�˳����ɽ׶�
                case MainState.Change:
                    break;
                case MainState.Teach:
                    break;
                case MainState.Dialog:
                    break;
            }
            GameEntry.Event.FireNow(this, LevelEventArgs.Create(mMainState, m_LevelData));
        }
        /// <summary>
        /// ����(���㷽������ɵı�������ʱ��Ľ�����С�ѵĽ���)
        /// </summary>
        /// ��ɱ���*ʱ�佱��*����+С��
        /// ���е�ʱ�佱���ǵ����ʱ�����޶�ʱ���
        /*
         * С�ѣ�
         * С��ֻ�ڷ���Ҫ�����������Ҳ����������èè��ľ����и���
         * С�ѵı���Ϊ���ȷ��õ� 0.01����0.1 ��������ȡ��
         * 
         * ʱ�佱���ͳͷ���
         * ʱ�佱����ΪԼ�����׶Σ��ּ���һ�����ȵ���ʱΪ60s
         * ʱ���Ϊ���ҵ������׶� |----------|----------|----------|----...
         *                      0          30         60         90  /s
         *                        ��ǰ���    ׼ʱ���    ��ʱ���
         *                        1.3����+0.3�� 1����0��  0.5����-0.5��/���ۣ�����ȡ��
         */
        private void Settle()
        {
            float a = mOrderData.GetValue();
            float b = m_LevelData.OrderData.GetValue();
            float c = mOrderData.OrderTime;
            float d = m_LevelData.OrderData.OrderTime;
            float e = mOrderData.OrderTips;
            GameEntry.UI.OpenUIForm(UIFormId.SettleForm, (int)(a / b/*  c / d*/ * m_LevelData.OrderData.OrderMoney + e));
        }
        private void Change()
        {
            if (m_ChangeDay)
            {
                GameEntry.UI.OpenUIForm(UIFormId.SettleForm,mOrderData);
            }
            else
            {
                GameEntry.UI.OpenUIForm(UIFormId.ChangeForm,mDay );
            }
        }
        /// <summary>
        /// ��ʼ����Ϸ�����ԣ�
        /// </summary>
        private void InitMain()
        {
            InitScene();
            InitUI();
            InitData();
        }
        private void InitScene()
        {
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(2);
            //����������
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", 2.ToString());
                return;
            }
            m_BackGame = false;
            //��������
            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), /*Constant.AssetPriority.SceneAsset*/0, this);

            //m_LoadingFlag.Add("LoadScene", false);
        }
        private void InitUI()
        {
            GameEntry.UI.OpenUIForm(UIFormId.MainForm, this);
            GameEntry.UI.OpenUIForm(UIFormId.WorkForm, this);
            GameEntry.UI.OpenUIForm(UIFormId.TeachForm, this);

            //m_LoadingFlag.Add("OpenMainForm", false);
            //m_LoadingFlag.Add("OpenWorkForm", false);
            //m_LoadingFlag.Add("OpenTeachForm", false);
        }
        private void InitData()
        {
            GameEntry.Utils.MaxEnergy = 80;
            GameEntry.Utils.Energy = 80;
            GameEntry.Utils.MaxAp = 6;
            GameEntry.Utils.Ap = 6;
            GameEntry.Utils.Money = 10000;
            GameEntry.Utils.Mood = 20;
            GameEntry.Utils.Favor = 0;
        }
        //���¹ؿ�
        public void GetLevel()//��Ϊװ��
        {
            mIndex++;
            if (mIndex > 3)
            {
                mDay++;
                mIndex = 1;
                m_ChangeDay = true;
            }
            else
                m_ChangeDay= false;
            m_LevelData = null;
            foreach (LevelData level in m_LevelDatas)
            {
                if (level.Day == mDay && level.Index == mIndex)
                    m_LevelData = level;
            }
            if (m_LevelData == null)
            {
                m_LevelData = GetRandomLevel();
            }
            mOrderManager.SetOrder(m_LevelData.OrderData);
        }

        private void GetForm(object sender,GameEventArgs args)
        { 
            OpenUIFormSuccessEventArgs openUIForm= (OpenUIFormSuccessEventArgs)args;
            if (openUIForm.UIForm.TryGetComponent<DialogForm>(out mDialogForm))
                return;
            if (openUIForm.UIForm.TryGetComponent<WorkForm>(out mWorkForm))
                return;
            if (openUIForm.UIForm.TryGetComponent<MainForm>(out mMainForm))
                return;
            if (openUIForm.UIForm.TryGetComponent<TeachingForm>(out mTeachingForm))
                return;
        }
        #region
        //private void CheckMaterials()
        //{
        //    ���µ���ֵ��Ϊ����
        //    if (mMaterialData.Milk < 3)
        //    {
        //        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Milk)
        //        {
        //            Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
        //        });
        //        mMaterialData.Milk++;
        //    }
        //    if (mMaterialData.Milk < 3)
        //    {
        //        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Milk)
        //        {
        //            Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
        //        });
        //        mMaterialData.Milk++;
        //    }
        //    if (mMaterialData.Milk < 3)
        //    {
        //        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Milk)
        //        {
        //            Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
        //        });
        //        mMaterialData.Milk++;
        //    }
        //    if (mMaterialData.Cream < 1)
        //    {
        //        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Cream)
        //        {
        //            Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
        //        });
        //        mMaterialData.Cream++;
        //    }
        //    if (mMaterialData.CoffeeBean < 1)
        //    {
        //        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.CoffeeBean)
        //        {
        //            Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
        //        });
        //        mMaterialData.CoffeeBean++;
        //    }
        //    if (mMaterialData.Water < 1)
        //    {
        //        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Water)
        //        {
        //            Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
        //        });
        //        mMaterialData.Water++;
        //    }
        //    if (mMaterialData.ChocolateSyrup < 1)
        //    {
        //        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.ChocolateSyrup)
        //        {
        //            Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
        //        });
        //        mMaterialData.ChocolateSyrup++;
        //    }
        //    if (mMaterialData.Ice < 1)
        //    {
        //        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Ice)
        //        {
        //            Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
        //        });
        //        mMaterialData.Ice++;
        //    }
        //    if (mMaterialData.Sugar < 1)
        //    {
        //        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Sugar)
        //        {
        //            Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
        //        });
        //        mMaterialData.Sugar++;
        //    }
        //}
        #endregion
    }
    /// <summary>
    /// Ŀǰ����������Ϸ״̬
    /// </summary>
    public enum MainState
    { 
        Foreword,//ǰ��
        Game,//�����ȵ���Ϸʱ��
        Text,//���Ƚ����������ʱ��
        Settle,//����

        Teach,//���ɽ׶�
        Behaviour,//ѡ���׶�
        Dialog,//�Ի��ڵ�
        Change,//�л��׶�
    }
}