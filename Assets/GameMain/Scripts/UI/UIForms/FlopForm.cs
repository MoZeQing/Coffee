using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class FlopForm : BaseForm
    {
        /* 1.���ű��Ķ��������
         * 2.���Ƶ����ָ�ΪͼƬ������ͼƬʹ�ñ�����List
         * 3.ע��һ����Ч
         * 4.�����������ѽ�������������
         * 5.ע��ע��Button�ļ���
         */
        [SerializeField] int pairCount = 8;
        [SerializeField] List<Sprite> cardFront = new List<Sprite>();
        private int[] m_pariGenCount;
        [SerializeField] private GameObject flopCard;
        [SerializeField] private Transform genNode;
        private List<FlopCard> m_cardList = new List<FlopCard>();
        [SerializeField] private FlopCard card01 = null;
        [SerializeField] private FlopCard card02 = null;
        [SerializeField] private float timeLimit;
        private float timer;
        [SerializeField] private Text showTime;

        [SerializeField] private CharData charData;
        [SerializeField] private PlayerData playerData;

        private Action mAction;
        private int flipCount;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mAction = BaseFormData.Action;
            
            InitFlop();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (timer <= 0)
            {
                Dictionary<ValueTag, int> dic = new Dictionary<ValueTag, int>();
                float power = flipCount / pairCount;
                int charm = (int)(charData.charm * power);
                dic.Add(ValueTag.Charm, charm);
                playerData.GetValueTag(dic);

                GameEntry.UI.OpenUIForm(UIFormId.CompleteForm, OnExit, dic);
            }
            else if(flipCount <= pairCount)
            {
                Dictionary<ValueTag, int> dic = new Dictionary<ValueTag, int>();
                dic.Add(ValueTag.Charm, charData.charm);
                playerData.GetValueTag(dic);

                GameEntry.UI.OpenUIForm(UIFormId.CompleteForm, OnExit, dic);
                return;
            }

            timer -= Time.deltaTime;
            showTime.text = FormatTime((int)timeLimit);

            foreach (var card in m_cardList)
            {
                if (card.isFlip && !card.isDone)
                {
                    if (card01 == null)
                    {
                        card01 = card;
                    }
                    else if (card02 == null && card01 != card)
                    {
                        card02 = card;
                    }
                }
            }

            if (card02 != null && card01 != null)
            {
                if (card01.ID == card02.ID)
                {
                    flipCount++;
                    card01.canClick = false;
                    card01.isDone = true;
                    card02.canClick = false;
                    card02.isDone = true;

                    card01 = null;
                    card02 = null;
                }
                else
                {
                    foreach (var card in m_cardList)
                    {
                        card.CoolDown();
                    }
                    card01.TrunBack();
                    card02.TrunBack();

                    card01 = null;
                    card02 = null;
                }
            }
        }

        private void InitFlop()
        {
            timer = timeLimit;
            flipCount = 0;

            m_pariGenCount = new int[pairCount];
            for (int i = 0; i < pairCount; i++)
                m_pariGenCount[i] = 2;
            m_cardList.Clear();

            do
            {
                var index = UnityEngine.Random.Range(0, 8);
                if (m_pariGenCount[index] > 0)
                {
                    var go = Instantiate(flopCard, genNode).GetComponent<FlopCard>();
                    go.ID = index;
                    go.front = cardFront[index];
                    m_cardList.Add(go);
                    m_pariGenCount[index]--;
                }

            } while (m_cardList.Count < pairCount * 2);
        }

        public static string FormatTime(int totalSeconds)
        {
            int minutes = totalSeconds / 60; // ���������
            int seconds = totalSeconds % 60; // ����ʣ�������

            // ��ʽ���ַ�����ȷ��������������λ���֣������Ҫ�Ļ���
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        private void OnExit()
        {
            mAction();
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
}

