using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class TitleForm : BaseForm
    {
        public UIFormId UIFormId { get; private set; } = UIFormId.TitleForm;
        [SerializeField] private Text text;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            text.text = (string)userData;
            Invoke(nameof(OnComplete), 1f);
        }

        private void OnComplete() => GameEntry.UI.CloseUIForm(this.UIForm);

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }

}