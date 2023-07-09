using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ֻҪ�ܺ�Ū��Ҿ����ˣ��������Ĺ�����ˮ���죡
public class Fog : MonoBehaviour
{
    private Transform mMask;
    private Transform mBG;

    private void Start()
    {
        mMask = this.transform;
        mBG = this.transform.Find("BG").transform;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = MouseToWorld(Input.mousePosition);
        //if (Mathf.Abs(mousePos.x) < 8.64f &&
        //    mousePos.y < -0.91f && mousePos.y > -8.69f)
        //{
        //    mMask.transform.position = MouseToWorld(Input.mousePosition);
        //}
        mMask.transform.position = MouseToWorld(Input.mousePosition);
        mMask.transform.position = new Vector3(Mathf.Clamp(mMask.transform.position.x, -8.8f, 8.8f), Mathf.Clamp(mMask.transform.position.y, -8f, -1.6f), 0);
        mBG.transform.position = new Vector3(0, -4.8f, 0);
    }

    private Vector3 MouseToWorld(Vector3 mousePos)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.z = screenPosition.z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
