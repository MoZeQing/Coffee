using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;

namespace GameMain
{
    public class RestaurantForm : UIFormLogic
    {
        [SerializeField] private Button leftBtn;
        [SerializeField] private Button rightBtn;
        [SerializeField] private Button exitBtn;
        [SerializeField] private Text pageText;
        [SerializeField] private PurchaseForm purchaseForm;
        [SerializeField] private List<ShopItem> mItems = new List<ShopItem>();

        private List<DRItem> dRItems = new List<DRItem>();
        private DRItem dRItem;
        private int index = 0;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            dRItems.Clear();
            foreach (DRItem item in GameEntry.DataTable.GetDataTable<DRItem>().GetAllDataRows())
            {
                if ((ItemKind)item.Kind != ItemKind.Cake)
                    continue;
                dRItems.Add(item);
            }
            exitBtn.onClick.AddListener(OnExit);
            leftBtn.onClick.AddListener(Left);
            rightBtn.onClick.AddListener(Right);

            index = 0;
            ShowItems();
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
            leftBtn.onClick.RemoveAllListeners();
            rightBtn.onClick.RemoveAllListeners();
        }
        private void ShowItems()
        {
            leftBtn.interactable = (index != 0);

            for (int i = 0; i < mItems.Count; i++)
            {
                if (index < dRItems.Count)
                {
                    mItems[i].SetData(dRItems[index]);
                    mItems[i].SetClick(OnClick);
                }
                else
                    mItems[i].Hide();
                index++;
            }

            rightBtn.interactable = index < dRItems.Count;
            pageText.text = (index / mItems.Count).ToString();
        }
        private void Right()
        {
            ShowItems();
        }

        private void Left()
        {
            index -= 2 * mItems.Count;
            ShowItems();
        }
        private void OnClick(DRItem itemData)
        {
            dRItem = itemData;
            if (itemData.Price > GameEntry.Utils.Money)
                GameEntry.UI.OpenUIForm(UIFormId.PopTips, "����ʽ���");
            else
                GameEntry.UI.OpenUIForm(UIFormId.OkTips, UpdateItem, "��ȷ��Ҫ������");
        }
        private void UpdateItem()
        {
            GameEntry.Utils.Money -= dRItem.Price;
            GameEntry.Utils.Favor += dRItem.Favor;
            GameEntry.Utils.Energy += dRItem.Energy;

            index -= mItems.Count;
            ShowItems();
        }
        private void OnExit()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChangeForm, this);
            GameEntry.Utils.outingBefore = false;
            GameEntry.Dialog.StoryUpdate();
            OnGameStateChange();
        }

        private void OnGameStateChange()
        {
            GameEntry.Utils.Location = OutingSceneState.Home;
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
}
