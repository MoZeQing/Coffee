using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;
using GameFramework.DataTable;
using GameMain;

[System.Serializable]
public class Item : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private Image itemImg;
    [SerializeField] private Image usingImg;
    [SerializeField] private Text itemText;
    [SerializeField] private Text priceText;
    [SerializeField] private Text amountText;
    [SerializeField] private Text itemInfoText;

    private PlayerItemData mItemData;
    private Action<PlayerItemData> mAction;

    public void SetData(PlayerItemData itemData)
    {
        mItemData = itemData;
        itemText.text = itemData.itemName.ToString();
        priceText.text = string.Format("�۸�:{0}", itemData.price.ToString());
        amountText.text = string.Format("���:{0}", itemData.itemNum.ToString());
        if (mItemData.equipable)
            //usingImg.gameObject.SetActive(mItemData.equiping);
            this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void SetClick(Action<ItemData> action)
    {
        mAction = action;
    }

    private void OnClick()
    {
        mAction(mItemData);
        if (mItemData.equipable)
        {
            usingImg.gameObject.SetActive(!usingImg.gameObject.activeSelf);
            //mItemData.equiping = usingImg.gameObject.activeSelf;
        }
        //������Ϣ
    }
}
[System.Serializable]
public class ItemData
{
    public ItemTag itemTag;
    public ItemKind itemKind;
    public string itemName;
    public int price;
    public GameMain.FilterMode filterMode;
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
    public ItemData(ItemTag itemTag)
    {
        this.itemTag = itemTag;
        IDataTable<DRItem> items = GameEntry.DataTable.GetDataTable<DRItem>();
        DRItem item = items.GetDataRow((int)itemTag);
        itemName = item.Name;
        itemTag = (ItemTag)item.Id;
        itemInfo = item.Info;
        price = item.Price;
        family = item.Family;
        hope = item.Hope;
        mood = item.Mood;
        love = item.Love;
        favor = item.Favor;
        ability = item.Ap;
        maxNum = item.MaxNum;
        filterMode = (GameMain.FilterMode)item.FilterMode;
        equipable = item.Equipable;
    }
}

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
        itemName = itemData.itemName;
        price = itemData.price;
        filterMode = itemData.filterMode;
        equipable = itemData.equipable;
        itemInfo = itemData.itemInfo;     
        itemNum = num;
    }
}

public enum ItemKind
{ 
    Materials=0,
    Item=1,
    Instrument=2,
    Book=3,
    Cake=4,
    Music = 5,
    Glass = 6,
    Dishes = 7,
    Food = 8,
    Clothes=9,
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
    ManualGrinder=16,//�ֶ���ĥ��
    Extractor=17,//�綯��ȡ
    ElectricGrinder=18,//�綯��ĥ��
    Heater=19,//������
    Syphon=20,//������
    FrenchPress=21,//��ѹ��
    Kettle=22,//���ݺ�
    FilterBowl=23,//��ֽʽ�˱�
    Cup=24,//���ȱ�
    Stirrer=25,//������
    Espresso,//Ũ������
    HotCafeAmericano,//����ʽ
    IceCafeAmericano,//����ʽ
    HotLatte,//������
    IceLatte,//������
    HotMocha,//��Ħ��
    IceMocha,//��Ħ��
    Kapuziner,//������ŵ
    FlatWhite,//�İ�
    Book1 =30,
    Book2=31,
    Book3=32,
    Book4=33,
    Book5=34,
    Music1 = 40,
    Music2 = 41,
    Music3 = 42,
    Dishes1 = 50,
    Dishes2 = 51,
    Dishes3 = 52,
    Dishes4 = 53,
    Dishes5 = 54,
    Food1 = 60,
    Food2 = 61,
    Food3 = 62,
    Food4 = 63,
    Food5 = 64,
    Closet1 = 101,
    Closet2 = 102,
    Closet3 = 103,
    Closet4 = 104,
}
