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
        [Header("固定区域")]
        [SerializeField] private Button teachBtn;
        [SerializeField] private Button teachBtn1;
        [SerializeField] private Transform mCanvas;
        [SerializeField] private Animator mAnimator;
        [SerializeField] private TeachingForm mTeachingForm;
        [SerializeField] private Image backgroundImg;
        [SerializeField] private Image changeBackgroundImg;
        [SerializeField] private Image littleCatImg;
        [SerializeField] private Image[] apPoints;
        [SerializeField] private Image apProgress;
        [SerializeField] private Text moneyText;
        [SerializeField] private Text dayText;//日期文本框
        [SerializeField] private Text apText;//行动力文本框
        [Header("主控")]
        [SerializeField] private Button warehouseBtn;
        [SerializeField] private Button loadBtn;
        [SerializeField] private Button saveBtn;
        [SerializeField] private Button optionBtn;
        [SerializeField] private Button guideBtn;
        [SerializeField] private Button closetBtn;
        [SerializeField] private Button outBtn;
        [SerializeField] private Button buffBtn;
        [SerializeField] private Button sleepBtn;
        [SerializeField] private CanvasGroup canvasGroup;

        private PlaySoundParams playSoundParams = PlaySoundParams.Create();
        private float nowTime;
        [SerializeField, Range(0, 5f)] private float rateTime = 5f;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            teachBtn.onClick.AddListener(ChangeTeach);
            teachBtn1.onClick.AddListener(ChangeTeach);

            warehouseBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.CupboradForm));
            loadBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.LoadForm, this));
            saveBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.SaveForm, this));
            optionBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.OptionForm, this));
            guideBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.GuideForm, 3));
            closetBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.ClosetForm));
            sleepBtn.onClick.AddListener(()=>
            {
                ChangeTeach();
                mTeachingForm.OnSleep();
            });
            outBtn.onClick.AddListener(Out_OnClick);
            buffBtn.onClick.AddListener(() => GameEntry.UI.OpenUIForm(UIFormId.BuffForm));

            canvasGroup.interactable = true;

            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, OnGameStateEvent);
            GameEntry.Event.Subscribe(PlayerDataEventArgs.EventId, OnPlayerDataEvent);
            GameEntry.Event.Subscribe(CatDataEventArgs.EventId, OnCatDataEvent);
            GameEntry.Event.Subscribe(PlayerDataEventArgs.EventId, mTeachingForm.OnPlayerDataEvent);
            GameEntry.Event.Subscribe(CatDataEventArgs.EventId, mTeachingForm.OnCatDataEvent);
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            teachBtn.onClick.RemoveAllListeners();
            teachBtn1.onClick.RemoveAllListeners();
            loadBtn.onClick.RemoveAllListeners();
            saveBtn.onClick.RemoveAllListeners();
            optionBtn.onClick.RemoveAllListeners();
            guideBtn.onClick.RemoveAllListeners();
            closetBtn.onClick.RemoveAllListeners();
            outBtn.onClick.RemoveAllListeners();
            buffBtn.onClick.RemoveAllListeners();
            sleepBtn.onClick.RemoveAllListeners();

            GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, OnGameStateEvent);
            GameEntry.Event.Unsubscribe(PlayerDataEventArgs.EventId, OnPlayerDataEvent);
            GameEntry.Event.Unsubscribe(CatDataEventArgs.EventId, OnCatDataEvent);
            GameEntry.Event.Unsubscribe(PlayerDataEventArgs.EventId, mTeachingForm.OnPlayerDataEvent);
            GameEntry.Event.Unsubscribe(CatDataEventArgs.EventId, mTeachingForm.OnCatDataEvent);
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
                outBtn.interactable = false;
            }
            else
            {
                outBtn.interactable = true;
            }
            if (GameEntry.Player.GuideId == 6 && GameEntry.Player.Day == 4)
            {
                teachBtn.interactable = false;
            }
            else
            {
                teachBtn.interactable = true;
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
        private void Out_OnClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm);
            GameEntry.UI.OpenUIForm(UIFormId.MapForm);
            GameEntry.UI.CloseUIForm(this.UIForm);
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
            MainUpdate();
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
                    if (GameEntry.Player.Ap <= 0)
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
            changeBackgroundImg.sprite = backgroundImg.sprite;
            changeBackgroundImg.gameObject.SetActive(true);
            changeBackgroundImg.color = Color.white;
            backgroundImg.sprite = Resources.Load<Sprite>(weather.AssetName);
            changeBackgroundImg.DOColor(Color.clear, 3f).OnComplete(() => changeBackgroundImg.gameObject.SetActive(false));
            GameEntry.Sound.PlaySound(weather.BackgroundMusicId);
        }


        private void TitleUpdate()
        {
            moneyText.text = moneyText.text = string.Format("{0}", GameEntry.Player.Money.ToString());
            apText.text = $"AP:{GameEntry.Player.Ap}/{GameEntry.Player.MaxAp}";
            dayText.text = $"第{GameEntry.Player.Day + 1}天";
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