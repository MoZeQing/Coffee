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
            if (GameEntry.Utils.Day > 14)
            {
                missionText.text = string.Format("��Ҫ�ٰ�ɳ��������Ҫ�����㹻��ĺ��ѣ���������ɳ��ǰ�������ǵĺøж�������100��\r\n<size=24><color=red>���ѵĺøжȻ�������Ĺ�������</color></size>\r\n", 35 - GameEntry.Utils.Day);
            }
            else if (GameEntry.Utils.Day < 7)
            {
                missionText.text = string.Format("��취ȡ�ð���˿�����Σ�\n<size=28><color=red>��������ǰ������˿�ĺøж�������10���ϰ�</color></size>");
            }
            else
            { 
                missionText.text= string.Format("<size=28><color=red>�����ܵĺͿ��ȵ���Ϲ˿����������ɣ�</color></size>\n����Ե�����Ѱ�ť���˽����ǵĺøж�");
            }
        }
    }
}

