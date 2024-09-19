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
        private GameState mGameState;

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Player.AddRecipe(1);
            GameEntry.Utils.Location = OutingSceneState.Home;
            GameEntry.UI.OpenUIForm(UIFormId.MainForm, this);
            mGameState = GameState.Afternoon;
            GameEntry.Utils.GameState = GameState.Afternoon;
            GameEntry.Dialog.StoryUpdate();
            GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Afternoon));
            mGameState = GameState.Night;
            GameEntry.Utils.GameState = GameState.Night;
            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, GameStateEvent);
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(2);
            //����������
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", 2.ToString());
                return;
            }
            //��������
            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), 0, this);
        }
        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, GameStateEvent);
            GameEntry.UI.CloseUIGroup("Default");
            GameEntry.UI.CloseAllLoadingUIForms();
            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
                GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }
            //�ȴ��������غ��ֶ���ʼ��һ������
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
        }
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (GameEntry.Dialog.InDialog)
                return;
            switch (mGameState)
            {
                case GameState.None:
                    break;
                case GameState.Work:
                    ChangeState<ProcedureWork>(procedureOwner);
                    //�л�bgm
                    break;
                case GameState.Menu:
                    ChangeState<ProcedureMenu>(procedureOwner);
                    break;
                default:
                    break;
            }
        }
        private void GameStateEvent(object sender, GameEventArgs e)
        { 
            GameStateEventArgs args= (GameStateEventArgs)e;
            mGameState = args.GameState;
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
        Dialog,
        Change,
        Guide
    }

    public enum TimeTag
    { 
        None,
        Morning,
        ForeWork,
        ForeSpecial,
        AfterSpecial,
        Afternoon,
        Evening,
        Night,
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
        Home=0,//��
        Market=1,//�г�
        Glass=2,//����������
        Gym=3,//������
        Restaurant=4,//�͹�
        Beach=5,//��̲
        Clothing=6,//��װ��
        Library=7//ͼ���
    }
}