using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class PlayerComponent : GameFrameworkComponent
    {
        public PlayerData mPlayerData  = new PlayerData();
        public List<RecipeData> recipes = new List<RecipeData>();//�ѽ������䷽

        private void Start()
        {
            recipes = new List<RecipeData>();
            mPlayerData.items = new List<PlayerItemData>();
        }

        public void ClearItem()
        {
            mPlayerData.items.Clear();
        }
        public void AddItem(ItemData itemData, int num)
        {
            if (GetItem(itemData.itemTag) == null)
            {
                mPlayerData.items.Add(new PlayerItemData(itemData, num));
            }
            else
            {
                GetItem(itemData.itemTag).itemNum += num;
            }
        }
        public void AddItem(ItemData itemData, int num, bool equip)
        {
            if (GetItem(itemData.itemTag) == null)
            {
                PlayerItemData playerItem = new PlayerItemData(itemData, num);
                playerItem.equiping = equip;
                mPlayerData.items.Add(playerItem);
            }
            else
            {
                GetItem(itemData.itemTag).itemNum += num;
            }
        }
        public PlayerItemData GetItem(ItemTag itemTag)
        {
            foreach (PlayerItemData itemData in mPlayerData.items)
            {
                if (itemData.itemTag == itemTag)
                    return itemData;
            }
            return null;
        }

        public void ClearRecipe()
        {
            recipes.Clear();
        }
        public void AddRecipes(string[] indexs)
        {
            foreach (string index in indexs)
            {
                AddRecipe(int.Parse(index));
            }
        }
        public void AddRecipes(int[] indexs)
        {
            foreach (int index in indexs)
            { 
                AddRecipe(index);
            }
        }

        public void AddRecipe(RecipeData recipeData)
        {
            if (!HasRecipe(recipeData.Id))
            {
                if (recipeData.IsCoffee)
                    GameEntry.UI.OpenUIForm(UIFormId.UnlockForm, recipeData);
                recipes.Add(recipeData);
            }
        }

        public void AddRecipe(int index)
        {
            if (HasRecipe(index))
                return;
            RecipeData recipeData = new RecipeData(GameEntry.DataTable.GetDataTable<DRRecipe>().GetDataRow(index));
            recipes.Add(recipeData);
        }
        public void RemoveRecipe(int index)
        {
            RecipeData recipeData = null;
            foreach (RecipeData recipe in recipes)
            {
                if (recipe.Id == index)
                    recipeData = recipe;
            }
            recipes.Remove(recipeData);
        }
        public void LoadGame(SaveLoadData saveLoadData)
        {
            recipes.Clear();
            foreach (int index in saveLoadData.recipes)
            {
                GameEntry.Player.recipes.Add(new RecipeData(GameEntry.DataTable.GetDataTable<DRRecipe>().GetDataRow(index)));
            }
        }

        public bool HasRecipe(int id)
        {
            foreach (RecipeData recipe in recipes)
            {
                if (recipe.Id==id)
                    return true;
            }
            return false;
        }
        public bool HasCoffeeRecipe(int node)
        {
            return HasCoffeeRecipe((NodeTag)node);
        }
        public bool HasCoffeeRecipe(NodeTag nodeTag)
        {
            foreach (RecipeData recipe in recipes)
            {
                if (recipe.products.Contains(nodeTag))
                    return true;
            }
            return false;
        }

        public int Money
        {
            get
            {
                return mPlayerData.money;
            }
            set
            {
                mPlayerData.money = value;
                GameEntry.Utils.UpdateData(TriggerTag.Money, mPlayerData.money.ToString());
            }
        }
        public int Energy
        {
            get
            {
                return mPlayerData.energy;
            }
            set
            {
                if (value > MaxEnergy)
                    mPlayerData.energy = MaxEnergy;
                else
                    mPlayerData.energy = value;
                GameEntry.Utils.UpdateData(TriggerTag.Money, mPlayerData.energy.ToString());
            }
        }
        public int MaxEnergy
        {
            get
            {
                return mPlayerData.maxEnergy;
            }
            set
            {
                mPlayerData.maxEnergy = value;
                GameEntry.Utils.UpdateData(TriggerTag.Money, mPlayerData.maxEnergy.ToString());
            }
        }
        public int Day
        {
            get
            {
                return mPlayerData.day;
            }
            set
            {
                mPlayerData.day = value;
                GameEntry.Utils.UpdateData(TriggerTag.Money, mPlayerData.day.ToString());
            }
        }
        public int Rent
        {
            get
            {
                return mPlayerData.rent;
            }
            set
            {
                mPlayerData.rent = value;
                GameEntry.Utils.UpdateData(TriggerTag.Money, mPlayerData.rent.ToString());
            }
        }
    }
}
