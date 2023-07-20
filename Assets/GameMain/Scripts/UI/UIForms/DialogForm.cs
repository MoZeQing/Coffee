﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using XNode;
using GameFramework.Event;
using System;

namespace GameMain
{
    public class DialogForm : MonoBehaviour
    {
        [SerializeField] private Text nameText;
        [SerializeField] private Text dialogText;
        [SerializeField] private Button dialogBtn;
        [SerializeField] private Image spriteRenderer;
        [SerializeField] private Transform mCanvas;
        [SerializeField] private GameObject mBtnPrefab;

        private int _index;
        private DialogueGraph m_Dialogue = null;
        private StartNode m_StartNode = null;
        private ChatTag chatTag;
        private Node m_Node = null;
        private List<GameObject> m_Btns = new List<GameObject>();
        private bool hasQuestion = false;
        // Start is called before the first frame update
        // Update is called once per frame
        public void ShowButtons(List<OptionData> options)
        {
            ClearButtons();
            foreach (OptionData option in options)
            {
                GameObject go = GameObject.Instantiate(mBtnPrefab, mCanvas);
                go.GetComponent<OptionItem>().OnInit(option, Option_Onclick);
                m_Btns.Add(go);
            }
        }
        public void ClearButtons()
        {
            foreach (GameObject go in m_Btns)
            {
                Destroy(go);
            }
            m_Btns.Clear();
        }
        public void Next()
        {
            if (m_Node == null)
                return;
            switch (chatTag)
            {
                case ChatTag.Start:
                    StartNode startNode = (StartNode)m_Node;
                    Next(startNode);
                    break;
                case ChatTag.Chat:
                    ChatNode chatNode = (ChatNode)m_Node;
                    Next(chatNode);
                    break;
                case ChatTag.Option:
                    OptionNode optionNode = (OptionNode)m_Node;
                    ShowButtons(optionNode.chatDatas);
                    break;
            }
        }
        private void Next(StartNode startNode)
        {
            if (startNode.GetOutputPort("start") != null)
            {
                var node = startNode.GetOutputPort("start").Connection.node;
                switch (node.GetType().ToString())
                {
                    case "ChatNode":
                        m_Node = node;
                        chatTag = ChatTag.Chat;
                        break;
                    case "OptionNode":
                        m_Node = node;
                        chatTag = ChatTag.Option;
                        break;
                }
                Next();
            }
        }
        private void Next(ChatNode chatNode)
        {
            if (_index < chatNode.chatDatas.Count)
            {
                ChatData chatData = chatNode.chatDatas[_index];
                nameText.text = chatData.charName;
                dialogText.text = chatData.text;
                if (chatData.charSprite != null)
                {
                    spriteRenderer.sprite = chatData.charSprite;
                    spriteRenderer.color = Color.white;
                }
                if (chatNode.GetPort(string.Format("chatDatas {0}", _index))!=null)
                {
                    NodePort nodePort = chatNode.GetPort(string.Format("chatDatas {0}", _index));
                    if (nodePort.Connection != null)
                    {
                        Node node = nodePort.Connection.node;
                        switch (node.GetType().ToString())
                        {
                            case "ChatNode":
                                m_Node = node;
                                chatTag = ChatTag.Chat;
                                break;
                            case "OptionNode":
                                m_Node = node;
                                chatTag = ChatTag.Option;
                                break;
                        }
                        _index = 0;
                    }
                }
                _index++;
            }
            else
            {
                //播放完毕
                nameText.text = string.Empty;
                dialogText.text = string.Empty;
                spriteRenderer.color = Color.clear;
                _index= 0;
                m_Dialogue = null;
                m_StartNode= null;
                m_Node = null;
                GameEntry.Event.FireNow(this, DialogEventArgs.Create(""));
                //这不是一个好的通信方式，因为事件最好是自己做了什么被监听
            }
        }
        private void Next(OptionData optionData)
        {
            if (optionData == null)
                return;
            ClearButtons();
            OptionNode optionNode = (OptionNode)m_Node;
            if (optionNode.GetPort(string.Format("chatDatas {0}", _index)) != null)
            {
                NodePort nodePort = optionNode.GetPort(string.Format("chatDatas {0}", _index));
                if (nodePort.Connection != null)
                {
                    Node node = nodePort.Connection.node;
                    switch (node.GetType().ToString())
                    {
                        case "ChatNode":
                            m_Node = node;
                            chatTag = ChatTag.Chat;
                            break;
                        case "OptionNode":
                            m_Node = node;
                            chatTag = ChatTag.Option;
                            break;
                    }
                    _index = 0;
                    Next();
                }
            }
        }
        public void SetDialog(DialogueGraph graph)
        {
            m_Dialogue = graph;
            _index = 0;
            foreach (Node node in m_Dialogue.nodes)
            {
                if (node.GetType().ToString() == "StartNode")
                {
                    m_StartNode = (StartNode)node;
                    m_Node = node;
                    chatTag = ChatTag.Start;
                }
            }
            Next();
        }
        public void SetDialog(string path)
        {
            m_Dialogue = (DialogueGraph)Resources.Load<DialogueGraph>(string.Format("DialogData/{0}", path));
            _index = 0;
            foreach (Node node in m_Dialogue.nodes)
            {
                if (node.GetType().ToString() == "StartNode")
                {
                    m_StartNode = (StartNode)node;
                    m_Node = node;
                    chatTag = ChatTag.Start;
                }
            }
            Next();
        }

        private void SetDialog(object sender, GameEventArgs e)
        {
            LevelEventArgs args = (LevelEventArgs)e;
            switch (args.MainState)
            {
                case MainState.Foreword:
                    SetDialog(args.LevelData.Foreword);
                    break;
                case MainState.Text:
                    SetDialog(args.LevelData.Text);
                    break;
            }
        }

        private void Start()
        {
            dialogBtn.onClick.AddListener(Next);
            GameEntry.Event.Subscribe(LevelEventArgs.EventId, SetDialog);
        }
        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.LeftControl))
                Next();
            if(Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Return))
                Next();
        }
        private void OnDestroy()
        {
            GameEntry.Event.Unsubscribe(LevelEventArgs.EventId, SetDialog);
        }

        private void Option_Onclick(object sender, EventArgs e)
        {
            OptionData optionData = (OptionData)sender;
            Next(optionData);
        }
    }
}
