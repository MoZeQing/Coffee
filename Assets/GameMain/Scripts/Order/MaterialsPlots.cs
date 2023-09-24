using GameMain;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;

public class MaterialsPlots : MonoBehaviour,IPointerDownHandler
{
    public NodeTag nodeTag;
    public bool IsGuide;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(nodeTag==NodeTag.Cup)
        {
            GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, nodeTag)
            {
                Position = this.transform.position,
                Jump = true
            });
            return;
        }
        else if (!IsGuide)
        {
            if (GameEntry.Utils.GetPlayerItem((ItemTag)(int)nodeTag).itemNum <= 0)
            {
                //����ԭ���ϲ������Ϣ������ʾ
                return;
            }
        }
        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10000, nodeTag)
        {
            Position = this.transform.position,
            Jump = true
        }); 
    }
}
