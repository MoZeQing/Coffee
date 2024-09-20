using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class Guide : MonoBehaviour
    {

        [SerializeField] private List<GameObject> materials = new List<GameObject>();

        void Start()
        {
            GameEntry.Event.Subscribe(GameStateEventArgs.EventId, Guide1_1);
        }

        // Update is called once per frame

        public void Guide1_1(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            if (args.GameState == GameState.Special)
            {
                //��ʼ��

                materials[(int)NodeTag.CoffeeBean].SetActive(true);
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("������ȶ���λ�������ɶ�Ӧ�Ĳ��Ͽ�", WorkTips.None));
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
                    Position = new Vector3(-1.86f, -3.68f+1.6f, 0f)
                });
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("ʹ����ĥ�������ȷ���ĥΪ�ֿ��ȷ�", WorkTips.None));
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
                if (baseCompenent.NodeTag != NodeTag.CoarseGroundCoffee)
                    return;
                materials[1].SetActive(true);//ˮ
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("���ˮ��λ�������ɶ�Ӧ�Ĳ��Ͽ�", WorkTips.None));
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
                    Position = new Vector3(2.96f, -3.74f+1.6f, 0f)
                });
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("ʹ�ü�������ˮ����", WorkTips.None));
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
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.FrenchPress)
                {
                    Position = new Vector3(0.21f, -3.66f+1.6f, 0f)
                });
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("����ˮ�ʹֿ��ȷ۷��õ���ֽ©���У�����Ũ������", WorkTips.None));
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
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("�϶����ɵĿ��ȿ��Ƶ���ߵĶ�Ӧ������", WorkTips.None));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide1_6);
                GameEntry.Event.Subscribe(GameStateEventArgs.EventId, Guide2_1);
            }
        }
        //�����ֿ��Ȱ汾������
        public void Guide2_1(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            if (args.GameState == GameState.Special)
            {
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("���ڣ�����һ���µ�Ũ�����Ȱ�", WorkTips.None));
                GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, Guide2_1);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_2);
            }
        }

        public void Guide2_2(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Espresso)
                    return;
                materials[2].SetActive(true);
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("���ţ�̲�λ�������ɶ�Ӧ�Ĳ��Ͽ�", WorkTips.None));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_2);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_3);
            }
        }

        public void Guide2_3(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Milk)
                    return;
                GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, NodeTag.Stirrer)
                {
                    Position = new Vector3(5.26f, -3.73f+1.6f, 0f)
                });
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("ʹ�ý�������ţ�̴�", WorkTips.None));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_3);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_4);
            }
        }

        public void Guide2_4(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.LowFoamingMilk)
                    return;
                materials[3].SetActive(true);
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("�������λ�������ɶ�Ӧ�Ĳ��Ͽ�", WorkTips.None));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_4);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_5);
            }
        }

        public void Guide2_5(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Cup)
                    return;
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("ʹ�ñ��ӽ�Ũ�����Ⱥ͵���ţ�̺ϳ�Ϊ������ŵ", WorkTips.None));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_5);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide2_6);
            }
        }

        public void Guide2_6(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.Kapuziner)
                    return;
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("ʹ�ÿ�����ŵ����", WorkTips.None));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_6);
                GameEntry.Event.Subscribe(GameStateEventArgs.EventId, Guide3_1);
            }
        }

        public void Guide3_1(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            if (args.GameState == GameState.Special)
            {
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("��������һ��ϸŨ�����ȣ��������������ϸ���ȷ�", WorkTips.None));
                GameEntry.Player.AddRecipe(1);
                GameEntry.Event.Unsubscribe(GameStateEventArgs.EventId, Guide3_1);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide3_3);
                Invoke(nameof(Guide3_2), 2f);
            }
        }
        public void Guide3_2()
        {
            GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "��νϸ���ȷۣ���������ĥһ�εĴֿ��ȷ�");
        }
        //���ڣ��������ϸ������
        public void Guide3_3(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.FineGroundCoffee)
                    return;
                materials[7].SetActive(true);
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("�������ϸ�汾�ı�������ŵ�ɣ�\n<size=24>�������䷽���ɻ󣬿��Ե�����½ǵ������ֲ�鿴�䷽��</size>", WorkTips.None));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide3_3);
                GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, Guide3_5);
            }
        }

        //public void Guide3_3(object sender, GameEventArgs e)
        //{
        //    ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
        //    BaseCompenent baseCompenent = null;
        //    if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
        //    {
        //        if (baseCompenent.NodeTag != NodeTag.Sugar)
        //            return;
        //        GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
        //        GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "����ֱ�ӷ����������ϣ���ô���Ⱦͻ�ϳ�Ϊ���ǵĿ���");
        //        Invoke(nameof(Guide3_4), 2f);
        //        GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide3_3);
        //    }
        //}

        //public void Guide3_4()
        //{
        //    GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
        //    GameEntry.UI.OpenUIForm(UIFormId.PopTips, "������Ŀ��Ƚ������ӿ��ȵļ۸񣬵���ע�⣺ֻ�ж����о��е�����Ż��������ȵļ۸�");
        //    Invoke(nameof(Guide3_5), 2f);
        //}

        public void Guide3_5(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = (ShowEntitySuccessEventArgs)e;
            BaseCompenent baseCompenent = null;
            if (args.Entity.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.NodeTag != NodeTag.IceKapuziner)
                    return;
                GameEntry.Event.FireNow(WorkEventArgs.EventId, WorkEventArgs.Create("��ô�������϶�����Ӧ�Ķ�����", WorkTips.None));
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide3_5);
            }
        }

        //��Ҫ�ڵ������ʱ������Ϣ
        public void Guide4_1()
        {
            GameEntry.UI.CloseUIForm(UIFormId.MainForm);
        }

        public void Guide4_2()
        {

        }
    }
}