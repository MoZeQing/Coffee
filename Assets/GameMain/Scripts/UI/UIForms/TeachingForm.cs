﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using DG.Tweening;
using XNode;
using UnityEditor.UI;
using System;
using static UnityEngine.GraphicsBuffer;

namespace GameMain
{
    public class TeachingForm : UIFormLogic
    {
        [Header("左侧信息栏")]
        [SerializeField] private Transform leftCanvas;
        [SerializeField] private Text moneyText;
        [SerializeField] private Text favorText;
        [SerializeField] private Text APText;
        [SerializeField] private Text energyText;
        [SerializeField] private Text moodText;
        [Header("右侧操作栏")]
        [SerializeField] private Transform rightCanvas;
        [SerializeField] private Button talkBtn;
        [SerializeField] private Button touchBtn;
        [SerializeField] private Button playBtn;
        [SerializeField] private Button storyBtn;
        [SerializeField] private Button sleepBtn;
        [Header("主控")]
        [SerializeField] private Transform mainCanvas;
        [SerializeField] private GameObject energyTips;
        [SerializeField] private GameObject apTips;
        [SerializeField] private DialogBox dialogBox;
        [SerializeField] private BaseStage stage;
        [SerializeField] private LittleCat mLittleCat = null;
        [SerializeField] private RectTransform mCanvas = null;

        private DialogForm mDialogForm = null;
        [SerializeField] private ActionGraph mActionGraph = null;
        private ActionNode mActionNode = null;
        private BehaviorTag mBehaviorTag;
        private ProcedureMain mProcedureMain = null;
        //Dialog区域
        private List<GameObject> m_Btns = new List<GameObject>();

        private void OnEnable()
        {
            mActionNode = mActionGraph.ActionNode();

            GameEntry.Event.Subscribe(CharDataEventArgs.EventId, CharDataEvent);
            GameEntry.Event.Subscribe(PlayerDataEventArgs.EventId, PlayerDataEvent);

            talkBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Talk));
            playBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Play));
            storyBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Story));
            sleepBtn.onClick.AddListener(() => Behaviour(BehaviorTag.Sleep));

            this.transform.localScale = Vector3.one * 0.01f;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                dialogBox.gameObject.SetActive(false);
                stage.gameObject.SetActive(false);
                leftCanvas.gameObject.SetActive(true);
                rightCanvas.gameObject.SetActive(true);
                apTips.gameObject.SetActive(false);
                energyTips.gameObject.SetActive(false);
                mLittleCat.ShowLittleCat();
            }
        }
        private void OnDisable()
        {
            mLittleCat.ShowLittleCat();
            GameEntry.Event.Unsubscribe(CharDataEventArgs.EventId, CharDataEvent);
            GameEntry.Event.Unsubscribe(PlayerDataEventArgs.EventId, PlayerDataEvent);

            talkBtn.onClick.RemoveAllListeners();
            playBtn.onClick.RemoveAllListeners();
            storyBtn.onClick.RemoveAllListeners();
            sleepBtn.onClick.RemoveAllListeners();
        }
        public void Behaviour(BehaviorTag behaviorTag)
        {
            mBehaviorTag = behaviorTag;
            List<Trigger> triggers = new List<Trigger>();
            PlayerData playerData = new PlayerData();
            switch (behaviorTag)
            {
                case BehaviorTag.Click:
                    triggers = mActionNode.Click;
                    playerData = mActionNode.ClickData;
                    break;
                case BehaviorTag.Talk:
                    triggers = mActionNode.Talk;
                    playerData = mActionNode.TalkData;
                    break;
                case BehaviorTag.Touch:
                    triggers = mActionNode.Touch;
                    break;
                case BehaviorTag.Play:
                    triggers = mActionNode.Play;
                    break;
                case BehaviorTag.Sleep:
                    triggers = mActionNode.Sleep;
                    break;
                case BehaviorTag.Morning:
                    triggers = mActionNode.Morning;
                    break;
            }
            if (behaviorTag != BehaviorTag.Sleep)
            {
                if (GameEntry.Utils.Energy < playerData.energy)
                {
                    energyTips.gameObject.SetActive(true);
                    return;
                }
                energyTips.gameObject.SetActive(false);
                if (GameEntry.Utils.Ap < playerData.ap)
                {
                    apTips.gameObject.SetActive(true);
                    return;
                }
                apTips.gameObject.SetActive(false);
                GameEntry.Utils.Energy -= playerData.energy;
                GameEntry.Utils.Money -= playerData.money;
                GameEntry.Utils.MaxEnergy -= playerData.maxEnergy;
                GameEntry.Utils.Ap -= playerData.ap;
                GameEntry.Utils.MaxAp -= playerData.maxAp;

                GameEntry.Utils.Mood += 20;
                GameEntry.Utils.Favor += 2;
                GameEntry.Utils.Hope += 1;
            }
            else
            {
                GameEntry.Utils.Energy += 60;
                GameEntry.Utils.Ap = GameEntry.Utils.MaxAp;
            }

            List<ChatNode> chatNodes = new List<ChatNode>();
            for (int i = 0; i < triggers.Count; i++)
            {
                if (GameEntry.Utils.Check(triggers[i]))
                {
                    if (mActionNode.GetPort(string.Format("{0} {1}", behaviorTag.ToString(), i)) != null)
                    {
                        NodePort nodePort = mActionNode.GetPort(string.Format("{0} {1}", behaviorTag.ToString(), i));
                        if (nodePort.Connection != null)
                        {
                            ChatNode node = (ChatNode)nodePort.Connection.node;
                            chatNodes.Add(node);
                        }
                    }
                }
            }
            if (chatNodes.Count > 0)
            {
                ChatNode chatNode = chatNodes[UnityEngine.Random.Range(0, chatNodes.Count)];
                dialogBox.gameObject.SetActive(true);
                dialogBox.SetDialog(chatNode);
                if (mBehaviorTag == BehaviorTag.Sleep)
                    dialogBox.SetComplete(OnSleep);//回调
                else if (mBehaviorTag == BehaviorTag.Morning)
                    dialogBox.SetComplete(OnGameChangeState);
                else
                    dialogBox.SetComplete(OnComplete);
                leftCanvas.gameObject.SetActive(false);
                rightCanvas.gameObject.SetActive(false);
                stage.gameObject.SetActive(true);
                mLittleCat.HideLittleCat();
            }
            else
            {
                Debug.LogWarningFormat("错误，不存在合法的对话剧情，请检查{0}的{1}", mActionNode.name, behaviorTag.ToString());
            }
        }
        private void OnComplete()
        {
            dialogBox.gameObject.SetActive(false);
            leftCanvas.gameObject.SetActive(true);
            rightCanvas.gameObject.SetActive(true);
            apTips.gameObject.SetActive(false);
            energyTips.gameObject.SetActive(false);
        }
        private void OnSleep()
        {
            GameEntry.Utils.Day++;
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, GameEntry.Utils.Day);//用这个this传参来调整黑幕
            mLittleCat.HideLittleCat();
            stage.gameObject.SetActive(false);
            rightCanvas.gameObject.SetActive(false);
            leftCanvas.gameObject.SetActive(false);
            apTips.gameObject.SetActive(false);
            energyTips.gameObject.SetActive(false);
            //播放一个睡觉效果
            Invoke(nameof(OnChangeDay), 1f);
        }
        private void OnChangeDay()
        {
            if (!GameEntry.Dialog.StoryUpdate())
                Behaviour(BehaviorTag.Morning);
            else
                OnGameChangeState();
        }
        private void OnGameChangeState()
        {
            GameEntry.Event.FireNow(this, MainStateEventArgs.Create(MainState.Work));
        }
        private void CharDataEvent(object sender, GameEventArgs e) 
        { 
            CharDataEventArgs charDataEvent= (CharDataEventArgs)e;
            CharData charData=charDataEvent.CharData;
            favorText.text = string.Format("好感:{0}", charData.favor.ToString());
            moodText.text= string.Format("心情:{0}", charData.mood.ToString());
        }
        private void PlayerDataEvent(object sender, GameEventArgs e)
        { 
            PlayerDataEventArgs playerDataEvent= (PlayerDataEventArgs)e;
            PlayerData playerData= playerDataEvent.PlayerData;
            APText.text = string.Format("行动点：{0}/{1}", playerData.ap, playerData.maxAp);
            energyText.text = string.Format("体力：{0}/{1}", playerData.energy, playerData.maxEnergy);
            moneyText.text=string.Format("金钱:{0}", playerData.money.ToString());
        }
        //单独给点击做一个方法调用
        public void Click_Action()
        {
            mLittleCat.ShowLittleCat();
            rightCanvas.gameObject.SetActive(false);
            leftCanvas.gameObject.SetActive(true);
            apTips.gameObject.SetActive(false);
            energyTips.gameObject.SetActive(false);
            Behaviour(BehaviorTag.Click);
        }
    }
}