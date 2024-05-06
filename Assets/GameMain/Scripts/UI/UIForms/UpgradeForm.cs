using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using DG.Tweening;

namespace GameMain
{
    public class UpgradeForm :UIFormLogic
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Button upgradeBtn;
        [SerializeField] private Transform canvas;
        [SerializeField] private Text statisticsText;
        [SerializeField] private Text upgradeText;
        [SerializeField] private Text moneyText;
        [SerializeField] private List<Image> levelImages = new List<Image>();
        [SerializeField] private List<Text> levelTexts = new List<Text>();
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            canvas.localPosition = Vector3.up * 1080f;
            canvas.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);

            exitBtn.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
            upgradeBtn.onClick.AddListener(Upgrade);

            CheckUpgrade();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            exitBtn.onClick.RemoveAllListeners();
            upgradeBtn.onClick.RemoveAllListeners();
        }

        private void CheckUpgrade()
        {
            statisticsText.text = string.Format("���A�����ȣ�{0}\n���B�����ȣ�{1}\n���C�����ȣ�{2}",
            GameEntry.Utils.PlayerData.acoffee,
            GameEntry.Utils.PlayerData.acoffee +
            GameEntry.Utils.PlayerData.bcoffee,
            GameEntry.Utils.PlayerData.acoffee +
            GameEntry.Utils.PlayerData.bcoffee +
            GameEntry.Utils.PlayerData.ccoffee);

            for (int i = 0; i < levelTexts.Count; i++)
            {
                levelTexts[i].text = GameEntry.DataTable.GetDataTable<DRUpgrade>().GetDataRow(i + 1).Text;
                levelTexts[i].text = levelTexts[i].text.Replace("\\n", "\n");
                levelImages[i].color = Color.gray;
            }
            levelImages[GameEntry.Utils.PlayerData.cafeID - 1].color = Color.white;

            DRUpgrade dRUpgrade = GameEntry.DataTable.GetDataTable<DRUpgrade>().GetDataRow(GameEntry.Utils.PlayerData.cafeID);
            if (dRUpgrade.UpgradeID == 0)
            {
                upgradeBtn.interactable = false;
            }
            else
            {
                upgradeBtn.interactable = true;
                upgradeText.text = string.Empty;
                if (dRUpgrade.ACoffee > GameEntry.Utils.PlayerData.acoffee)
                {
                    upgradeText.text += string.Format("<color=red>A�������������{0}</color>\n", dRUpgrade.ACoffee);
                    upgradeBtn.interactable = false;
                }
                else
                {
                    upgradeText.text += string.Format("A�������������{0}\n", dRUpgrade.ACoffee);
                }
                if (dRUpgrade.BCoffee > GameEntry.Utils.PlayerData.bcoffee+ GameEntry.Utils.PlayerData.acoffee)
                {
                    upgradeText.text += string.Format("<color=red>B�������������{0}</color>\n", dRUpgrade.BCoffee);
                    upgradeBtn.interactable = false;
                }
                else
                {
                    upgradeText.text += string.Format("B�������������{0}\n", dRUpgrade.BCoffee);
                }
                if (dRUpgrade.CCoffee > GameEntry.Utils.PlayerData.ccoffee+ GameEntry.Utils.PlayerData.bcoffee + GameEntry.Utils.PlayerData.acoffee)
                {
                    upgradeText.text += string.Format("<color=red>C�������������{0}</color>", dRUpgrade.CCoffee);
                    upgradeBtn.interactable = false;
                }
                else
                {
                    upgradeText.text += string.Format("C�������������{0}", dRUpgrade.CCoffee);
                }
                if (dRUpgrade.Money > GameEntry.Utils.PlayerData.money)
                {
                    moneyText.text = string.Format("<color=red>�����Ǯ��{0}</color>", dRUpgrade.Money);
                    upgradeBtn.interactable = false;
                }
                else
                {
                    moneyText.text = string.Format("�����Ǯ��{0}", dRUpgrade.Money);
                }
                if (dRUpgrade.UpgradeID == 0)
                {
                    moneyText.text = string.Format("<color=red>�޷�����</color>");
                    upgradeBtn.interactable = false;
                }
            }
        }

        private void Upgrade()
        {
            DRUpgrade dRUpgrade = GameEntry.DataTable.GetDataTable<DRUpgrade>().GetDataRow(GameEntry.DataTable.GetDataTable<DRUpgrade>().GetDataRow(GameEntry.Utils.PlayerData.cafeID).UpgradeID);
            GameEntry.Utils.Money -= GameEntry.DataTable.GetDataTable<DRUpgrade>().GetDataRow(GameEntry.Utils.PlayerData.cafeID).Money;
            GameEntry.Utils.PricePower = dRUpgrade.Increase/100f;
            GameEntry.Utils.PlayerData.cafeID = dRUpgrade.Id;
            string[] recipes = dRUpgrade.UnlockCoffee.Split('-');
            GameEntry.Player.AddRecipes(recipes);
            CheckUpgrade();
        }
    }
}
