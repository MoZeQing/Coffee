using System;
using System.Collections.Generic;
using System.IO;
using static GameMain.Editor.DataTableTools.DataTableProcessor;

namespace GameMain.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class NodeTagProcessor : GenericDataProcessor<NodeTag>
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
                    return "NodeTag";
                }
            }

            /// <summary>
            /// �������������
            /// </summary>
            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "NodeTag",
                };
            }

            /// <summary>
            /// ���������ֵ�����ض��������ֵ
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public override NodeTag Parse(string value)
            {
                return (NodeTag)System.Enum.Parse(typeof(NodeTag), value);
            }

            /// <summary>
            /// д���������
            /// </summary>
            /// <param name="dataTableProcessor"></param>
            /// <param name="binaryWriter"></param>
            /// <param name="value"></param>
            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                    binaryWriter.Write((float)Parse(value));
            }
        }
    }
}
