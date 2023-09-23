using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using XNode;

namespace GameMain
{
    public class BaseCharacter : EntityLogic, IPointerDownHandler
    {
        public DialogPos DialogPos
        {
            get;
            set;
        }

        private CharacterData mCharacterData = null;
        private Image mImage = null;
        private ActionNode mActionNode = null;

        private List<Sprite> mDiffs = new List<Sprite>();//���
        private ActionState mActionState;

        private void Start()
        {
            mImage = this.GetComponent<Image>();
        }
        private void OnEnable()
        {
            mImage = this.GetComponent<Image>();
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            mCharacterData = (CharacterData)userData;
            DialogPos= mCharacterData.DialogPos;

            mImage = this.GetComponent<Image>();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            //mActionState = ActionState.Click;
            //if (mActionNode.click != null)
            //{
            //    List<ChatNode> chatNodes = new List<ChatNode>();
            //    for (int i = 0; i < mActionNode.click.Count; i++)
            //    {
            //        if (GameEntry.Utils.Check(mActionNode.click[i]))
            //        {
            //            if (mActionNode.GetPort(string.Format("click {0}", i)) != null)
            //            {
            //                NodePort nodePort = mActionNode.GetPort(string.Format("click {0}", i));
            //                if (nodePort.Connection != null)
            //                {
            //                    ChatNode node = (ChatNode)nodePort.Connection.node;
            //                    chatNodes.Add(node);
            //                }
            //            }
            //        }
            //    }
            //    if (chatNodes.Count > 0)
            //    {
            //        ChatNode chatNode = chatNodes[Random.Range(0, chatNodes.Count)];
            //    }
            //    else
            //    {
            //        Debug.LogWarningFormat("���󣬲�������Ч�ĶԻ��ļ��������ļ��Լ������������ļ���{0}", mCharacterData.ActionGraph.name);
            //    }
            //}
        }
        public void SetAction(ActionData actionData)
        {
            Debug.Log(this.gameObject.name);
            mImage = this.GetComponent<Image>();
            mImage.sprite = mDiffs[(int)actionData.diffTag];
            switch (actionData.actionTag)
            {
                case ActionTag.Jump:
                    break;
                case ActionTag.Shake:
                    break;
            }
        }

        public void SetData(CharSO charSO)
        {
            mDiffs = charSO.diffs;
        }
    }
    [System.Serializable]
    public class ActionData
    {
        public DiffTag diffTag;
        public ActionTag actionTag;
        public SoundTag soundTag;
    }
    //���Tag
    public enum DiffTag
    {
        AnXiang,//����
        CuoE,//��G��O~O
        no,//��������
        Yun,//��
        Chan,//��
        OO,//ŶŶ
        HaiXiu,//����
        DanXin,//����
        QieYi,//���
        O,//Ŷ
        Ai,//��
        YinAnDeXiao,//������Ц
        XianQi,//����
        ShengQi,//����
        ZhongJingYa,//�о���
        WeiXiao,//΢Ц
        XiaoJingYa,//С����
        MoRen,//Ĭ�ϱ���
    }
    //����Tag
    public enum ActionTag
    {
        None,//��
        Jump,//��������
        Shake,//���Ҷ���
        Squat
    }
    public enum ActionState
    {
        Idle,//��ֹ״̬
        Click,//���״̬
        Coffee,//����״̬
    }
}
