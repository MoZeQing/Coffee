using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class CupboradForm : UIFormLogic
    {
        [SerializeField] private Transform mCanvas;
        [SerializeField] private Button exitBtn;
        [SerializeField] private GameObject itemItem;
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private Toggle allToggle;
        [SerializeField] private Toggle materialsToggle;
        [SerializeField] private Toggle itemToggle;
        [SerializeField] private Toggle onceItemToggle;

        private List<Item> mItems = new List<Item>();
        private List<PlayerItemData> mItemDatas=new List<PlayerItemData>();
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mItemDatas = GameEntry.Utils.PlayerData.items;
            allToggle.isOn = true;
            allToggle.onValueChanged.AddListener(OnAllChange);
            materialsToggle.onValueChanged.AddListener(OnMaterialsChange);
            itemToggle.onValueChanged.AddListener(OnItemChange);
            onceItemToggle.onValueChanged.AddListener(OnInstrumentChange);
            exitBtn.onClick.AddListener(OnExit);
            ShowItems();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            allToggle.onValueChanged.RemoveAllListeners();
            materialsToggle.onValueChanged.RemoveAllListeners();
            itemToggle.onValueChanged.RemoveAllListeners();
            onceItemToggle.onValueChanged.RemoveAllListeners();
            exitBtn.onClick.RemoveAllListeners();
            ClearItems();
        }

        public void ShowItems()
        {
            ShowItems(mItemDatas);
        }

        public void ShowItems(List<PlayerItemData> itemDatas)
        {
            foreach (PlayerItemData itemData in itemDatas)
            {
                if ((int)itemData.itemTag > 100)
                    continue;
                if (itemData.itemNum <= 0)
                    continue;
                GameObject go = Instantiate(itemItem, mCanvas);
                Item item =go.GetComponent<Item>();
                item.SetData(itemData);
                item.SetClick(OnClick);
                mItems.Add(item);
            }
        }

        public void ClearItems()
        {
            foreach (Item item in mItems)
            { 
                GameObject.Destroy(item.gameObject);
            }
            mItems.Clear();
        }

        private void OnClick(ItemData itemData)
        { 
            //弹出一个确认弹窗
        }

        private void OnAllChange(bool value)
        {
            if(value)
                OnFilterChange(ItemKind.None);
        }

        private void OnMaterialsChange(bool value) 
        {
            if (value)
                OnFilterChange(ItemKind.Materials);
        }

        private void OnItemChange(bool value)
        {
            if (value)
                OnFilterChange(ItemKind.Item);
        }

        private void OnInstrumentChange(bool value)
        {
            if (value)
                OnFilterChange(ItemKind.Instrument);
        }

        private void OnFilterChange(ItemKind kind)
        { 
            ClearItems();
            List<PlayerItemData> newItems=new List<PlayerItemData>();
            foreach (PlayerItemData itemData in mItemDatas)
            {
                if (itemData.itemKind == kind)
                {
                    newItems.Add(itemData);
                    continue;
                }
                if (kind==ItemKind.None)
                {
                    newItems.Add(itemData);
                    continue;
                }
            }
            ShowItems(newItems);
        }

        public void OnExit()
        {
            GameEntry.UI.CloseUIForm(this.UIForm);
        }
    }
}