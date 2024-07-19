using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class GuideComponent : GameFrameworkComponent
    {

        private void Update()
        {
            if (GameEntry.Utils.PlayerData.guideID==3)
            {
                if (GameEntry.Utils.GameState == GameState.Afternoon&&
                    GameEntry.Utils.Day==3)
                { 
                    GameEntry.UI.OpenUIForm(UIFormId.GuideForm, 4);
                    GameEntry.Utils.PlayerData.guideID = 4;
                }
            }
            if (GameEntry.Utils.PlayerData.guideID == 4)
            {
                if (GameEntry.Utils.GameState == GameState.Afternoon &&
                    GameEntry.Utils.Day == 4)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.GuideForm, 5);
                    GameEntry.Utils.PlayerData.guideID = 5;
                }
            }
            if (GameEntry.Utils.PlayerData.guideID == 5)
            {
                if (GameEntry.Utils.GameState == GameState.Afternoon)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.GuideForm, 4);
                    GameEntry.Utils.PlayerData.guideID = 4;
                }
            }
            if (GameEntry.Utils.PlayerData.guideID == 3)
            {
                if (GameEntry.Utils.GameState == GameState.Afternoon)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.GuideForm, 4);
                    GameEntry.Utils.PlayerData.guideID = 4;
                }
            }
        }
        public void OnGuideEvent(object sender, GameEventArgs e)
        { 
            GuideEventArgs guide = (GuideEventArgs)e;
            int guideId = guide.Id;
        }
    }
}

