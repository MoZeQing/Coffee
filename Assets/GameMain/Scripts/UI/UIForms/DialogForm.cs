﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using XNode;
using GameFramework.Event;
using System;
using UnityEngine.EventSystems;
using GameMain;
using DG.Tweening;
using System.Runtime.InteropServices;
using UnityEngine.SocialPlatforms.Impl;
using Dialog;

namespace GameMain
{
    public class DialogForm : BaseForm
    {
        [SerializeField] private DialogBox mDialogBox;
        [SerializeField] private BaseStage mStage;

        private DialogData m_DialogData;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm);
            SetData((DialogData)BaseFormData.UserData);
        }
        public void SetData(DialogData dialogData)
        {
            m_DialogData = dialogData;
            mDialogBox.SetDialog(dialogData);
            mDialogBox.SetComplete(CloseForm);
        }
        public void SetData(DialogueGraph dialogue)
        {
            XNodeSerializeHelper helper=new XNodeSerializeHelper();
            DialogData dialogData = helper.Serialize(dialogue);
            mDialogBox.SetDialog(dialogData);
            mDialogBox.SetComplete(CloseForm);
        }
        private void OnComplete()
        {
            GameEntry.Dialog.OnComplete();
        }
        private void CloseForm()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm);
            GameEntry.Dialog.InDialog = false;
            Invoke(nameof(OnComplete), 1f);
            GameEntry.Event.FireNow(this, DialogEventArgs.Create(false, m_DialogData.DialogName));
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
}
