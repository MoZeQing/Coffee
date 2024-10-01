using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class CatCompenent : BaseCompenent
    {
        private NodeTag productTag;
        private bool productGrind;
        protected override void Compound()
        {
            //�㼶ˢ��
            mProgressBarRenderer.GetComponent<Canvas>().sortingOrder = mIconSprite.sortingOrder + 1;
            mProgressBarRenderer.GetComponent<Canvas>().sortingLayerName = mIconSprite.sortingLayerName;
            //������������У���ʼ����Ƿ�ʼ����
            if (!Producing)
            {
                if (Child == null)
                    return;
                if (Child.IsCoffee)
                {
                    productTag = Child.NodeTag;
                    productGrind = !Child.Grind;
                    Producing = true;
                    float power = (float)(1f - ((float)GameEntry.Cat.WisdomLevel - 1f) / 6f);
                    mProducingTime = 10 * power;
                    mTime = 10 * power;
                    mProgressBarRenderer.gameObject.SetActive(true);
                    return;
                }
                //�Ƚ��߼�
            }
            else//�������������
            {
                mProgressBarRenderer.gameObject.SetActive(true);
                mProgressBarRenderer.fillAmount = 1 - (1 - mProducingTime / mTime);
                mProducingTime -= Time.deltaTime;
                if (Child==null ||!Child.IsCoffee)
                {
                    mProducingTime = 0;
                    mTime = 0f;
                    mProgressBarRenderer.gameObject.SetActive(false);
                    mProgressBarRenderer.fillAmount = 1;
                    Producing = false;
                    return;
                }
                if (mProducingTime <= 0)//����������
                {
                    GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, productTag)
                    {
                        Position = this.transform.position + new Vector3(0.5f, 0, 0),
                        RamdonJump = false,
                        Grind = productGrind
                    });
                    RemoveChildren();//ɾ��ȫ�����ӽڵ�
                    mProducingTime = 0;
                    mTime = 0f;
                    mRecipeData = null;
                    mProgressBarRenderer.gameObject.SetActive(false);
                    Producing = false;
                    return;
                }
            }
        }
    }

}