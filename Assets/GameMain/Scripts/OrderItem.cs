using GameMain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.DataTable;
using UnityEngine.SocialPlatforms;
using XNode.Examples.RuntimeMathNodes;

public class OrderItem : Entity
{
    [SerializeField] private Image coffeeItem;
    [SerializeField] private Text coffeeName;
    [SerializeField] private Text sugar;
    [SerializeField] private Text condensedMilk;
    [SerializeField] private Text salt;
    [SerializeField] private Text orderTime;
    [SerializeField] private Button exitBtn;

    private OrderItemData mOrderItemData = null;
    private OrderData mOrderData = null;
    private float nowTime=0f;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        Transform orderCanvas = this.transform.Find("OrderForm");
        coffeeItem = orderCanvas.Find("ItemText").GetComponent<Image>();
        sugar = orderCanvas.Find("Sugar").GetComponent<Text>();
        condensedMilk = orderCanvas.Find("CondensedMilk").GetComponent<Text>();
        salt = orderCanvas.Find("Salt").GetComponent<Text>();
        orderTime=orderCanvas.Find("TimeText").GetComponent<Text>();
        //exitBtn = orderCanvas.Find("Exit").GetComponent<Button>();
        coffeeName = orderCanvas.Find("ItemText").GetComponent<Text>();

        //exitBtn.onClick.AddListener(OnExit);
    }
    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        mOrderItemData = (OrderItemData)userData;
        mOrderData = mOrderItemData.OrderData;
        DRNode dRNode = GameEntry.DataTable.GetDataTable<DRNode>().GetDataRow((int)mOrderData.NodeTag);
        //coffeeItem.sprite = GameEntry.Utils.nodeImage[(int)mOrderData.NodeTag];
        coffeeName.text = dRNode.Description;
        sugar.color = mOrderData.Sugar ? Color.black : Color.clear;
        condensedMilk.color = mOrderData.CondensedMilk ? Color.black : Color.clear;
        salt.color = mOrderData.Salt ? Color.black : Color.clear; 
        nowTime = mOrderData.OrderTime;
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        nowTime-=Time.deltaTime;
        if (nowTime < 0)
            orderTime.text = "��";
        else
            orderTime.text = Mathf.Floor(nowTime).ToString();
        if (nowTime <= 0f&&nowTime>-1f)
        {
            nowTime = -1;
            OnExit();
        }
    }

    protected override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);
        nowTime = 9999f;
    }

    private void OnExit()
    {
        GameEntry.Event.FireNow(this, OrderEventArgs.Create(mOrderData, 0));
        GameEntry.Entity.HideEntity(this.Entity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseCompenent baseCompenent = null;
        if (collision.TryGetComponent<BaseCompenent>(out baseCompenent))
        {
            if (baseCompenent.NodeTag == mOrderData.NodeTag)
            {
                //��������
                int income = 0;
                IDataTable<DRNode> dtNode=GameEntry.DataTable.GetDataTable<DRNode>();
                income = dtNode.GetDataRow((int)mOrderData.NodeTag).Price;
                income += mOrderData.Sugar ? 2 : 0;
                income += mOrderData.CondensedMilk ? 5 : 0;
                income += mOrderData.Salt ? 3 : 0;
                GameEntry.Event.FireNow(this, OrderEventArgs.Create(mOrderData, income));
                GameEntry.Entity.HideEntity(baseCompenent.transform.parent.GetComponent<BaseNode>().Entity);
                GameEntry.Entity.HideEntity(this.Entity);
            }
        }
    }
}
