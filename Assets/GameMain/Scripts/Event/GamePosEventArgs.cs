using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class GamePosEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(GamePosEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public GamePos GamePos
        {
            get;
            set;
        }

        public static GamePosEventArgs Create(GamePos gamePos)
        {
            GamePosEventArgs args = ReferencePool.Acquire<GamePosEventArgs>();
            args.GamePos = gamePos;
            return args;
        }

        public override void Clear()
        {

        }
    }
}

public enum GamePos
{ 
    Up,//��Ϣ��
    Down,//������
    Left,//�ֿ���
    Right//�����
}
