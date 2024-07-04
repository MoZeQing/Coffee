﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2024-07-04 17:08:52.528
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    /// <summary>
    /// EventEffect閰嶇疆鏂囦欢。
    /// </summary>
    public class DREventEffect : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取鏁堟灉ID。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取鎻忚堪。
        /// </summary>
        public string Text
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取鏉′欢。
        /// </summary>
        public string Trigger
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取鏁堟灉鏍囩?。
        /// </summary>
        public string EventEffectTag
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取鍙傛暟1。
        /// </summary>
        public string ParamOne
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取鍙傛暟2。
        /// </summary>
        public int ParamTwo
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取鍙傛暟3。
        /// </summary>
        public int ParamThree
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            Text = columnStrings[index++];
            Trigger = columnStrings[index++];
            EventEffectTag = columnStrings[index++];
            ParamOne = columnStrings[index++];
            ParamTwo = int.Parse(columnStrings[index++]);
            ParamThree = int.Parse(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    Text = binaryReader.ReadString();
                    Trigger = binaryReader.ReadString();
                    EventEffectTag = binaryReader.ReadString();
                    ParamOne = binaryReader.ReadString();
                    ParamTwo = binaryReader.Read7BitEncodedInt32();
                    ParamThree = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
