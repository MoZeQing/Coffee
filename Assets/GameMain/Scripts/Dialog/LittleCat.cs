using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class LittleCat : MonoBehaviour,IPointerClickHandler
    {
        public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

        public SceneTag sceneTag;

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            //��ʾ��Ӧ���ɵ�UI
            GameEntry.UI.OpenUIForm(UIFormId.TeachingForm, sceneTag);
            this.gameObject.SetActive(false);
        }
    }

    public enum SceneTag
    { 
        Teaching,//����
        Working,//����
        Cupborad//�ֿ�
    }
}
