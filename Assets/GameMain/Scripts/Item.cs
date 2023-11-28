using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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
        {
            usingImg.gameObject.SetActive(mItemData.equiping);
            this.GetComponent<Button>().onClick.AddListener(OnClick);
        }
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
            mItemData.equiping = usingImg.gameObject.activeSelf;
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
        itemKind = (ItemKind)item.Kind;
        itemInfo = item.Info;
        price = item.Price;
        family = item.Family;
        hope = item.Hope;
        mood = item.Mood;
        love = item.Love;
        favor = item.Favor;
        ability = item.Ap;
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
    Closet1 = 1001,
    Closet2 = 1002,
    Closet3 = 1003,
    Closet4 = 1004,
}
