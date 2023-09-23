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
        private string sceneName;

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
            GameEntry.Dialog.StoryUpdate();
            GameEntry.Utils.TimeTag = TimeTag.Night;
            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, LoadSceneSuccess);
            GameEntry.Event.Subscribe(MainStateEventArgs.EventId, MainStateEvent);
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(2);
            //加载主界面
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", 2.ToString());
                return;
            }
            //场景加载
            sceneName = AssetUtility.GetSceneAsset(drScene.AssetName);
            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), /*Constant.AssetPriority.SceneAsset*/0, this);
        }
        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            //GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, LoadCatSuccess);
            GameEntry.Event.Unsubscribe(MainStateEventArgs.EventId, MainStateEvent);
            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, LoadSceneSuccess);
            GameEntry.UI.CloseUIGroup("Default");
            GameEntry.UI.CloseAllLoadingUIForms();
            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
                GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }
            //等待场景加载后手动初始化一下数据
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
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
                    //切换bgm
                    break;
                case MainState.Work:
                    ChangeState<ProcedureWork>(procedureOwner);
                    //切换bgm
                    break;
                case MainState.Menu:
                    ChangeState<ProcedureMenu>(procedureOwner);
                    break;
                case MainState.Outing:
                    //切换bgm
                    break;
                case MainState.Change:
                    ChangeState<ProcedureInitMain>(procedureOwner);
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
        private void LoadSceneSuccess(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs args = (LoadSceneSuccessEventArgs)e;
            if (args.SceneAssetName == sceneName)
                GameEntry.Utils.UpdateData();
        }
    }
    /// <summary>
    /// 目前所处的主游戏状态
    /// </summary>
    public enum MainState
    {
        Undefined,
        Work,
        Teach,
        Menu,
        Outing,
        Dialog,
        Change,
        Guide
    }

    public enum TimeTag
    { 
        Morning,
        ForeWork,
        AfterWork,
        Afternoon,
        Night
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
        Main=-1,
        Home,//家
        Greengrocer,//果蔬商
        Glass,//玻璃仪器店
        MusicHall,//电影院
        Hospital,//医院
        Restaurant,//餐馆
        Beach,//海滩
        Bakery,//烘培店
        Bookstore,//书店
        BlackMarket,//黑市
        Park//公园
    }
}