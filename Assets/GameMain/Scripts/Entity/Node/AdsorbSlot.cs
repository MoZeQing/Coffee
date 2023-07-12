using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsorbSlot : BaseCompenent
{
    public bool HasAdsorb
    {
        get
        {
            return Adsorb != null;
        }
    }
    //ӵ�������
    public BaseCompenent Adsorb
    {
        get;
        private set;
    }
    //�����������
    public BaseCompenent Owner
    {
        get;
        private set;
    }

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        CompenentData data = (CompenentData)userData;
        Owner = GameEntry.Entity.GetEntity(data.OwnerId).GetComponent<BaseCompenent>();
        GameEntry.Entity.AttachEntity(this.Id, data.OwnerId);

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        BaseCompenent compenent = null;
        if (collision.TryGetComponent<BaseCompenent>(out compenent))
        {
            if (compenent.Follow)
                return;
            if (Adsorb != null)
                return;

            Adsorb= compenent;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        BaseCompenent compenent = null;
        if (collision.TryGetComponent<BaseCompenent>(out compenent))
        {
            if (Adsorb != compenent)
                return;

            Adsorb = null;
        }
    }
}
