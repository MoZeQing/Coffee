using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class MainMenu :UIFormLogic
{
        private ProcedureMenu m_ProcedureMenu;

        [SerializeField] private Button startBtn;
        [SerializeField] private Button loadBtn;
        [SerializeField] private Button optionBtn;
        [SerializeField] private Button galleryForm;
        [SerializeField] private Button exitBtn;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_ProcedureMenu = (ProcedureMenu)userData;

            startBtn.onClick.AddListener(m_ProcedureMenu.StartGame);
            loadBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.LoadForm, this));
            optionBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.OptionForm, this));
            galleryForm.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.GalleryForm, this));
            exitBtn.onClick.AddListener(() => UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit));
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            startBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.RemoveAllListeners();
            loadBtn.onClick.RemoveAllListeners();
            optionBtn.onClick.RemoveAllListeners();
            galleryForm.onClick.RemoveAllListeners();
        }
    }

}

