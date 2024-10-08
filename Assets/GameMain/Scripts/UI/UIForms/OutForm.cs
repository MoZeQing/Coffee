using DG.Tweening;
using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.Event;

namespace GameMain
{
    public class OutForm : BaseForm
    {
        [SerializeField] protected Button exitBtn;
        [SerializeField] protected Button trainBtn;
        [SerializeField] protected Button matchBtn;
        [SerializeField] protected Button quickBtn;
        [SerializeField] protected Button gameBtn;
        [SerializeField] protected Text moneyText;
        [SerializeField] protected ValueData valueData;
        [SerializeField] protected int cost;
        [SerializeField] protected Week week1;
        [SerializeField] protected Week week2;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            trainBtn.onClick.AddListener(QuickBtn_Click);
            matchBtn.onClick.AddListener(GameBtn_Click);
            exitBtn.onClick.AddListener(OnExit);
            moneyText.text=GameEntry.Player.Money.ToString();

            matchBtn.interactable = (GameEntry.Player.Week == week1 || GameEntry.Player.Week == week2);

            GameEntry.Event.Subscribe(DialogEventArgs.EventId, OnDialogEvent);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            trainBtn.onClick.RemoveAllListeners();
            matchBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.RemoveAllListeners();

            GameEntry.Event.Unsubscribe(DialogEventArgs.EventId, OnDialogEvent);
        }
        private void OnDialogEvent(object sender, GameEventArgs e)
        {
            DialogEventArgs args = (DialogEventArgs)e;
            if (!args.InDialog)
            {
                DRUIForms dRUIForms = GameEntry.DataTable.GetDataTable<DRUIForms>().GetDataRow((int)BaseFormData.UIFormId);

                if (dRUIForms.OpenSound != 0)
                {
                    GameEntry.Sound.PlaySound(dRUIForms.OpenSound);
                }
            }
        }
        protected virtual void QuickBtn_Click()
        {        
            GameEntry.UI.OpenUIForm(UIFormId.ActionForm3, OnExit, valueData);
        }

        protected virtual void GameBtn_Click()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm);
            GameEntry.UI.OpenUIForm(UIFormId.QueryForm, OnExit);
        }

        protected virtual void OnExit()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.UI.OpenUIForm(UIFormId.MapForm);
            GameEntry.Utils.Location = OutingSceneState.Home;
            GameEntry.UI.CloseUIForm(this.UIForm);
            GameEntry.Event.FireNow(this, OutEventArgs.Create(OutingSceneState.Home));
        }
    }

}