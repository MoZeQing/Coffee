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

        private MainState mMainState;

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
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(2);
            //����������
            Debug.Log(drScene != null);
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", 2.ToString());
                return;
            }
            m_BackGame = false;
            Debug.Log("Start Load Scene");
            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), /*Constant.AssetPriority.SceneAsset*/0, this);
            GameEntry.UI.OpenUIForm(UIFormId.MainForm, this);
            //��ʼ����Ϣ
            m_LevelDatas.Clear();
            IDataTable<DRLevel> dtLevel = GameEntry.DataTable.GetDataTable<DRLevel>();
            for (int i = 0; i < dtLevel.Count; i++)
            {
                DRLevel dRLevel = dtLevel.GetDataRow(i);
                LevelData level = new LevelData(dRLevel);
                m_LevelDatas.Add(level);
            }
            GameEntry.Event.Subscribe(OrderEventArgs.EventId, Level);
            GameEntry.Event.Subscribe(MaterialEventArgs.EventId, UpdateMaterial);

            CheckMaterials();
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
            //GameEntry.Scene.UnloadScene(AssetUtility.GetSceneAsset(drScene.AssetName), this);

            GameEntry.UI.CloseAllLoadedUIForms();
            GameEntry.Entity.HideAllLoadedEntities();

            GameEntry.Event.Unsubscribe(OrderEventArgs.EventId, Level);
            GameEntry.Event.Unsubscribe(MaterialEventArgs.EventId, UpdateMaterial);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (m_BackGame)
            {
                ChangeState<ProcedureMenu>(procedureOwner);
            }
        }
        private LevelData GetRandomLevel()
        {
            LevelData levelData = new LevelData();
            int total = mDay + Random.Range(0, mDay);
            Debug.Log(total);
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
            int index = Random.Range(1, 3);
            levelData.Foreword = string.Format("plot_f_wm_{0}", index);
            levelData.Text = string.Format("plot_wm_{0}", index);
            return levelData;
        }
        //���¹ؿ�
        private void Level(object sender, GameEventArgs e)
        {
            OrderManager orderManager = (OrderManager)sender;
            Debug.Log("���ڳ�ʼ���ؿ�");
            switch (mMainState)
            {
                case MainState.Foreword:
                    break;
                case MainState.Game:
                    break;
                case MainState.Text:
                    break;
                case MainState.Change:
                    break;
            }
            mIndex++;
            if (mIndex > 4)
            {
                mDay++;
                mIndex = 1;
            }
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
            orderManager.SetOrder(m_LevelData.OrderData);
            GameEntry.Event.FireNow(this, MainFormEventArgs.Create(MainFormTag.Lock));
            GameEntry.Event.FireNow(this, MainFormEventArgs.Create(MainFormTag.Up));
            GameEntry.Event.FireNow(this, DialogEventArgs.Create(m_LevelData.Foreword));
        }
        private void UpdateMaterial(object sender, GameEventArgs e)
        {
            MaterialEventArgs args = (MaterialEventArgs)e;

            switch (args.NodeTag)
            {
                case NodeTag.Milk:
                    mMaterialData.Milk += args.Value;
                    break;
                case NodeTag.Water:
                    mMaterialData.Water += args.Value;
                    break;
                case NodeTag.Cream:
                    mMaterialData.Cream += args.Value;
                    break;
                case NodeTag.CoffeeBean:
                    mMaterialData.CoffeeBean += args.Value;
                    break;
                case NodeTag.ChocolateSyrup:
                    mMaterialData.ChocolateSyrup += args.Value;
                    break;
            }

            CheckMaterials();
        }
        private void CheckMaterials()
        {
            //���µ���ֵ��Ϊ����
            if (mMaterialData.Milk < 1)
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Milk)
                {
                    Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
                });
                mMaterialData.Milk++;
            }
            if (mMaterialData.Cream < 1)
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Cream)
                {
                    Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
                });
                mMaterialData.Cream++;
            }
            if (mMaterialData.CoffeeBean < 1)
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.CoffeeBean)
                {
                    Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
                });
                mMaterialData.CoffeeBean++;
            }
            if (mMaterialData.Water < 1)
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Water)
                {
                    Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
                });
                mMaterialData.Water++;
            }
            if (mMaterialData.ChocolateSyrup < 1)
            {
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.ChocolateSyrup)
                {
                    Position = new Vector3(Random.Range(-7.18f, 7.18f), Random.Range(-4.76f, 2.84f), 0f)
                });
                mMaterialData.ChocolateSyrup++;
            }
        }
    }
    /// <summary>
    /// Ŀǰ����������Ϸ״̬
    /// </summary>
    public enum MainState
    { 
        Foreword,//ǰ��
        Game,//�����ȵ���Ϸʱ��
        Text,//���Ƚ����������ʱ��
        Change//ת��
    }
}