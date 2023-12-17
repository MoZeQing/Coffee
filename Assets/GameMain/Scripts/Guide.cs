using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class Guide : MonoBehaviour
    {
        //Lv1
        //������ȶ�֮��Ż�������ĥ������ʾ��ʾ��

        //��ĥ����֮���Ż���ʾˮ�ͼ���������ʾ��ʾ��
        [SerializeField] private List<GameObject> materials = new List<GameObject>();
        //����ˮ֮��Ż����ӹ���������ʾ��

        //���˺�Ҫ�󽻵�

        //Lv2
        //Ҫ������������

        //����ţ�̺ʹ�����

        //Ҫ���ţ��

        //�򷢺����ɿ��ȱ�

        //��ʾ����

        //Lv3
        //Ҫ�����������������ǣ�

        //Ҫ������������

        //��ʾ���ǣ�Ҫ�����

        // Start is called before the first frame update
        void Start()
        {
            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, Guide1_1);
        }

        // Update is called once per frame

        public void Guide1_1(object sender,GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            if (args.GameState == GameState.Special)
            {
                materials[(int)NodeTag.CoffeeBean].SetActive(true);
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "������ȶ���λ�������ɶ�Ӧ�Ĳ��Ͽ�");
                GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, Guide1_1);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide1_2);
            }
        }

        public void Guide1_2(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.CoffeeBean)
                    return;
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.ManualGrinder)
                {
                    Position = Vector3.zero
                });
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "ʹ����ĥ�������ȷ���ĥΪϸ���ȷ�");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide1_2);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide1_3);
            }
        }

        public void Guide1_3(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.FineGroundCoffee)
                    return;
                materials[1].SetActive(true);//ˮ
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "���ˮ��λ�������ɶ�Ӧ�Ĳ��Ͽ�");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide1_3);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide1_4);
            }
        }

        public void Guide1_4(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Water)
                    return;
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Heater)
                {
                    Position = Vector3.zero
                });
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "ʹ�ü�������ˮ����");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide1_4);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide1_5);
            }
        }

        public void Guide1_5(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.HotWater)
                    return;
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Kettle)
                {
                    Position = Vector3.zero
                });
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "����ˮ��ϸ���ȷ۰�<color=red>˳��</color>���õ����˺��У�����Ũ������");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide1_5);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide1_6);
            }
        }

        public void Guide1_6(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Espresso)
                    return;
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "�϶����ɵĿ��ȿ��Ƶ����ϽǵĶ�Ӧ������");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide1_6);
                GameEntry.Event.Subscribe(GameStateEventArgs.EventId, Guide2_1);
            }
        }

        public void Guide2_1(object sender,GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            if (args.GameState == GameState.Special)
            {
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "���ڣ�����һ���µ�Ũ�����Ȱ�");
                GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, Guide2_1);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_2);
            }
        }

        public void Guide2_2(object sender,GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Espresso)
                    return;
                materials[2].SetActive(true);
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "���ţ�̲�λ�������ɶ�Ӧ�Ĳ��Ͽ�");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_2);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_3);
            }
        }

        public void Guide2_3(object sender,GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Milk)
                    return;
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Stirrer)
                {
                    Position = Vector3.zero
                });
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "ʹ�ý�������ţ�̴�");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_3);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_4);
            }
        }

        public void Guide2_4(object sender,GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.LowFoamingMilk)
                    return;
                materials[3].SetActive(true);
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "�������λ�������ɶ�Ӧ�Ĳ��Ͽ�");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_4);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_5);
            }
        }

        public void Guide2_5(object sender,GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Cup)
                    return;
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "ʹ�ñ��ӽ����ȷۺ͵���ţ�̺ϳ�Ϊ����");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_5);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_6);
            }
        }

        public void Guide2_6(object sender,GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args=(ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.HotLatte)
                    return;
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "ʹ����������");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_6);
                GameEntry.Event.Subscribe(GameStateEventArgs.EventId, Guide3_1);
            }
        }

        public void Guide3_1(object sender,GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            if (args.GameState == GameState.Special)
            {
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "��������һ��������");
                GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, Guide3_1);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide3_2);
            }
        }

        public void Guide3_2(object sender,GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                if (baseCompenent.NodeTag != NodeTag.HotLatte)
                    return;
                materials[8].SetActive(true);
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "����ǲ�λ�������ɶ�Ӧ�Ĳ��Ͽ�");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide3_2);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide3_3);
            }
        }

        public void Guide3_3(object sender,GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Sugar)
                    return;
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "����ֱ�ӷ����������ϣ���ô���Ⱦͻ�ϳ�Ϊ���ǵĿ���");
                Invoke(nameof(Guide3_4), 2f);
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide3_3);
            }
        }

        public void Guide3_4()
        {
            GameEntry.UI.OpenUIForm(UIFormId.PopTips, "������Ŀ��Ƚ������ӿ��ȵļ۸񣬵���ע�⣺ֻ�ж����о��е�����Ż��������ȵļ۸�");
            Invoke(nameof(Guide3_5), 2f);
        }

        public void Guide3_5()
        {
            GameEntry.UI.OpenUIForm(UIFormId.PopTips, "��ô�������϶�����Ӧ�Ķ�����");
        }
    }

}