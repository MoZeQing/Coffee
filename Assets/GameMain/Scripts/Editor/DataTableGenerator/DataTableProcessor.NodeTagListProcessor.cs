using System;
using System.Collections.Generic;
using System.IO;
using static GameMain.Editor.DataTableTools.DataTableProcessor;

namespace GameMain.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class NodeTagListProcessor : GenericDataProcessor<List<NodeTag>>
        {
            //�Ƿ���ϵͳ�Դ�������
            public override bool IsSystem
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// �������������
            /// </summary>
            public override string LanguageKeyword
            {
                get
                {
                    return "List<NodeTag>";
                }
            }

            /// <summary>
            /// �������������
            /// </summary>
            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "List<NodeTag>",
                };
            }

            /// <summary>
            /// ���������ֵ�����ض��������ֵ
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public override List<NodeTag> Parse(string value)
            {
                List<NodeTag> temp = new List<NodeTag>();
                if (value == "" || value == "empty")
                {
                    return temp;
                }
                string[] values = value.Split(',');
                foreach (var VarIAble in values)
                {
                    temp.Add((NodeTag)System.Enum.Parse(typeof(NodeTag), VarIAble));
                }
                return temp;
            }

            /// <summary>
            /// д���������
            /// </summary>
            /// <param name="dataTableProcessor"></param>
            /// <param name="binaryWriter"></param>
            /// <param name="value"></param>
            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                List<NodeTag> NodeTagList = Parse(value);
                binaryWriter.Write(NodeTagList.Count);
                foreach (var NodeTagItem in NodeTagList)
                {
                    binaryWriter.Write((byte)NodeTagItem);
                }


            }
        }
    }
}
