using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using DG.Tweening;

namespace GameMain
{
    public class UnlockForm : BaseForm
    {
        [SerializeField] private Transform canvas;
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

            canvas.localScale = Vector3.one * 0.1f;
            canvas.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutExpo);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            button.onClick.RemoveAllListeners();
        }
    }
}
