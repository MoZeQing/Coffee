using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;
using GameFramework.DataTable;
using GameMain;


namespace GameMain
{
    public class ClothingForm : ShopForm
    {
        [Header("��������")]
        [SerializeField] private Text salaryInfoText;
        [SerializeField] private Text salaryText;
        [SerializeField] private Button salaryBtn;
        [Header("���ʶ��")]
        [SerializeField] private int salary;

        protected override void OnInitValue(object userData)
        {
            base.OnInitValue(userData);
            dRItems.Clear();
            foreach (DRItem item in GameEntry.DataTable.GetDataTable<DRItem>().GetAllDataRows())
            {
                if ((ItemKind)item.Kind != ItemKind.Clothes)
                    continue;
                if ((ItemTag)item.Id == ItemTag.Closet1)
                    continue;
                if ((ItemTag)item.Id == ItemTag.Closet2)
                    continue;
                if ((ItemTag)item.Id == ItemTag.Closet6)
                    continue;
                dRItems.Add(item);
            }

            salaryBtn.onClick.AddListener(SalaryBtn_OnClick);
        }
        protected override void UpdateItem()
        {
            salaryInfoText.text = $"����,����һ������������{salary}��Ǯ";
            salaryText.text = $"+{salary}";
            salaryBtn.interactable = !GameEntry.Utils.CheckDayPassFlag("Work");
            base.UpdateItem();
        }
        private void SalaryBtn_OnClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.OkTips, SalaryBtn_OnConfirm, "��ȷ��Ҫ������");
        }

        private void SalaryBtn_OnConfirm()
        {
            GameEntry.Player.Ap--;
            GameEntry.Player.Money += salary;
            GameEntry.Utils.AddDayPassFlag("Work");
            UpdateItem();
        }
    }
}
