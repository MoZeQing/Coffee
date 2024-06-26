﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using DG.Tweening;
using XNode;
using System;

namespace GameMain
{
    public class TeachingForm : UIFormLogic
    {
        [Header("左侧信息栏")]
        [SerializeField] private Transform leftCanvas;
        [SerializeField] private Text moneyText;
        [SerializeField] private Text energyText;
        [SerializeField] private Text favorText;
        [SerializeField] private Text loveText;
        [SerializeField] private Text familyText;
        [SerializeField] private Text timeText;
        [SerializeField] private Text rentText;
        [Header("右侧操作栏")]
        [SerializeField] private Transform rightCanvas;
        [SerializeField] private Transform buttonCanvas;
        [SerializeField] private Transform exitBtn;
        [Header("主控")]
        [SerializeField] private DialogBox dialogBox;
        [SerializeField] private BaseStage stage;
        [SerializeField] private CanvasGroup mCanvasGroup = null;

        private DialogForm mDialogForm = null;
        [SerializeField] public ActionGraph mActionGraph = null;
        [SerializeField] private GameObject behaviorBtn;
        private CatStateData mCatStateData = null; 
        private ActionNode mActionNode = null;
        private BehaviorTag mBehaviorTag;
        private ProcedureMain mProcedureMain = null;
        private bool InDialog;
        //Dialog区域
        private List<GameObject> m_Btns = new List<GameObject>();

        private void OnEnable()
        {
            GameEntry.Event.Subscribe(CharDataEventArgs.EventId, CharDataEvent);
            GameEntry.Event.Subscribe(PlayerDataEventArgs.EventId, PlayerDataEvent);
            GameEntry.Utils.UpdateData();
            ShowButtons();
            mCanvasGroup = this.GetComponent<CanvasGroup>();
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(CharDataEventArgs.EventId, CharDataEvent);
            GameEntry.Event.Unsubscribe(PlayerDataEventArgs.EventId, PlayerDataEvent);
        }
        //private void Start()
        //{
        //    GameEntry.Event.Subscribe(CharDataEventArgs.EventId, CharDataEvent);
        //    GameEntry.Event.Subscribe(PlayerDataEventArgs.EventId, PlayerDataEvent);
        //    GameEntry.Utils.UpdateData();
        //    ShowButtons();
        //    mCanvasGroup = this.GetComponent<CanvasGroup>();
        //}
        private void Update()
        {
            //if (Input.GetMouseButtonDown(1)&&!InDialog)
            //{
            //    GameEntry.Event.FireNow(this, MainFormEventArgs.Create(MainFormTag.Unlock));
            //    dialogBox.gameObject.SetActive(false);
            //    stage.gameObject.SetActive(false);
            //}
            if (mCanvasGroup.alpha < 1)
            {
                mCanvasGroup.alpha = Mathf.Lerp(mCanvasGroup.alpha, 1, 0.3f);
            }
        }
        private void ShowButtons()
        {
            if (mCatStateData == GameEntry.Cat.GetCatState())
                return;
            mCatStateData = GameEntry.Cat.GetCatState();
            DestroyButtons();
            foreach (BehaviorData behaviorData in mCatStateData.behaviors)
            {
                if (behaviorData.behaviorTag == BehaviorTag.Morning)
                    continue;
                if (behaviorData.behaviorTag == BehaviorTag.Click)
                    continue;
                GameObject go = GameObject.Instantiate(behaviorBtn, buttonCanvas);
                Button button=go.GetComponent<Button>();
                Text text = go.transform.Find("Text").GetComponent<Text>();
                if(behaviorData.behaviorTag==BehaviorTag.Sleep)
                    button.onClick.AddListener(OnSleep);
                else
                    button.onClick.AddListener(() => Behaviour(behaviorData.behaviorTag));
                text.text = behaviorData.behaviorTag.ToString();
                m_Btns.Add(go);
            }
        }
        private void DestroyButtons()
        {
            for (int i=0;i<buttonCanvas.childCount;i++)
            {
                Destroy(buttonCanvas.GetChild(i).gameObject);
            }
            m_Btns.Clear();
        }
        public void Behaviour(BehaviorTag behaviorTag)
        {
            GameEntry.Event.FireNow(this, MainFormEventArgs.Create(MainFormTag.Lock));
            mBehaviorTag = behaviorTag;
            BehaviorData behavior = GameEntry.Cat.GetBehavior(behaviorTag);
            if (behaviorTag != BehaviorTag.Sleep)
            {
                if (GameEntry.Utils.Energy < behavior.playerData.energy)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你没有足够的体力");
                    GameEntry.UI.OpenUIForm(UIFormId.MainForm);
                    return;
                }
                if (GameEntry.Utils.Ap < behavior.playerData.ap)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.PopTips, "你没有足够的行动力");
                    GameEntry.UI.OpenUIForm(UIFormId.MainForm);
                    return;
                }
                BuffData buffData = GameEntry.Buff.GetBuff();
                GameEntry.Utils.Energy -= (int)Mathf.Clamp((behavior.playerData.energy*buffData.EnergyMulti+buffData.EnergyPlus),0,9999999);
                GameEntry.Utils.Money -= behavior.playerData.money;
                GameEntry.Utils.MaxEnergy -= behavior.playerData.maxEnergy;
                GameEntry.Utils.Ap -= behavior.playerData.ap;
                GameEntry.Utils.MaxAp -= behavior.playerData.maxAp;

                GameEntry.Utils.Favor += (int)(behavior.catData.favour * buffData.FavorMulti + buffData.FavorPlus);
                GameEntry.Utils.Love+= behavior.catData.love;
                GameEntry.Utils.Family += behavior.catData.family;
            }
            else
            {
                GameEntry.Utils.Ap = GameEntry.Utils.MaxAp;
            }
            DialogueGraph dialogueGraph = behavior.dialogues[UnityEngine.Random.Range(0, behavior.dialogues.Count)];
            dialogBox.gameObject.SetActive(true);
            dialogBox.SetDialog(dialogueGraph);
            InDialog = true;
            dialogBox.SetComplete(OnComplete);
            leftCanvas.gameObject.SetActive(false);
            rightCanvas.gameObject.SetActive(false);
            stage.gameObject.SetActive(true);
        }
        //不允许在回调中再设置回调，会导致回调错误
        //也就是说，在SetComplete方法中设置的方法不能有Behaviour等会设置回调的方法
        private void PassDay()
        {
            GameEntry.Utils.Ap = GameEntry.Utils.MaxAp;
            rightCanvas.gameObject.SetActive(false);
            leftCanvas.gameObject.SetActive(false);
            GameEntry.Utils.Day++;
            GameEntry.UI.OpenUIForm(UIFormId.PassDayForm);//用这个this传参来调整黑幕
            GameEntry.Utils.GameState = GameState.Morning;
            GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Morning));
            Invoke(nameof(OnFaded), 4f);
        }
        private void OnSleep()
        {
            InDialog = false;
            BuffData buffData = GameEntry.Buff.GetBuff();
            GameEntry.Utils.Energy = (int)(GameEntry.Utils.MaxEnergy * buffData.EnergyMaxMulti + buffData.EnergyMaxPlus);
            GameEntry.Utils.GameState = GameState.Midnight;
            if (!GameEntry.Dialog.StoryUpdate(PassDay))
                Behaviour(BehaviorTag.Sleep);
        }
        //private void OnMorning()
        //{
        //    if (!GameEntry.Dialog.StoryUpdate())
        //        Behaviour(BehaviorTag.Morning);
        //}
        private void OnComplete()
        {
            InDialog=false;
            if (mBehaviorTag == BehaviorTag.Sleep)
            {
                rightCanvas.gameObject.SetActive(false);
                leftCanvas.gameObject.SetActive(false);
                GameEntry.Utils.Day++;
                GameEntry.UI.OpenUIForm(UIFormId.PassDayForm);//用这个this传参来调整黑幕
                Invoke(nameof(OnFaded), 4f);
            }
            else if (mBehaviorTag == BehaviorTag.Morning)
            {
                if (GameEntry.Utils.Week != Week.Sunday)
                    GameEntry.Utils.GameState = GameState.Work;
                else
                {
                    GameEntry.Event.FireNow(this, MainFormEventArgs.Create(MainFormTag.Unlock));
                    dialogBox.gameObject.SetActive(false);
                    stage.gameObject.SetActive(true);
                    leftCanvas.gameObject.SetActive(true);
                    rightCanvas.gameObject.SetActive(true);
                    GameEntry.Utils.GameState = GameState.Night;
                }
            }
            else
            {
                dialogBox.gameObject.SetActive(false);
                leftCanvas.gameObject.SetActive(true);
                rightCanvas.gameObject.SetActive(true);
                stage.ShowDiff(DialogPos.Middle, DiffTag.MoRen);
                ShowButtons();
            }
        }
        private void OnFaded()
        {
            mCanvasGroup.alpha = 0;
            GameEntry.Utils.GameState = GameState.Morning;
            mBehaviorTag = BehaviorTag.Morning;
            if (!GameEntry.Dialog.StoryUpdate(OnComplete))
            {
                Behaviour(BehaviorTag.Morning);
            }
        }
        private void CharDataEvent(object sender, GameEventArgs e) 
        { 
            CharDataEventArgs charDataEvent= (CharDataEventArgs)e;
            CharData charData=charDataEvent.CharData;
            favorText.text = charData.favor.ToString();
            loveText.text = charData.love.ToString();
            familyText.text = charData.family.ToString();
        }
        private void PlayerDataEvent(object sender, GameEventArgs e)
        { 
            PlayerDataEventArgs playerDataEvent= (PlayerDataEventArgs)e;
            PlayerData playerData= playerDataEvent.PlayerData;
            //rentText.transform.parent.gameObject.SetActive(GameEntry.Utils.Rent != 0);
            //rentText.text = string.Format("距离下一次欠款缴纳还有{0}天\r\n下一次交纳欠款：{1}",6-(playerData.day + 20) % 7, GameEntry.Utils.Rent.ToString());
            BuffData buffData = GameEntry.Buff.GetBuff();
            energyText.text = string.Format("{0}/{1}", playerData.energy, playerData.maxEnergy*buffData.EnergyMaxMulti+buffData.EnergyMaxPlus);
            moneyText.text=string.Format("{0}", playerData.money.ToString());
            timeText.text = string.Format("第{0}周 第{1}天 星期{2}", playerData.day/7+1, playerData.day% 7, AssetUtility.GetWeekCN(playerData.day % 7));
        }
        //单独给点击做一个方法调用
        public void Click_Action()
        {
            Behaviour(BehaviorTag.Click);
        }
    }
}