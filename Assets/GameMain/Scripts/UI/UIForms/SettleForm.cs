using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using System.Text;
using DG.Tweening;

namespace GameMain
{
    public class SettleForm : BaseForm
    {
        [SerializeField] private Transform randomCanvas;
        [SerializeField] private Transform settleCanvas;
        //[SerializeField] private Text randomText;
        [SerializeField] private Text coffeeText;
        [SerializeField] private Text catText;
        [SerializeField] private Text settleText;
        [SerializeField] private Button mOKButton;
        [SerializeField] private Image[] stars;

        private bool mIsRandom = false;
        private WorkData mWorkData;

        //˼������ʲô����
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mWorkData = (WorkData)BaseFormData.UserData;
            mIsRandom = false;
            mOKButton.onClick.AddListener(OnClick);
            float level = GetPower(mWorkData);
            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(i < level);
                if (i < level)
                {
                    stars[i].transform.localScale = Vector3.zero;
                    sequence.Append(stars[i].transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutExpo).OnComplete(()=>
                    {
                        GameEntry.Sound.PlaySound(300+i);
                    }));
                }
            }
            sequence.AppendCallback(ShowSettleData);
            settleCanvas.gameObject.SetActive(true);
        }

        private int GetPower(WorkData workData)
        {
            //ʣ��ʱ�����1/3�õ�3��
            //ʣ��ʱ�����1/6�õ�2��
            //ʣ��ʱ�����0���������1���õ�1��
            //ʣ��ʱ����������û����ɶ����õ�0��
            int a = (int)(workData.Power * 6);
            if (a > 2)
                return 3;
            if (a > 1)
                return 2;
            if (a > 0 || workData.Income > 0)
                return 1;
            return 0;
        }

        private void OnEnable()
        {

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            mOKButton.onClick.RemoveAllListeners();
        }

        private void ShowRandomEvent()
        {
            RandomEventSO[] randomEvents = Resources.LoadAll<RandomEventSO>("RandomEvent");
            List<RandomEvent> newEvents = new List<RandomEvent>();
            foreach (RandomEventSO random in randomEvents)
            {
                if (GameEntry.Utils.Check(random.randomEvent.trigger))
                {
                    newEvents.Add(random.randomEvent);
                }
            }
            RandomEvent randomEvent = newEvents[Random.Range(0, newEvents.Count - 1)];
            //randomText.text = randomEvent.text;
            mWorkData.RandomEvent = randomEvent;
        }

        private void ShowSettleData()
        {
            //�����б�
            coffeeText.text = string.Empty;
            catText.text = string.Empty;
            settleText.text = string.Empty;
            Sequence sequence = DOTween.Sequence(); 
            foreach (OrderData order in mWorkData.orderDatas)
            {
                DRNode dRNode = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)order.NodeTag);
                StringBuilder sb= new StringBuilder();
                sb.Append(dRNode.Description.ToString());
                if (order.Grind) sb.Append("(��)");
                if (!order.Grind) sb.Append("(ϸ)");
                if (dRNode.Ice) sb.Append("(��)");
                if (!dRNode.Ice) sb.Append("(��)");
                sequence.Append(coffeeText.DOText(sb.ToString() + "\n", 1f));
            }
            //Сè�б�

            //���������б�
            int money = (int)(mWorkData.Income*mWorkData.Power+0.33f);
            sequence.Append(DOTween.To(value => { settleText.text = Mathf.Floor(value).ToString(); }, startValue: 0, endValue: money, duration: 0.5f));
            GameEntry.Utils.Money += money;
        }

        private void OnClick()
        {
            GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Afternoon));
            GameEntry.UI.CloseUIForm(this.UIForm);
            BuffData buffData = GameEntry.Buff.GetBuff();
            GameEntry.Utils.Money += (int)(mWorkData.Income * buffData.MoneyMulti  + buffData.MoneyPlus);
        }
    }
}

