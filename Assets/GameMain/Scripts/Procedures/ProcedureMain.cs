using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Procedure;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using GameFramework.Fsm;
using System;
using DG.Tweening;

namespace GameMain
{
    public class ProcedureMain : ProcedureBase
    {
        private MainState mMainState;
        private bool mDialog = false;  

        public GamePos GamePos
        {
            get;
            private set;
        }
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            mMainState = MainState.Teach;
            GameEntry.Utils.Location = OutingSceneState.Home;
            GameEntry.UI.OpenUIForm(UIFormId.MainForm, this);
            //GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, LoadCatSuccess);
            GameEntry.Event.Subscribe(MainStateEventArgs.EventId, MainStateEvent);
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
        }
        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            //GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, LoadCatSuccess);
            GameEntry.Event.Unsubscribe(MainStateEventArgs.EventId, MainStateEvent);
            GameEntry.UI.CloseAllLoadedUIForms();
            GameEntry.UI.CloseAllLoadingUIForms();
            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
                GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }
        }
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (GameEntry.Dialog.InDialog)
                return;
            switch (mMainState)
            {
                case MainState.Undefined:
                    break;
                case MainState.Teach:
                    //�л�bgm
                    break;
                case MainState.Work:
                    ChangeState<ProcedureWork>(procedureOwner);
                    //�л�bgm
                    break;
                case MainState.Menu:
                    ChangeState<ProcedureMenu>(procedureOwner);
                    break;
                case MainState.Outing:
                    //�л�bgm
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void MainStateEvent(object sender, GameEventArgs e)
        { 
            MainStateEventArgs args= (MainStateEventArgs)e;
            mMainState = args.MainState;
        }
        private void GamePosEvent(object sender, GameEventArgs args)
        {
            GamePosEventArgs gamePos = (GamePosEventArgs)args;
            GamePos = gamePos.GamePos;
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
        Outing,
        Dialog
    }

    public enum TimeTag
    { 
        ForeWork,
        AfterWork,
    }

    public enum Week
    { 
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    public enum OutingSceneState
    {
        Home,//��
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