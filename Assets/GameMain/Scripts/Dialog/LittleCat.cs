using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class LittleCat : MonoBehaviour,IPointerClickHandler
    {
        public List<SpriteRenderer> littleCats = new List<SpriteRenderer>();
        public ScenePosTag sceneTag;

        private void Start()
        {
            littleCats.Clear();
            for (int i = 0; i < this.transform.childCount; i++)
            {
                littleCats.Add(this.transform.GetChild(i).GetComponent<SpriteRenderer>());
            }
            //littleCats.Add(this.transform.Find("Cat1").GetComponent<SpriteRenderer>());
            //littleCats.Add(this.transform.Find("Cat2").GetComponent<SpriteRenderer>());
            //littleCats.Add(this.transform.Find("Cat3").GetComponent<SpriteRenderer>());
        }
        public void OnPointerClick(PointerEventData pointerEventData)
        {
            //��ʾ��Ӧ���ɵ�UI
            GameEntry.Event.FireNow(this, LittleCatEventArgs.Create(sceneTag));
        }
        public void ShowLittleCat(ScenePosTag posTag)
        {
            HideLittleCat();
            sceneTag= posTag;
            littleCats[(int)sceneTag].gameObject.SetActive(true);
        }
        public void ShowLittleCat()
        {
            HideLittleCat();
            littleCats[(int)sceneTag].gameObject.SetActive(true);
        }
        public void HideLittleCat()
        {
            foreach (SpriteRenderer sprite in littleCats)
            { 
                sprite.gameObject.SetActive(false);
            }
        }
    }

    public enum ScenePosTag
    { 
        Left,//��
        Middle,//��
        Right//��
    }
}
