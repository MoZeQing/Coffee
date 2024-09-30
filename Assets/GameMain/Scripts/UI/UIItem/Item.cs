using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameFramework.DataTable;
using GameMain;

[System.Serializable]
public class Item : ShopItem
{
    public override void SetData(DRItem itemData)
    {
        //mShopItemData = itemData;
        //this.gameObject.SetActive(true);
        //itemImage.sprite = Resources.Load<Sprite>(itemData.ImagePath);
        //if (GameEntry.Player.GetPlayerItem((ItemTag)itemData.Id) != null)
        //    inventoryText.text = string.Format("{0}", GameEntry.Player.GetPlayerItem((ItemTag)itemData.Id).itemNum.ToString());
        //else
        //    inventoryText.text = string.Format("{0}", 0);
    }
    public override void SetData(DRItem itemData, Action<DRItem> onClick, Action<bool, DRItem> onTouch)
    {
        //mShopItemData = itemData;
        //this.gameObject.SetActive(true);
        //itemImage.sprite = Resources.Load<Sprite>(itemData.ImagePath);
        //if (GameEntry.Player.GetPlayerItem((ItemTag)itemData.Id) != null)
        //    inventoryText.text = string.Format("���:{0}", GameEntry.Player.GetPlayerItem((ItemTag)itemData.Id).itemNum.ToString());
        //else
        //    inventoryText.text = string.Format("���:{0}", 0);
        //SetClick(onClick);
        //SetTouch(onTouch);
    }
}
[System.Serializable]
public class ItemData
{
    public ItemTag itemTag;
    public ItemKind itemKind;
    public string itemName;
    public string itemImgPath;
    public int price;
    public bool equipable;
    public int family;
    public int hope;
    public int mood;
    public int love;
    public int favor;
    public int ability;
    public int maxNum;
    [TextArea]
    public string itemInfo;

    public ItemData() { }

    public ItemData(DRItem item)
    {
        itemName = item.Name;
        itemTag = (ItemTag)item.Id;
        itemKind = (ItemKind)item.Kind;
        itemInfo = item.Info;
        price = item.Price;
        maxNum = item.MaxNum;
        equipable = item.Equipable;
        itemImgPath = item.ImagePath;
    }
    public ItemData(ItemTag itemTag)
    {
        this.itemTag = itemTag;
        IDataTable<DRItem> items = GameEntry.DataTable.GetDataTable<DRItem>();
        DRItem item = items.GetDataRow((int)itemTag);
        itemName = item.Name;
        itemTag = (ItemTag)item.Id;
        itemKind = (ItemKind)item.Kind;
        itemInfo = item.Info;
        price = item.Price;
        maxNum = item.MaxNum;
        equipable = item.Equipable;
    }
}
[System.Serializable]
public class PlayerItemData : ItemData
{
    public int itemNum;
    public bool equiping;

    public PlayerItemData() { }
    public PlayerItemData(ItemTag itemTag, int num)
        :base(itemTag) 
    {
        this.itemNum = num;
    }

    public PlayerItemData(ItemData itemData, int num)
    {
        itemTag = itemData.itemTag;
        itemKind = itemData.itemKind;
        itemName = itemData.itemName;
        price = itemData.price;
        equipable = itemData.equipable;
        itemInfo = itemData.itemInfo;     
        itemNum = num;
        itemImgPath = itemData.itemImgPath;
    }
}

public enum ItemKind
{ 
    None=-1,//ȫ��
    Materials=0,//ԭ����
    Item=1,//����
    Instrument=2,//��е
    Book=3,//��
    Cake=4,//����
    Music = 5,//����
    Dishes = 6,//����
    Food = 7,//ʳ��
    Clothes=8,//�·�
}
public enum ItemTag
{
    CoffeeBean=0,//���ȶ�
    Water=4,//ˮ
    Milk=6,//ţ��
    Cream=8,//����
    ChocolateSyrup=9,//�ɿ�����
    Sugar=11,//��
    Ice=10,//��
    Salt=12,//��
    CondensedMilk=13,
    ManualGrinder=101,//�ֶ���ĥ��
    Extractor=102,//�綯��ȡ
    ElectricGrinder=103,//�綯��ĥ��
    Heater=104,//������
    Syphon=105,//������
    FrenchPress=106,//��ѹ��
    Kettle=107,//���ݺ�
    FilterBowl=108,//��ֽʽ�˱�
    Cup=109,//���ȱ�
    Stirrer=110,//������
    Food1 = 200,
    Food2 = 201,
    Food3 = 202,
    Food4 = 203,
    Food5 = 204,
    Food6 = 205,
    Food7 = 206,
    Item1 = 500,
    Item2 = 501,
    Item3 = 502,
    Item4 = 503,
    Item5 = 504,
    Item6 = 505,
    Item7 = 506,
    Item8 = 507,
    Item9 = 508,
    Item10 = 509,
    Item11 = 510,
    Item12 = 511,
    Closet1 = 1001,//�ƾ��·�
    Closet2 = 1002,//����
    Closet3 = 1003,//Ů��װ
    Closet4 = 1004,//Ӿװ
    Closet5 = 1005,//�˶���
    Closet6 = 1006//�����
}
