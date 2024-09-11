using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.DataTable;

namespace GameMain
{
    public class MarketForm : ShopForm
    {
        [Header("��������Ķ���")]
        [SerializeField] private ItemKind kind;
        [Header("Ͷ�ʽ��")]
        [SerializeField] private int invest = 500;
        [Header("Ͷ������")]
        [SerializeField] private Button investBtn;
        [SerializeField] private Text investText;
        [SerializeField] private Text financialText;

        protected override void OnInitValue(object userData)
        {
            base.OnInitValue(userData);
            dRItems.Clear();
            foreach (DRItem item in GameEntry.DataTable.GetDataTable<DRItem>().GetAllDataRows())
            {
                if ((ItemKind)item.Kind != kind)
                    continue;
                dRItems.Add(item);
            }
            investBtn.onClick.AddListener(InvestBtn_OnClick);
        }
        private void InvestBtn_OnClick()
        {
            if (GameEntry.Player.Money < invest)
            {
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, $"��Ľ�Ǯ����{invest}");
                return;
            }
            GameEntry.UI.OpenUIForm(UIFormId.OkTips, InvestBtn_OnConfirm, "��ȷ��ҪͶ����");
        }
        protected override void OnConfirm(DRItem itemData)
        {
            base.OnConfirm(itemData);
        }
        private void InvestBtn_OnConfirm()
        {
            GameEntry.Player.Investment += invest;
            GameEntry.Player.Money -= invest;
            GameEntry.Utils.AddFlag("Invest");
            UpdateItem();
        }
        protected override void UpdateItem()
        {
            financialText.text = $"Ͷ�ʶ{GameEntry.Player.Investment}";
            investText.text = $"Ͷ�ʻر���ÿ�죩��{GameEntry.Player.Investment / invest + 9}%";
            investBtn.interactable = !GameEntry.Utils.CheckFlag("Invest");

            base.UpdateItem();
        }
    }
}

