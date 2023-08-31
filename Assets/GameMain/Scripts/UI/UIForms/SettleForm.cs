using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using System.Text;

namespace GameMain
{
    public class SettleForm : UIFormLogic
    {
        [SerializeField] private Transform randomCanvas;
        [SerializeField] private Transform settleCanvas;
        [SerializeField] private Text randomText;
        [SerializeField] private Text coffeeText;
        [SerializeField] private Text catText;
        [SerializeField] private Text settleText;
        [SerializeField] private Button mOKButton;

        private bool mIsRandom = false;
        private WorkData mWorkData;

        //˼������ʲô����
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mWorkData = (WorkData)userData;
            mIsRandom = false;

            randomCanvas.gameObject.SetActive(true);
            settleCanvas.gameObject.SetActive(false);

            mOKButton.onClick.AddListener(OnClick);
            ShowRandomEvent();
        }

        private void OnEnable()
        {
            ShowRandomEvent();
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
            //RandomEventSO[] randomEvents = Resources.LoadAll<RandomEventSO>("RandomEvent");
            //List<RandomEvent> newEvents= new List<RandomEvent>();
            //foreach (RandomEventSO random in randomEvents)
            //{
            //    if (GameEntry.Utils.Check(random.randomEvent.trigger))
            //    { 
            //        newEvents.Add(random.randomEvent);
            //    }
            //}
            //RandomEvent randomEvent = newEvents[Random.Range(0, newEvents.Count-1)];
            //randomText.text = randomEvent.text;
            //mWorkData.RandomEvent = randomEvent;
        }

        private void ShowSettleData()
        {
            coffeeText.text = string.Empty;
            catText.text = string.Empty;
            settleText.text = string.Empty;
            foreach (OrderData order in mWorkData.orderDatas)
            { 
                StringBuilder sb= new StringBuilder();
                sb.Append(order.NodeName.ToString());
                if (order.Sugar) sb.Append("(��)");
                if (order.CondensedMilk) sb.Append("(����)");
                if (order.Salt) sb.Append("(��)");
                coffeeText.text += (sb.ToString() + "\n");
            }
            //catText
        }

        private void OnClick()
        {
            if (mIsRandom)
            {
                GameEntry.Event.FireNow(this, MainStateEventArgs.Create(MainState.Teach));
                GameEntry.UI.CloseUIForm(this.UIForm);
            }
            else
            { 
                mIsRandom= true;
                randomCanvas.gameObject.SetActive(false);
                settleCanvas.gameObject.SetActive(true);
            }
        }
    }
}

