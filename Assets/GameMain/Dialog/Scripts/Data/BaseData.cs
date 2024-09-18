using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    public class BaseData
    {
        private List<BaseData> m_Fore { get; set; } = new List<BaseData>();
        private List<BaseData> m_After { get; set; } = new List<BaseData>();
        public int Id { get; set; } // �����ı�ʶ������

        /// <summary>
        /// ǰ�ýڵ�
        /// </summary>
        public List<BaseData> Fore
        {
            get
            {
                return m_Fore;
            }
            set
            {
                m_Fore = value;
            }
        }
        /// <summary>
        /// ���ýڵ�
        /// </summary>
        public List<BaseData> After
        {
            get
            {
                return m_After;
            }
            set
            {
                Debug.Log($"Setting After for BaseData ID: {Id}, New After Count: {value.Count}");

                if (value.Count > 0)
                {
                    foreach (BaseData after in value)
                    {
                        Debug.Log($"After BaseData ID: {after.Id}");
                    }
                }

                m_After = value;
            }
        }
    }
}
