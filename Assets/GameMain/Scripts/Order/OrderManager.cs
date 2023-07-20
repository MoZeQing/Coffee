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
            if (OrderData.Check())
            {
                GameEntry.Event.FireNow(this, OrderEventArgs.Create(OrderData));
                //ProcedureMain main = (ProcedureMain)GameEntry.Procedure.CurrentProcedure;
                //main.Level(this);
            }
        }

        public void SetOrder(int index)
        {
            IDataTable<DROrder> dtOrder = GameEntry.DataTable.GetDataTable<DROrder>();
            DROrder drOrder = dtOrder.GetDataRow(index);
            OrderData = new OrderData(drOrder);
            GameEntry.Event.Fire(this, OrderEventArgs.Create(mOrderData));//用于更新UI信息的事件，需要保证线程安全
        }

        public void SetOrder(OrderData order)
        {
            OrderData = order;
            GameEntry.Event.Fire(this, OrderEventArgs.Create(mOrderData));//用于更新UI信息的事件，需要保证线程安全
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            BaseCompenent baseCompenent = null;
            if (collision.TryGetComponent<BaseCompenent>(out baseCompenent))
            {
                if (baseCompenent.transform.parent.GetComponent<BaseNode>().NodeData == null)
                    return;
                NodeData nodeData = baseCompenent.transform.parent.GetComponent<BaseNode>().NodeData;
                switch (nodeData.NodeTag)
                {
                    case NodeTag.Espresso:
                        OrderData.Espresso -= 1;
                        break;
                    case NodeTag.ConPanna:
                        OrderData.ConPanna -= 1;
                        break;
                    case NodeTag.Mocha:
                        OrderData.Mocha -= 1;
                        break;
                    case NodeTag.WhiteCoffee:
                        OrderData.WhiteCoffee -= 1;
                        break;
                    case NodeTag.CafeAmericano:
                        OrderData.CafeAmericano -= 1;
                        break;
                    case NodeTag.Latte:
                        OrderData.Latte -= 1;
                        break;
                    default:
                        return;
                }
                GameEntry.Entity.HideEntity(nodeData.Id);
                GameEntry.Event.FireNow(this, OrderEventArgs.Create(OrderData));
            }
        }
    }

    public class MaterialData
    { 
        public int Water
        {
            get;
            set;
        }

        public int Sugaer
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
    }

    public class OrderData
    {
        /// <summary>
        ///获取当天对话 
        /// </summary>
        public string Dialog
        {
            get;
            set;
        }

        /// <summary>
        /// 获取浓缩咖啡。
        /// </summary>
        public int Espresso
        {
            get;
            set;
        }

        /// <summary>
        /// 获取拿铁。
        /// </summary>
        public int Latte
        {
            get;
            set;
        }

        /// <summary>
        /// 获取美式咖啡。
        /// </summary>
        public int CafeAmericano
        {
            get;
            set;
        }

        /// <summary>
        /// 获取白咖啡。
        /// </summary>
        public int WhiteCoffee
        {
            get;
            set;
        }

        /// <summary>
        /// 获取摩卡。
        /// </summary>
        public int Mocha
        {
            get;
            set;
        }

        /// <summary>
        /// 获取康宝蓝。
        /// </summary>
        public int ConPanna
        {
            get;
            set;
        }

        public bool Check()
        {
            return (ConPanna <= 0 &&
                CafeAmericano <= 0 &&
                Mocha <= 0 &&
                WhiteCoffee <= 0 &&
                Latte <= 0 &&
                Espresso <= 0);
        }

        public OrderData() { }
        public OrderData(DROrder order)
        {
            ConPanna = order.ConPanna;
            Mocha = order.Mocha;
            WhiteCoffee = order.WhiteCoffee;
            Latte = order.Latte;
            CafeAmericano = order.CafeAmericano;
            Espresso = order.Espresso;
            Dialog = order.Dialog;
        }
    }
}
