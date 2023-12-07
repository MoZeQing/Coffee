using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;

namespace GameMain
{
    public class UnlockForm : UIFormLogic
    {
        [SerializeField] private Image image;
        [SerializeField] private Text title;
        [SerializeField] private Text text;
        [SerializeField] private Button button;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            RecipeData recipe = userData as RecipeData;
            if (!recipe.IsCoffee)
            {
                Debug.LogError("����Ľ������������Ӧ��Ϊ�������ȵ��䷽");
                GameEntry.UI.CloseUIForm(this.UIForm);
            }
            //��ȡ���еĿ��Ȳ�Ʒ
            
            //image.sprite= ����DR���ȡͼƬ
            //text.text = recipe.itemName;
            button.onClick.AddListener(() => GameEntry.UI.CloseUIForm(this.UIForm));
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            button.onClick.RemoveAllListeners();
        }
    }
}
