using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.DataTable;
using UnityEngine.UI;
using GameFramework.Event;


namespace GameMain
{
    public class OrderManager : MonoBehaviour
    {
        private List<DROrder> orders = new List<DROrder>();
        private OrderData mOrderData = new OrderData();
        public OrderData OrderData
        {
            get
            {
                return mOrderData;
            }
            private set
            {
                mOrderData = value;
            }
        }

        private void Start()
        {
            //if (OrderData.Check())
            //{
            //    GameEntry.Event.FireNow(this, OrderEventArgs.Create(OrderData));
            //    ProcedureMain main = (ProcedureMain)GameEntry.Procedure.CurrentProcedure;
            //    main.Level(this);
            //}
        }

        public void SetOrder(int index)
        {
            //IDataTable<DROrder> dtOrder = GameEntry.DataTable.GetDataTable<DROrder>();
            //DROrder drOrder = dtOrder.GetDataRow(index);
            //OrderData = new OrderData(drOrder);
            //OrderData.NodeTag = NodeTag.None;
            //GameEntry.Event.Fire(this, OrderEventArgs.Create(mOrderData));//���ڸ���UI��Ϣ���¼�����Ҫ��֤�̰߳�ȫ
        }

        public void SetOrder(OrderData order)
        {
            //OrderData = order;
            //OrderData.NodeTag = NodeTag.None;
            //GameEntry.Event.Fire(this, OrderEventArgs.Create(mOrderData));//���ڸ���UI��Ϣ���¼�����Ҫ��֤�̰߳�ȫ
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Entity baseCompenent = null;
            //if (collision.TryGetComponent<Entity>(out baseCompenent))
            //{
            //    if (baseCompenent.transform.parent.GetComponent<BaseNode>() == null)
            //    {
            //        Debug.LogWarning("����û�иýű�");
            //        return; 
            //    }
            //    if (baseCompenent.transform.parent.GetComponent<BaseNode>().NodeData == null)
            //        return;
            //    NodeData nodeData = baseCompenent.transform.parent.GetComponent<BaseNode>().NodeData;
            //    switch (nodeData.NodeTag)
            //    {
            //        case NodeTag.Espresso:
            //            OrderData.Espresso -= 1;
            //            break;
            //        case NodeTag.IceEspresso:
            //            OrderData.Espresso -= 1;
            //            break;
            //        case NodeTag.SweetEspresso:
            //            OrderData.Espresso -= 1;
            //            break;
            //        case NodeTag.ConPanna:
            //            OrderData.ConPanna -= 1;
            //            break;
            //        case NodeTag.IceConPanna:
            //            OrderData.ConPanna -= 1;
            //            break;
            //        case NodeTag.SweetConPanna:
            //            OrderData.ConPanna -= 1;
            //            break;
            //        case NodeTag.Mocha:
            //            OrderData.Mocha -= 1;
            //            break;
            //        case NodeTag.IceMocha:
            //            OrderData.Mocha -= 1;
            //            break;
            //        case NodeTag.SweetMocha:
            //            OrderData.Mocha -= 1;
            //            break;
            //        case NodeTag.WhiteCoffee:
            //            OrderData.WhiteCoffee -= 1;
            //            break;
            //        case NodeTag.IceWhiteCoffee:
            //            OrderData.WhiteCoffee -= 1;
            //            break;
            //        case NodeTag.SweetWhiteCoffee:
            //            OrderData.WhiteCoffee -= 1;
            //            break;
            //        case NodeTag.CafeAmericano:
            //            OrderData.CafeAmericano -= 1;
            //            break;
            //        case NodeTag.IceCafeAmericano:
            //            OrderData.CafeAmericano -= 1;
            //            break;
            //        case NodeTag.SweetCafeAmericano:
            //            OrderData.CafeAmericano -= 1;
            //            break;
            //        case NodeTag.Latte:
            //            OrderData.Latte -= 1;
            //            break;
            //        case NodeTag.IceLatte:
            //            OrderData.Latte -= 1;
            //            break;
            //        case NodeTag.SweetLatte:
            //            OrderData.Latte -= 1;
            //            break;
            //        default:
            //            return;
            //    }
            //    GameEntry.Entity.HideEntity(nodeData.Id);
            //    OrderData.NodeTag= nodeData.NodeTag;
            //    GameEntry.Event.FireNow(this, OrderEventArgs.Create(OrderData));
            //}
        }
    }

    public class MaterialData
    { 
        public int Water
        {
            get;
            set;
        }

        public int CoffeeBean
        {
            get;
            set;
        }

        public int Milk
        {
            get;
            set;
        }

        public int ChocolateSyrup
        {
            get;
            set;
        }

        public int Cream
        {
            get;
            set;
        }
        public int Ice
        {
            get;
            set;
        }
        public int Sugar
        {
            get;
            set;
        }
    }
    [System.Serializable]
    public class OrderData
    {
        //��������
        public NodeTag NodeTag;
        public string NodeName;
        //�������ƺͺøж�
        public string friendName;
        public int friendFavor;
        //�������ȵ�ʱ��
        public float OrderTime;
        public string OrderChar;
        public int CharFavor;
        public bool Grind;//��ϸ��
        public bool Urgent;//�Ƿ�Ӽ�
        public bool Sugar;
        public bool CondensedMilk;
        public bool Salt;
    }
}
