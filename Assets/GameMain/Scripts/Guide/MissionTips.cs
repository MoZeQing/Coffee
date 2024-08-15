using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class MissionTips : MonoBehaviour
    {
        [SerializeField] private Text missionText;

        private void Update()
        {
            switch (GameEntry.Player.Day)
            {
                case 2:
                case 3:
                    this.gameObject.SetActive(true);
                    missionText.text = "��������������˿�ɣ�";
                    break;
                case 4:
                    this.gameObject.SetActive(true);
                    missionText.text = "<color=red>���</color>ȥ��װ��ݷ�¶ϣ����";
                    break;
                case 5:
                case 6:
                case 7:
                    this.gameObject.SetActive(true);
                    if (GameEntry.Player.Money<500)
                        missionText.text = "׼��<color=red>500</color>Ԫ������ɣ�";
                    else
                        missionText.text = "׼��500Ԫ������ɣ�";
                    break;
                case 8:
                case 9:
                case 10:
                case 11:
                    this.gameObject.SetActive(true);
                    if (GameEntry.Player.Money < 500)
                        missionText.text = "׼��<color=red>800</color>Ԫ������ɣ�";
                    else
                        missionText.text = "׼��800Ԫ������ɣ�";
                    break;
                case 12:
                case 13:
                case 14:
                case 15:
                    this.gameObject.SetActive(true);
                    if (GameEntry.Player.Money < 500)
                        missionText.text = "׼��<color=red>1100</color>Ԫ������ɣ�";
                    else
                        missionText.text = "׼��1100Ԫ������ɣ�";
                    break;
                case 16:
                case 17:
                case 18:
                case 19:
                    this.gameObject.SetActive(true);
                    if (GameEntry.Player.Money < 500)
                        missionText.text = "׼��<color=red>1500</color>Ԫ������ɣ�";
                    else
                        missionText.text = "׼��1500Ԫ������ɣ�";
                    break;
                default:
                    this.gameObject.SetActive(false);
                    break;
            }
        }
    }
}

