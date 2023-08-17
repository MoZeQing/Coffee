using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Procedure;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
using System;

namespace GameMain
{
    public class ProcedureMain : ProcedureBase
    {
        private MainState mMainState;
        public Cat Cat
        {
            get;
            set;
        } = null;

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            // ��ԭ��Ϸ�ٶ�
            GameEntry.Base.ResetNormalGameSpeed();

            //��ʼ����Ϣ
            InitMain();     
        }
        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, LoadCatSuccess);
        }
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            switch (mMainState)
            {
                case MainState.Undefined:
                    break;
                case MainState.Teach:
                    //�л�bgm
                    GameEntry.Dialog.StoryUpdate();
                    break;
                case MainState.Work:
                    //�л�bgm
                    GameEntry.Dialog.StoryUpdate();
                    break;
                case MainState.Menu:
                    ChangeState<ProcedureMenu>(procedureOwner);
                    break;
                case MainState.Outing:
                    //�л�bgm
                    GameEntry.Dialog.StoryUpdate();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// ��ʼ����Ϸ�����ԣ�
        /// </summary>
        private void InitMain()
        {
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(2);
            //����������
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", 2.ToString());
                return;
            }
            //��������
            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), /*Constant.AssetPriority.SceneAsset*/0, this);

            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, LoadCatSuccess);

            GameEntry.UI.OpenUIForm(UIFormId.MainForm, this);
            GameEntry.UI.OpenUIForm(UIFormId.TeachForm, this);
            //��ʼ����ɫ
            GameEntry.Entity.ShowCat(new CatData(GameEntry.Entity.GenerateSerialId(), 10008)
            {
                Position = new Vector3(0f, 4.6f)
            });

            InitData();
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

        private void LoadCatSuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs showEntitySuccess= (ShowEntitySuccessEventArgs)e;
            Cat cat = null;
            if (showEntitySuccess.Entity.TryGetComponent<Cat>(out cat))
            { 
                Cat= cat;
            }
            Cat.HideCat();
        }
    }
    /// <summary>
    /// Ŀǰ����������Ϸ״̬
    /// </summary>
    public enum MainState
    {
        Undefined,
        Work,
        Teach,
        Menu,
        Outing
    }

    public enum OutingSceneState
    {
        Greengrocer,//������
        Glass,//����������
        Cinema,//��ӰԺ
        Hospital,//ҽԺ
        Restaurant,//�͹�
        Beach,//��̲
        Bakery,//�����
        Bookstore,//���
        BlackMarket,//����
        Park//��԰
    }
}