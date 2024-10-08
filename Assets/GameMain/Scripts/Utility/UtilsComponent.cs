using GameFramework.Event;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class UtilsComponent : GameFrameworkComponent
    {
        public bool PickUp { get; set; } = false;
        private int mCarfSort = 99;
        public int CartSort
        {
            get
            {
                return mCarfSort++;
            }
        }
        private int mSoundSort = 10;
        public int SoundSort
        {
            get
            {
                return mSoundSort++;
            }
        }
        //接下来尽可能使得utils只用于存储资源信息，而数据信息都给其它的类
        public Dictionary<string, RecipeData> recipes = new Dictionary<string, RecipeData>();
        public Dictionary<string, CharSO> chars = new Dictionary<string, CharSO>();

        private UtilsData mUtilsData=new UtilsData();
        public UtilsData GetSaveData()
        {
            return mUtilsData;
        }
        public void LoadData(UtilsData utilsData)
        { 
            mUtilsData= utilsData;
        }
        public void AddFriendFavor(string name, int favor)
        {
            if (!mUtilsData.friends.ContainsKey(name))
                mUtilsData.friends.Add(name, favor);
            else
                mUtilsData.friends[name] += favor;
            //硬编码转换
            if (name == "Money") mUtilsData.values[TriggerTag.FMoney] = mUtilsData.friends[name].ToString();
            if (name == "Regular") mUtilsData.values[TriggerTag.FRegular] = mUtilsData.friends[name].ToString();
            if (name == "Dog") mUtilsData.values[TriggerTag.FDog] = mUtilsData.friends[name].ToString();
            if (name == "Fiction") mUtilsData.values[TriggerTag.FFiction] = mUtilsData.friends[name].ToString();
            if (name == "Courier") mUtilsData.values[TriggerTag.FCourier] = mUtilsData.friends[name].ToString();
            if (name == "Doc") mUtilsData.values[TriggerTag.FWitch] = mUtilsData.friends[name].ToString();
        }

        public Dictionary<string, int> GetFriends()
        {
            return mUtilsData.friends;
        }

        public void ClearFriendFavor()
        {
            mUtilsData.friends.Clear();
        }
        public WeatherTag WeatherTag
        {
            get
            {
                return mUtilsData.weatherTag;
            }
            set
            { 
                mUtilsData.weatherTag= value;
            }
        }
        public GameState GameState
        {
            get
            {
                return mUtilsData.gameState;
            }
            set
            {
                mUtilsData.gameState = value;
                switch (mUtilsData.gameState)
                {
                    case GameState.Midnight:
                        WeatherTag = WeatherTag.Night;
                        break;
                    case GameState.Night:
                        if (Mathf.Abs(GameEntry.Player.MaxAp - GameEntry.Player.Ap) >= 2)
                            WeatherTag = WeatherTag.Night;
                        else
                            WeatherTag = WeatherTag.Afternoon;
                        break;
                    case GameState.Morning:
                        WeatherTag = WeatherTag.Morning;
                        break;
                }
                GameEntry.Event.FireNow(this, GameStateEventArgs.Create(mUtilsData.gameState));
                GameEntry.Utils.AddValue(TriggerTag.TimeTag, mUtilsData.gameState.ToString());
            }
        }
        public OutingSceneState Location
        {
            get
            {
                return mUtilsData.location;
            }
            set
            {
                mUtilsData.location = value;
                GameEntry.Utils.AddValue(TriggerTag.Location, mUtilsData.location.ToString());
            }
        }
        public bool CheckDayPassFlag(string key)
        {
            return mUtilsData.dayPassFlags.ContainsKey(key) && mUtilsData.dayPassFlags[key] > 0;
        }
        public void AddDayPassFlag(string flag)
        {
            if (mUtilsData.dayPassFlags.ContainsKey(flag))
            {
                mUtilsData.dayPassFlags[flag]++;
                mUtilsData.flags[flag] = mUtilsData.dayPassFlags[flag];
            }
            else
            {
                mUtilsData.dayPassFlags.Add(flag, 1);
                mUtilsData.flags.Add(flag, 1);
            }
        }
        public void DayPass(int day)
        {
            foreach (KeyValuePair<string, int> pair in mUtilsData.dayPassFlags)
            {
                mUtilsData.dayPassFlags[pair.Key]-=day;
                mUtilsData.flags[pair.Key] = mUtilsData.dayPassFlags[pair.Key];
            }
        }
        public bool CheckFlag(string key)
        {
            return mUtilsData.flags.ContainsKey(key);
        }
        public bool CheckFlag(string key, int value)
        {
            if (mUtilsData.flags.ContainsKey(key))
                return mUtilsData.flags[key] == value;
            return false;
        }
        public void AddFlag(string flag)
        {
            if (mUtilsData.flags.ContainsKey(flag))
            {
                mUtilsData.flags[flag]++;
            }
            else
            {
                mUtilsData.flags.Add(flag, 1);
            }
        }
        public void RemoveFlag(string flag)
        {
            if (mUtilsData.flags.ContainsKey(flag))
                mUtilsData.flags.Remove(flag);
        }

        public void ClearFlag()
        {
            mUtilsData.flags.Clear();
        }
        public bool Check(Trigger trigger)
        {
            if (trigger == null)
                return true;
            if (trigger.key == TriggerTag.Or)
            {
                if (trigger.GetTriggers().Count != 0)
                {
                    foreach (Trigger tr in trigger.GetTriggers())
                    {
                        if (Check(tr))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                return true;
            }
            else
            {
                if (trigger.GetTriggers().Count != 0)
                {
                    foreach (Trigger tr in trigger.GetTriggers())
                    {
                        if (!Check(tr))
                        {
                            return false;
                        }
                    }
                }
                if (trigger.key == TriggerTag.None)
                    return true;
                if (trigger.key == TriggerTag.Flag)
                {
                    if (!trigger.not)
                        return CheckFlag(trigger.value);
                    else
                        return !CheckFlag(trigger.value);
                }
                if (!mUtilsData.values.ContainsKey(trigger.key))
                    return false;
                if (trigger.equals)
                {
                    if (!trigger.not)
                    {
                        if (mUtilsData.values[trigger.key] == trigger.value)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        if (mUtilsData.values[trigger.key] == trigger.value)
                            return false;
                        else
                            return true;
                    }
                }
                else
                {
                    if (trigger.not)//�ж�����
                    {
                        if (int.Parse(mUtilsData.values[trigger.key]) > int.Parse(trigger.value))
                            return false;
                        else
                            return true;
                    }
                    else
                    {
                        if (int.Parse(mUtilsData.values[trigger.key]) < int.Parse(trigger.value))
                            return false;
                        else
                            return true;
                    }
                }
            }            
        }
        public void AddValue(TriggerTag valueTag, string value)
        {
            if (!mUtilsData.values.ContainsKey(valueTag))
            {
                mUtilsData.values.Add(valueTag, value);
            }
            else
            {
                mUtilsData.values[valueTag] = value;
            }
        }
        public void RunEvent(string[] consoles)
        {
            if (consoles == null) return;
            for (int i=0; i<consoles.Length;i++)
            {
                RunEvent(consoles[i]);
            }
        }
        public bool RunEvent(string consoleStr)
        {
            if (String.IsNullOrEmpty(consoleStr))
                return false;
            string[] consoleStrs = consoleStr.Split(' ');
            EventTag eventTag;
            Enum.TryParse<EventTag>(consoleStrs[0],out eventTag);
            EventData eventData=new EventData(eventTag);
            {
                eventData.values = consoleStrs;
            }
            return RunEvent(eventData);
        }
        public void RunEvent(List<EventData> eventDatas)
        {
            foreach (EventData eventData in eventDatas)
            {
                RunEvent(eventData);
            }
        }
        public bool RunEvent(EventData eventData)
        {
            switch (eventData.eventTag)
            {
                case EventTag.Play:
                    GameEntry.Dialog.PlayStory(eventData.values[1]);
                    return true;
                case EventTag.AddMoney:
                    GameEntry.Player.Money += int.Parse(eventData.values[1]);
                    return true;
                case EventTag.AddFavor:
                    GameEntry.Cat.Favor += int.Parse(eventData.values[1]);
                    return true;
                case EventTag.AddEnergy:
                    GameEntry.Player.Energy += int.Parse(eventData.values[1]);
                    return true;
                case EventTag.AddAp:
                    GameEntry.Player.Ap+= int.Parse(eventData.values[1]);
                    return true;
                case EventTag.AddMaxAp:
                    GameEntry.Player.MaxAp += int.Parse(eventData.values[1]);
                    return true;
                case EventTag.AddItem:
                    GameEntry.Player.AddPlayerItem(new ItemData((ItemTag)Enum.Parse(typeof(ItemTag), eventData.values[1])), int.Parse(eventData.values[2]));
                    return true;
                case EventTag.AddFlag:
                    GameEntry.Utils.AddFlag(eventData.values[1]);
                    return true;
                case EventTag.RemoveFlag:
                    GameEntry.Utils.RemoveFlag(eventData.values[1]);
                    return true;
                case EventTag.NextDay://重写逻辑
                    GameEntry.Player.Day++;
                    GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Night));
                    return true;
                case EventTag.PlayBgm:
                    return true;
                case EventTag.EndGame:
                    GameEntry.Dialog.SetComplete(null);
                    GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Menu));
                    GameEntry.UI.OpenUIForm(UIFormId.EndForm);
                    return true;
                case EventTag.AddDay:
                    GameEntry.Player.Day += int.Parse(eventData.values[1]);
                    return true;
                case EventTag.Rent:
                    return true; 
                case EventTag.AddFriend:
                    GameEntry.Utils.AddFriendFavor(eventData.values[1], int.Parse(eventData.values[2]));
                    return true; 
                case EventTag.AddRecipe:
                    GameEntry.Player.AddRecipe(int.Parse(eventData.values[1]));
                    return true; 
                case EventTag.Test:
                    GameEntry.Event.FireNow(this, ValueEventArgs.Create(TriggerTag.Energy,"成功"));
                    return true;
                case EventTag.AddBuff:
                    GameEntry.Buff.AddBuff(int.Parse(eventData.values[1]));
                    return true;
                case EventTag.RemoveBuff:
                    GameEntry.Buff.RemoveBuff(int.Parse(eventData.values[1]));
                    return true;
                case EventTag.Weather:
                    GameEntry.Utils.WeatherTag = (WeatherTag)int.Parse(eventData.values[1]);
                    return true;
                case EventTag.SetClothing:
                    GameEntry.Player.AddPlayerItem(new ItemData((ItemTag)int.Parse(eventData.values[1])), 1);
                    GameEntry.Utils.AddFlag(((ItemTag)int.Parse(eventData.values[1])).ToString());
                    GameEntry.Cat.Closet = int.Parse(eventData.values[1]);
                    break;
                case EventTag.ShowForm:
                    GameEntry.UI.OpenUIForm((UIFormId)Enum.Parse(typeof(UIFormId), eventData.values[1]));
                    break;
                case EventTag.AddCharm:
                    GameEntry.Cat.Charm+= (int.Parse(eventData.values[1]));
                    break;
                case EventTag.AddStamina:
                    GameEntry.Cat.Stamina += (int.Parse(eventData.values[1]));
                    break;
                case EventTag.WorkTest:
                    if (GameEntry.Procedure.CurrentProcedure is ProcedureMenu)
                    {
                        GameEntry.Event.FireNow(this, GameStateEventArgs.Create(GameState.Test));
                    }
                    break;
                case EventTag.OrderTest:
                    GameEntry.Event.FireNow(eventData.values, EventEventArgs.Create());
                    break;
                case EventTag.AddCGFlag:
                    GameEntry.SaveLoad.AddCGFlag(eventData.values[1]);
                    break;
                case EventTag.AddWisdom:
                    GameEntry.Cat.Wisdom += (int.Parse(eventData.values[1]));
                    break;
            }
            return false;
        }
    }
    [System.Serializable]
    public class UtilsData
    {
        public float orderPower;
        public float pricePower;
        public Dictionary<TriggerTag, string> values = new Dictionary<TriggerTag, string>();
        public Dictionary<string, int> flags = new Dictionary<string, int>();
        public Dictionary<string,int> dayPassFlags=new Dictionary<string, int>();
        public Dictionary<string, int> friends = new Dictionary<string, int>();//好友字典
        public OutingSceneState location;
        public GameState gameState=GameState.None;
        public WeatherTag weatherTag;
    }
}