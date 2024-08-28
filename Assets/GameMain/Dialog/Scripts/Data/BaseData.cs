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
                m_After = value;
            }
        }
    }
}
