using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Consumable, Equipment, Material }
[System.Serializable]
public class Item
{
    public string itemName;
    public string instanceID;
    public Sprite sprite;
    [TextArea(1,3)]
    public string description;
    public ItemType itemType;

    public int purchasePrice;
    public int sellPrice;

    [System.NonSerialized]
    public int Count = 1;
    public bool isStackable;

    public virtual void Use()
    {
        Debug.Log(itemName + " is used.");
    }
}

[System.Serializable]
public class Potion : Item
{
    public int hp = 0;
    public int mp = 0;

    public override void Use()
    {
        //find player stat and recover
        GameObject.FindObjectOfType<PlayerStat>().currentHealth += hp;
        GameObject.FindObjectOfType<PlayerStat>().currentMana += mp;
        Debug.Log(string.Format("{0} is used. {1} | {2}", itemName, hp, mp));
    }

    public Potion()
    {
        isStackable = true;
        itemType = ItemType.Consumable;
    }

}

[System.Serializable]
public class Equipment : Item
{
    public int attack = 0;
    public int defense = 0;
    public int spAttack = 0;
    public int spDefense = 0;
    public int speed = 0;
    
    public EquipmentSlot equipmentSlot;

    public override void Use()
    {
        Debug.Log(itemName + " is euiped." + string.Format("{0} | {1} | {2} | {3}",attack,defense,spAttack,spDefense));
        GameplayManager.instance.equipmentController.Equip(this);
    }

    public Equipment()
    {
        isStackable = false;
        ItemData.UNSTACKABLE_ID++;
        itemType = ItemType.Equipment;
    }
}

public class ItemData : MonoBehaviour {

    [SerializeField]
    List<Potion> potions = new List<Potion>();
    [SerializeField]
    List<Equipment> equipments = new List<Equipment>();

    public static Dictionary<string, Potion> potionDictionary = new Dictionary<string, Potion>();
    public static Dictionary<string, Equipment> equipmentDictionary = new Dictionary<string, Equipment>();
    public static Dictionary<string, Item> allItemDictionary = new Dictionary<string, Item>();
    public static int UNSTACKABLE_ID = 1;

    private void Awake()
    {
        foreach(var po in potions)
        {
            potionDictionary.Add(po.itemName, po);
            allItemDictionary.Add(po.itemName, po);
        }

        foreach(var eq in equipments)
        {
            equipmentDictionary.Add(eq.itemName, eq);
            allItemDictionary.Add(eq.itemName, eq);
        }
    }

    public static Item CreateItem(string itemName)
    {
        Item tempItem = null;

        if (potionDictionary.ContainsKey(itemName))
        {
            Potion tempPotion = new Potion();
            tempPotion.itemName = potionDictionary[itemName].itemName;
            tempPotion.instanceID = itemName;
            tempPotion.hp = potionDictionary[itemName].hp ;
            tempPotion.mp = potionDictionary[itemName].mp;
            tempPotion.description = potionDictionary[itemName].description;
            tempPotion.sprite = potionDictionary[itemName].sprite;
            tempItem = tempPotion;
        }
        else if (equipmentDictionary.ContainsKey(itemName))
        {
            Equipment equipment = new Equipment();
            equipment.itemName = equipmentDictionary[itemName].itemName;
            equipment.instanceID = "equipment"+ ItemData.UNSTACKABLE_ID;
            //Debug.Log("item name :" + equipment.itemName + ", id :" + ItemData.UNSTACKABLE_ID);
            equipment.attack = equipmentDictionary[itemName].attack;
            equipment.spAttack = equipmentDictionary[itemName].spAttack;
            equipment.defense = equipmentDictionary[itemName].defense;
            equipment.spDefense = equipmentDictionary[itemName].spDefense;
            equipment.speed = equipmentDictionary[itemName].speed;
            equipment.description = equipmentDictionary[itemName].description;
            equipment.sprite = equipmentDictionary[itemName].sprite;
            equipment.equipmentSlot = equipmentDictionary[itemName].equipmentSlot;
            tempItem = equipment;
        }
        if (tempItem == null)
            Debug.LogError(itemName + " is item not exits");

        return tempItem;
    }
}
