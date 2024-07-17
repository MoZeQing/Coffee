using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class GameStateEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(GameStateEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public GameState GameState
        {
            get;
            set;
        }

        public static GameStateEventArgs Create(GameState gameState)
        {
            GameStateEventArgs args = ReferencePool.Acquire<GameStateEventArgs>();
            args.GameState = gameState;
            return args;
        }

        public override void Clear()
        {

        }
    }
}

public enum GameState
{
    None,
    Morning,//�����ִ�
    Work,//�����ִ�
    ForeSpecial,//�������ǰ�ִ�
    Special,//��������ִ�
    AfterSpecial,//������˺��ִ�
    Afternoon,//�����ִ�
    Night,//�����ִ�
    Midnight,//�����ִ�
    Sleep,
    Guide,
    Menu,
    Weekend
}
