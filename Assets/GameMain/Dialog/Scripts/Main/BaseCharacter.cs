using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XNode;
using DG.Tweening;

public class BaseCharacter :MonoBehaviour
{
    [SerializeField] private Image mImage = null;
    public CharacterTag CharacterTag { get; set; }
    public CharSO CharSO { get; set; } = null;

    public void SetAction(ActionTag actionTag)
    {
        switch (actionTag)
        {
            case ActionTag.Jump:
                JumpAction();
                break;
            case ActionTag.Shake:
                ShakeAction();
                break;
        }
    }
    //���Ҷ�������
    protected virtual void ShakeAction()
    {
        mImage.gameObject.transform.DOPause();
        mImage.gameObject.transform.localPosition = Vector3.zero;
        mImage.gameObject.transform.DOPunchPosition(Vector3.right * 100, 0.4f, 10);
    }
    //������������
    protected virtual void JumpAction()
    {
        mImage.gameObject.transform.DOPause();
        mImage.gameObject.transform.localPosition = Vector3.zero;
        mImage.gameObject.transform.DOLocalJump(Vector3.zero, 200, 1, 0.3f, false);
    }
    public void SetDiff(DiffTag diffTag)
    {
        if (CharSO == null)
            return;
        mImage.sprite = CharSO.diffs[(int)diffTag];
    }
    public void SetData(CharSO charSO)
    {
        CharSO = charSO;
    }
    //�볡����
    public void Show()
    {
        CharacterTag = CharacterTag.Show;
        mImage.color = Color.gray;
        mImage.DOKill();
        mImage.DOColor(Color.white, 0.3f);
        mImage.gameObject.transform.localPosition += Vector3.right * 100f;
        mImage.gameObject.transform.DOLocalMoveX(0f, 0.3f);
    }

    public void Hide()
    {
        CharacterTag = CharacterTag.Hide;
        mImage.DOKill();
        //����
        mImage.DOFade(0, 0.3f);
        mImage.gameObject.transform.DOLocalMoveX(0, 0.3f);
    }

    public void MainShow()
    {
        mImage.DOKill();
        //��Ҫ��ɫ���������Ա��Ϊʲô����û��DOScaleֱ�ӵ�����������
        mImage.DOColor(Color.white, 0.3f);
        mImage.gameObject.transform.DOScale(1.05f, 0.3f);
        mImage.canvas.sortingOrder = 2;
    }

    public void SubShow()
    {
        mImage.DOKill();
        //����Ҫ��ɫ���
        mImage.DOColor(Color.grey, 0.3f);
        mImage.gameObject.transform.DOScale(1f, 0.3f);
        mImage.canvas.sortingOrder = 1;
    }
}
public enum CharacterTag
{ 
    Show,
    Hide
}
[System.Serializable]
public class ActionData
{
    public CharSO charSO;
    public DiffTag diffTag;
    public ActionTag actionTag;

    public ActionData() { }
    public ActionData(CharSO charSO)
    { 
        this.charSO = charSO;
        diffTag = DiffTag.Idle;
        actionTag = ActionTag.None;
    }
}
//���Tag
public enum DiffTag
{
    Idle,
    Smile
}
//����Tag
public enum ActionTag
{
    None,//��
    Jump,//��������
    Shake//���Ҷ���
}

