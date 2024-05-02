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

        public void Guide1_1(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            if (args.GameState == GameState.Special)
            {
                //��ʼ��
                materials.Add(GameObject.Find("Materials").transform.Find("CoffeeBean").gameObject);
                materials.Add(GameObject.Find("Materials").transform.Find("Water").gameObject);
                materials.Add(GameObject.Find("Materials").transform.Find("Milk").gameObject);
                materials.Add(GameObject.Find("Materials").transform.Find("Cup").gameObject);
                materials.Add(GameObject.Find("Materials").transform.Find("Sugar").gameObject);

                materials[(int)NodeTag.CoffeeBean].SetActive(true);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "������ȶ���λ�������ɶ�Ӧ�Ĳ��Ͽ�");
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
                    Position = Vector3.down * 6
                });
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "ʹ����ĥ�������ȷ���ĥΪ�ֿ��ȷ�");
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
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "���ˮ��λ�������ɶ�Ӧ�Ĳ��Ͽ�");
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
                    Position = Vector3.down * 6
                }) ;
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "ʹ�ü�������ˮ����");
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
                    Position = Vector3.down * 6
                });
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "����ˮ��ϸ���ȷ۷��õ����˺��У�����Ũ������");
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
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "�϶����ɵĿ��ȿ��Ƶ����ϽǵĶ�Ӧ������");
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
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "���ڣ�����һ���µ�Ũ�����Ȱ�");
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
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "���ţ�̲�λ�������ɶ�Ӧ�Ĳ��Ͽ�");
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
                    Position = Vector3.down * 6
                });
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "ʹ�ý�������ţ�̴�");
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
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "�������λ�������ɶ�Ӧ�Ĳ��Ͽ�");
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
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "ʹ�ñ��ӽ�Ũ�����Ⱥ͵���ţ�̺ϳ�Ϊ����");
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
                if (baseCompenent.NodeTag != NodeTag.HotLatte)
                    return;
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "ʹ����������");
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, Guide2_6);
                GameEntry.Event.Subscribe(GameStateEventArgs.EventId, Guide3_1);
            }
        }

        public void Guide3_1(object sender, GameEventArgs e)
        {
            GameStateEventArgs args = (GameStateEventArgs)e;
            if (args.GameState == GameState.Special)
            {
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "��������һ��ϸŨ�����ȣ��������������ϸ���ȷ�");
                GameEntry.Player.AddRecipe(2);
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
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.HighlightTips, "�������ϸ�汾�ı������ɣ�");
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
                if (baseCompenent.NodeTag != NodeTag.IceLatte)
                    return;
                GameEntry.UI.CloseUIForm(UIFormId.HighlightTips);
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "��ô�������϶�����Ӧ�Ķ�����");
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