using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;

namespace GameMain
{
    public class OptionForm : BaseForm
    {
        [SerializeField] private Button exit;
        [SerializeField] private Button main;
        [SerializeField] private Transform canvas;
        [SerializeField] private Transform staffCanvas;
        [SerializeField] private Button staffBtn;
        [SerializeField] private Button staffExitBtn;
        [SerializeField] private Scrollbar voiceScrollbar;
        [SerializeField] private Scrollbar wordScrollbar;
        [SerializeField] private Text voiceText;
        [SerializeField] private Text wordText;
        [SerializeField] private Text testText;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            staffCanvas.gameObject.SetActive(false);
            canvas.localPosition = Vector3.up * 1080f;
            canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);
            voiceScrollbar.onValueChanged.AddListener(OnVoiceChanged);
            wordScrollbar.onValueChanged.AddListener(OnWordChanged);

            voiceText.text = $"{GameEntry.SaveLoad.Voice * 5}";
            wordText.text = $"{GameEntry.SaveLoad.Word * 10}";
            testText.text = string.Empty;
            testText.DOText($"�����в����в����С�����������", GameEntry.SaveLoad.Word* "�����в����в����С�����������".Length);

            exit.onClick.AddListener(()=> GameEntry.UI.CloseUIForm(this.UIForm));
            main.onClick.AddListener(() => 
            {
                GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Menu));
                GameEntry.UI.CloseUIForm(this.UIForm);
            });
            staffBtn.onClick.AddListener(() => staffCanvas.gameObject.SetActive(true));
            staffExitBtn.onClick.AddListener(() => staffCanvas.gameObject.SetActive(false));
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            staffBtn.onClick.RemoveAllListeners();
            staffExitBtn.onClick.RemoveAllListeners();
            exit.onClick.RemoveAllListeners();
            main.onClick.RemoveAllListeners();
            voiceScrollbar.onValueChanged.RemoveAllListeners();
            wordScrollbar.onValueChanged.RemoveAllListeners();
        }

        private void OnVoiceChanged(float value)
        { 
            GameEntry.SaveLoad.Voice=value;
            voiceText.text = $"{value * 5}";
            GameEntry.Sound.SetVolume("BGM", GameEntry.SaveLoad.Voice);
            GameEntry.Sound.SetVolume("Sound", GameEntry.SaveLoad.Voice);
            GameEntry.Sound.SetVolume("Voice", GameEntry.SaveLoad.Voice);
            GameEntry.Sound.SetVolume("UI", GameEntry.SaveLoad.Voice);
        }

        private void OnWordChanged(float value)
        {
            GameEntry.SaveLoad.Word=0.1f-0.08f*value;
            wordText.text = $"{GameEntry.SaveLoad.Word}";
            testText.text = string.Empty;
            testText.DOKill();
            testText.DOText($"�����в����в����С�����������", GameEntry.SaveLoad.Word * "�����в����в����С�����������".Length).OnComplete(()=> testText.text = string.Empty).SetLoops(-1);
        }
    }
}

