using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using DG.Tweening;

namespace GameMain
{
    public class ClosetForm : UIFormLogic
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button okBtn;
        [SerializeField] private Button leftBtn;
        [SerializeField] private Button rightBtn;
        [SerializeField] private Text infoText;
        [SerializeField] private Image closetImg;
        [Header("ê��")]
        [SerializeField] private Image[] closetItems = new Image[4];//0��1��2��3��
        [SerializeField] private Transform[] canvas = new Transform[4];//0��1��2��3��

        private List<DRItem> dRItems = new List<DRItem>();
        private int index = 0;
        private int itemIndex = 0;


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            okBtn.onClick.AddListener(OkBtn);

            dRItems.Clear();
            foreach (DRItem item in GameEntry.DataTable.GetDataTable<DRItem>().GetAllDataRows())
            {
                if ((ItemKind)item.Kind != ItemKind.Clothes)
                    continue;
                dRItems.Add(item);
            }

            leftBtn.onClick.AddListener(Left);
            rightBtn.onClick.AddListener(Right);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
            okBtn.onClick.RemoveAllListeners();
            leftBtn.onClick.RemoveAllListeners();
            rightBtn.onClick.RemoveAllListeners();
        }
        private void Right()
        {
            index--;
            if (index < 0) index = dRItems.Count - 1;
            itemIndex--;
            if (itemIndex < 0) itemIndex = closetItems.Length - 1;

            DRItem cloest = dRItems[index];
            Image img = closetItems[0];
            closetItems[0].sprite = Resources.Load<Sprite>(dRItems[index].ImagePath);
            Debug.Log(dRItems[index].ImagePath);
            closetItems[2].transform.DOMove(canvas[3].position, 0.5f).SetEase(Ease.OutExpo);
            closetItems[2].transform.DOScale(Vector3.one * 0.8f, 0.5f).SetEase(Ease.OutExpo);
            closetItems[2].color = Color.grey;
            closetItems[3].transform.DOMove(canvas[0].position, 0.5f).SetEase(Ease.OutExpo);
            closetItems[0].transform.DOMove(canvas[1].position, 0.5f).SetEase(Ease.OutExpo);
            closetItems[1].transform.DOMove(canvas[2].position, 0.5f).SetEase(Ease.OutExpo);
            closetItems[1].transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);
            closetItems[1].color = Color.white;
            closetItems[1].transform.SetSiblingIndex(4);

            closetItems[0] = closetItems[3];
            closetItems[3] = closetItems[2];
            closetItems[2] = closetItems[1];
            closetItems[1] = img;

            int closet = index - 1;
            if (closet == -1) closet = dRItems.Count - 1;
            infoText.text = dRItems[closet].Info;
        }
        private void Left()
        {
            index++;
            if (index >= dRItems.Count) index = 0;
            itemIndex++;
            if (itemIndex >= closetItems.Length) itemIndex = 0;

            DRItem cloest = dRItems[index];
            Image img = closetItems[0];
            closetItems[0].sprite = Resources.Load<Sprite>(dRItems[index].ImagePath);
            closetItems[2].transform.DOMove(canvas[1].position, 1f).SetEase(Ease.OutExpo);
            closetItems[2].transform.DOScale(Vector3.one * 0.8f, 1f).SetEase(Ease.OutExpo);
            closetItems[2].color = Color.grey;
            closetItems[3].transform.DOMove(canvas[2].position, 1f).SetEase(Ease.OutExpo);
            closetItems[0].transform.DOMove(canvas[3].position, 1f).SetEase(Ease.OutExpo);
            closetItems[1].transform.DOMove(canvas[0].position, 1f).SetEase(Ease.OutExpo);
            closetItems[3].transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);
            closetItems[3].color = Color.white;
            closetItems[3].transform.SetSiblingIndex(4);

            closetItems[0] = closetItems[1];
            closetItems[1] = closetItems[2];
            closetItems[2] = closetItems[3];
            closetItems[3] = img;

            int closet = index - 1;
            if (closet == -1) closet = dRItems.Count - 1;
            infoText.text = dRItems[closet].Info;
        }
        private void OkBtn()
        {
            int closet = index-1;
            if (closet== -1) closet = dRItems.Count-1;
            GameEntry.Utils.closet = dRItems[closet].Id;
        }
    }
}