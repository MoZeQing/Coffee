using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;
using GameFramework.Sound;
using GameFramework.DataTable;
using GameFramework.Event;

namespace GameMain
{
    public partial class MainForm : BaseForm
    {
        [Header("�̶�����")]
        [SerializeField] private Button teachBtn;
        [SerializeField] private Button teachBtn1;
        [SerializeField] private Animator mAnimator;
        [SerializeField] private TeachingForm mTeachingForm;
        [SerializeField] private Image backgroundImg;
        [SerializeField] private Image changeBackgroundImg;
        [SerializeField] private Image littleCatImg;
        [SerializeField] private Text moneyText;
        [SerializeField] private Text dayText;//�����ı���
        [SerializeField] private Text apText;//�ж����ı���
        [Header("����")]
        [SerializeField] private Button warehouseBtn;
        [SerializeField] private Button saveLoadBtn;
        [SerializeField] private Button guideBtn;
        [SerializeField] private Button closetBtn;
        [SerializeField] private Button outBtn;
        [SerializeField] private Button buffBtn;
        [SerializeField] private Button sleepBtn;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Button galleryForm;

        private PlaySoundParams playSoundParams = PlaySoundParams.Create();
        private float nowTime;
        [SerializeField, Range(0, 5f)] private float rateTime = 5f;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            teachBtn.onClick.AddListener(ChangeTeach);
            teachBtn1.onClick.AddListener(ChangeTeach);

            warehouseBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.CupboradForm));
            saveLoadBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.SaveLoadForm, this));
            guideBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.GuideForm, 3));
            closetBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.ClosetForm));
            galleryForm.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.GalleryForm, this));
            sleepBtn.onClick.AddListener(()=>
            {
                if (mAnimator.GetBool("Into"))
                    ShowLittleCat();
                else
                    littleCatImg.gameObject.SetActive(false);
                mAnimator.SetBool("Into", !mAnimator.GetBool("Into"));
                canvasGroup.interactable = !mAnimator.GetBool("Into");
                teachBtn1.interactable = mAnimator.GetBool("Into");
                mTeachingForm.OnSleep();
            });
            outBtn.onClick.AddListener(Out_OnClick);
            buffBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.BuffForm));

            canvasGroup.interactable = true;
            MainUpdate();
            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, OnGameStateEvent);
            GameEntry.Event.Subscribe(PlayerDataEventArgs.EventId, OnPlayerDataEvent);
            GameEntry.Event.Subscribe(CatDataEventArgs.EventId, OnCatDataEvent);
            GameEntry.Event.Subscribe(PlayerDataEventArgs.EventId, mTeachingForm.OnPlayerDataEvent);
            GameEntry.Event.Subscribe(CatDataEventArgs.EventId, mTeachingForm.OnCatDataEvent);
            GameEntry.Event.Subscribe(DialogEventArgs.EventId, OnDialogEvent);

            GameEntry.Event.Subscribe(OutEventArgs.EventId, OnOutEvent);
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            teachBtn.onClick.RemoveAllListeners();
            teachBtn1.onClick.RemoveAllListeners();
            saveLoadBtn.onClick.RemoveAllListeners();
            guideBtn.onClick.RemoveAllListeners();
            closetBtn.onClick.RemoveAllListeners();
            galleryForm.onClick.RemoveAllListeners();
            outBtn.onClick.RemoveAllListeners();
            buffBtn.onClick.RemoveAllListeners();
            sleepBtn.onClick.RemoveAllListeners();

            GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, OnGameStateEvent);
            GameEntry.Event.Unsubscribe(PlayerDataEventArgs.EventId, OnPlayerDataEvent);
            GameEntry.Event.Unsubscribe(CatDataEventArgs.EventId, OnCatDataEvent);
            GameEntry.Event.Unsubscribe(PlayerDataEventArgs.EventId, mTeachingForm.OnPlayerDataEvent);
            GameEntry.Event.Unsubscribe(CatDataEventArgs.EventId, mTeachingForm.OnCatDataEvent);
            GameEntry.Event.Unsubscribe(DialogEventArgs.EventId, OnDialogEvent);

            GameEntry.Event.Unsubscribe(OutEventArgs.EventId, OnOutEvent);
        }
        protected override void OnStart(object userData)
        {
            MainUpdate();
            TitleUpdate();
            mTeachingForm.ShowButtons();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            TitleUpdate();
            if (GameEntry.Player.GuideId <= 5&&GameEntry.Player.Day<4)
            {
                outBtn.gameObject.SetActive(false);
            }
            else
            {
                outBtn.gameObject.SetActive(true);
            }
            if (GameEntry.Player.GuideId == 6 && GameEntry.Player.Day == 4)
            {
                teachBtn.gameObject.SetActive(false);
            }
            else
            {
                teachBtn.gameObject.SetActive(true);
            }
            nowTime -=Time.deltaTime;
            if (nowTime <= 0)
            {
                nowTime = rateTime;
                ShowLittleCat();
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (!mAnimator.GetBool("Into"))
                    return;
                ChangeTeach();
            }
        }
        private void OnOutEvent(object sender, GameEventArgs e)
        {
            OutEventArgs args = e as OutEventArgs;
            if (args.OutingSceneState != OutingSceneState.Home)
                return;
            MainUpdate();
            DRWeather weather = GameEntry.DataTable.GetDataTable<DRWeather>().GetDataRow((int)GameEntry.Utils.WeatherTag);
            GameEntry.Sound.PlaySound(weather.BackgroundMusicId);
        }
        private void Out_OnClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm);
            GameEntry.UI.OpenUIForm(UIFormId.MapForm);
        }
        private void ShowLittleCat()
        {
            DRLittleCat littleCat = GameEntry.DataTable.GetDataTable<DRLittleCat>().GetDataRow(GameEntry.Cat.Closet);
            if (littleCat.Range == 0)
            {
                littleCatImg.gameObject.SetActive(false);
                return;
            }
            littleCatImg.gameObject.SetActive(true);
            int index = Random.Range(0, littleCat.Range);
            littleCatImg.sprite = Resources.Load<Sprite>($"{littleCat.ClothingPath}_{index+1}");
        }

        private void ChangeTeach()
        {
            if (mAnimator.GetBool("Into"))
                ShowLittleCat();
            else
                littleCatImg.gameObject.SetActive(false);
            mAnimator.SetBool("Into", !mAnimator.GetBool("Into"));
            canvasGroup.interactable = !mAnimator.GetBool("Into");
            teachBtn1.interactable = mAnimator.GetBool("Into");
            if (mAnimator.GetBool("Into"))
            {
                mTeachingForm.Click_Action();
            }
        }
        private void OnDialogEvent(object sender, GameEventArgs e)
        {
            DialogEventArgs args = (DialogEventArgs)e;
            if (!args.InDialog)
                MainUpdate();
        }
        private void OnCatDataEvent(object sender, GameEventArgs e)
        {
            CatDataEventArgs args = (CatDataEventArgs)e;
            MainUpdate();
            TitleUpdate();
        }
        private void OnPlayerDataEvent(object sender, GameEventArgs e)
        { 
            PlayerDataEventArgs args= (PlayerDataEventArgs)e;
            MainUpdate();
            TitleUpdate();
        }
        private void OnGameStateEvent(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            if (args.GameState == GameState.Night || args.GameState == GameState.Afternoon || args.GameState == GameState.Morning)
            {
                canvasGroup.interactable = true;
            }
        }
        protected virtual void MainUpdate()
        {
            WeatherTag weatherTag=WeatherTag.None;
            switch (GameEntry.Utils.GameState)
            {
                case GameState.Midnight:
                    weatherTag = WeatherTag.Night;
                    break;
                case GameState.Night:
                    if (Mathf.Abs(GameEntry.Player.MaxAp- GameEntry.Player.Ap) > 2)
                        weatherTag = WeatherTag.Night;
                    else
                        weatherTag = WeatherTag.Afternoon;
                    break;
                case GameState.Morning:
                    weatherTag = WeatherTag.Morning;
                    break;
            }
            if (weatherTag == GameEntry.Utils.WeatherTag)
                return;
            GameEntry.Utils.WeatherTag = weatherTag;
            DRWeather weather = GameEntry.DataTable.GetDataTable<DRWeather>().GetDataRow((int)GameEntry.Utils.WeatherTag);
            GameEntry.Sound.PlaySound(weather.BackgroundMusicId);
            changeBackgroundImg.sprite = backgroundImg.sprite;
            changeBackgroundImg.gameObject.SetActive(true);
            changeBackgroundImg.color = Color.white;
            backgroundImg.sprite = Resources.Load<Sprite>(weather.AssetName);
            changeBackgroundImg.DOColor(Color.clear, 3f).OnComplete(() => changeBackgroundImg.gameObject.SetActive(false));
        }


        private void TitleUpdate()
        {
            moneyText.text = moneyText.text = string.Format("{0}", GameEntry.Player.Money.ToString());
            apText.text = $"AP:{GameEntry.Player.Ap}/{GameEntry.Player.MaxAp}";
            dayText.text = $"��{GameEntry.Player.Day + 1}��";
        }
    }

    public enum WeatherTag
    {
        None=0,
        Morning=1,
        Afternoon=2,
        Night=3,
        Rain=4
    }
}

public class GamePosUtility
{
    private static GamePosUtility instance;
    private GamePosUtility() { }
    public static GamePosUtility Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GamePosUtility();
            }
            return instance;
        }
    }

    public GamePos GamePos
    {
        get;
        set;
    }

    public void GamePosChange(GamePos gamePos)
    {
        GamePos = gamePos;
        switch (GamePos)
        {
            case GamePos.Up:
                Camera.main.transform.DOMove(new Vector3(0f, 4.6f, -8f), 1f).SetEase(Ease.InOutExpo);
                break;
            case GamePos.Down:
                Camera.main.transform.DOMove(new Vector3(0f, -6.2f, -8f), 1f).SetEase(Ease.InOutExpo);
                break;
            case GamePos.Left:
                Camera.main.transform.DOMove(new Vector3(-19.2f, 4.6f, -8f), 1f).SetEase(Ease.InOutExpo);
                break;
            case GamePos.Right:
                Camera.main.transform.DOMove(new Vector3(19.2f, 4.6f, -8f), 1f).SetEase(Ease.InOutExpo);
                break;
        }
    }
}