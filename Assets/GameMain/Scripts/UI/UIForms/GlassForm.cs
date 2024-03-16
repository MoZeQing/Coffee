using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using GameFramework.Event;

namespace GameMain
{
    public class GlassForm : UIFormLogic
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button leftBtn;
        [SerializeField] private Button rightBtn;
        [SerializeField] private Text pageText;
        [SerializeField] private Text headerField;
        [SerializeField] private Text contentField;
        [SerializeField] private PurchaseForm purchaseForm;
        [SerializeField] private List<GlassItem> mItems = new List<GlassItem>();

        private List<DRItem> dRItems = new List<DRItem>();
        private ItemData mItemData = new ItemData();
        private int index = 0;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            dRItems.Clear();
            foreach (DRItem item in GameEntry.DataTable.GetDataTable<DRItem>().GetAllDataRows())
            {
                if ((ItemKind)item.Kind != ItemKind.Instrument)
                    continue;
                dRItems.Add(item);
            }

            leftBtn.interactable = false;
            rightBtn.interactable = dRItems.Count > mItems.Count;

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
        }

        private void ShowItems()
        {
            for (int i = 0; i < mItems.Count; i++)
            {
                if (index < dRItems.Count)
                    mItems[i].SetData(dRItems[index]);
                else
                    mItems[i].Hide();
                index++;
            }
            leftBtn.interactable = index != 0;
            rightBtn.interactable = index < dRItems.Count;
            pageText.text = (index / mItems.Count).ToString();
        }
        private void OnClick(ItemData itemData)
        {
            //purchaseForm.SetData(itemData);
            purchaseForm.gameObject.SetActive(true);
            purchaseForm.SetClick(UpdateItem);
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
        private void UpdateItem()
        {
            index -= mItems.Count;
            ShowItems();
        }
        private void OnTouch(bool flag, ItemData itemData)
        {
            if (flag)
            {
                headerField.text = itemData.itemName;
                contentField.text = itemData.itemInfo;
            }
            else
            {
                headerField.text = string.Empty;
                contentField.text = string.Empty;
            }
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
