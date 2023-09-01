using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class DialogComponent : GameFrameworkComponent
    {      
        private List<StorySO> stories=new List<StorySO>();
        private List<StorySO> loadedStories=new List<StorySO>();

        public List<string> LoadedStories
        { 
            get
            {
                List<string> newStory = new List<string>();
                foreach (StorySO storySO in loadedStories)
                {
                    newStory.Add(storySO.name);
                }
                return newStory;
            }
        }
        public bool InDialog
        {
            get;
            set;
        }

        private void OnEnable()
        {
            stories=new List<StorySO>(Resources.LoadAll<StorySO>("StoryData"));
        }

        public bool StoryUpdate()
        {
            foreach (StorySO story in loadedStories)
            {
                if (GameEntry.Utils.Location != story.outingSceneState)
                    continue;
                if (GameEntry.Utils.Check(story.trigger))
                {
                    GameEntry.UI.OpenUIForm(UIFormId.DialogForm, story.dialogueGraph);
                    InDialog = true;
                    if (story.isRemove)
                        loadedStories.Remove(story);
                    return true;
                }
            }
            return false;
        }

        private void SaveGame(object sender, GameEventArgs e)
        {
            SaveGameEventArgs args = (SaveGameEventArgs)e;
            List<string> newStory = new List<string>();
            foreach (StorySO storySO in loadedStories)
            {
                newStory.Add(storySO.name);
            }
            args.SaveLoadData.storyData= newStory;
        }

        private void LoadGame(object sender, GameEventArgs e)
        {
            LoadGameEventArgs args = (LoadGameEventArgs)e;
            loadedStories.Clear();
            foreach (StorySO storySO in stories)
            {
                if (args.SaveLoadData.storyData.Contains(storySO.name))
                { 
                    loadedStories.Add(storySO);
                }
            }
        }
    }
}
