using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class GymForm : UIFormLogic
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button easyBtn;
        [SerializeField] private Button middleBtn;
        [SerializeField] private Button hardBtn;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            easyBtn.onClick.AddListener(EasyBtn_Click);
            middleBtn.onClick.AddListener(MiddleBtn_Click);
            hardBtn.onClick.AddListener(HardBtn_Click);
            exitBtn.onClick.AddListener(OnExit);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            easyBtn.onClick.RemoveAllListeners();
            middleBtn.onClick.RemoveAllListeners();
            hardBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.RemoveAllListeners();
        }

        private void EasyBtn_Click() {
            if (GameEntry.Player.Energy < 20)
            {
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你的体力不足");
                return;
            }
            if (GameEntry.Player.Money < 100)
            {
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你的金钱不足");
                return;
            }
            GameEntry.Player.Energy -= 20;
            GameEntry.Player.MaxEnergy += 2;

            easyBtn.interactable= false;
            middleBtn.interactable = false;
            hardBtn.interactable = false;
        }

        private void MiddleBtn_Click() {
            if (GameEntry.Player.Energy < 40)
            {
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你的体力不足");
                return;
            }
            if (GameEntry.Player.Money < 300)
            {
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你的金钱不足");
                return;
            }
            GameEntry.Player.Energy -= 40;
            GameEntry.Player.MaxEnergy += 5;

            easyBtn.interactable = false;
            middleBtn.interactable = false;
            hardBtn.interactable = false;
        }

        private void HardBtn_Click()
        {
            if (GameEntry.Player.Energy < 60)
            {
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你的体力不足");
                return;
            }
            if (GameEntry.Player.Money < 500)
            {
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你的金钱不足");
                return;
            }
            GameEntry.Player.Energy -= 60;
            GameEntry.Player.MaxEnergy += 8;

            easyBtn.interactable = false;
            middleBtn.interactable = false;
            hardBtn.interactable = false;
        }

        private void OnExit()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.Utils.Location = OutingSceneState.Home;
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }

}