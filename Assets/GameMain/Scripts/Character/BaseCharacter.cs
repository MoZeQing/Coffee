using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using XNode;
using DG.Tweening;

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
        private ActionNode mActionNode = null;
        public CharSO mCharSO = null;
        public Image mImage = null;

        private List<Sprite> mDiffs = new List<Sprite>();//���
        private ActionState mActionState;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            mCharacterData = (CharacterData)userData;
            DialogPos= mCharacterData.DialogPos;
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

        }
        public void SetAction(ActionData actionData)
        {
            SetDiff(actionData.diffTag);
            switch (actionData.actionTag)
            {
                case ActionTag.Jump:
                    break;
                case ActionTag.Shake:
                    break;
            }
        }
        public void SetDiff(DiffTag diffTag)
        {
            if (mCharSO == null)
                return;
            if (mCharSO.isMain)
                mImage.sprite = mDiffs[(GameEntry.Utils.closet - 1001) * 18 + (int)diffTag];
            else
                mImage.sprite = mDiffs[(int)diffTag];
        }
        public void SetData(CharSO charSO)
        {
            mCharSO = charSO;
            mDiffs = charSO.diffs;

            mImage.color = Color.gray;
            mImage.DOColor(Color.white, 0.3f);
            mImage.gameObject.transform.localPosition += Vector3.right * 100f;
            mImage.gameObject.transform.DOLocalMoveX(0f, 0.3f);
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
