using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EquipmentSlot { Head, Weapon, Armor, Shoes }
public class EquipmentController : MonoBehaviour {

    public Button headSlotButton;
    public Button weaponSlotButton;
    public Button armorSlotButton;
    public Button shoesSlotButton;
    //public List<Button> buttons; // equipment slot buttons

    public InventoryController invetory;

    //[System.NonSerialized]
    public Equipment[] currentEquipment;

    public delegate void OnCurrentEquipmentChanged(Equipment oldItem, Equipment newItem);
    public static OnCurrentEquipmentChanged onEquipmentChanged;

    public void Awake()
    {
        headSlotButton.onClick.RemoveAllListeners();
        headSlotButton.onClick.AddListener(() => UnEquip(0));

        weaponSlotButton.onClick.RemoveAllListeners();
        weaponSlotButton.onClick.AddListener(() => UnEquip(1));

        armorSlotButton.onClick.RemoveAllListeners();
        armorSlotButton.onClick.AddListener(() => UnEquip(2));

        shoesSlotButton.onClick.RemoveAllListeners();
        shoesSlotButton.onClick.AddListener(() => UnEquip(3));

        if(currentEquipment.Length == 0)
        {
            int numerber = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
            currentEquipment = new Equipment[numerber];
        }
    }

    private void OnEnable()
    {
        onEquipmentChanged += OnEquipmentChanged;

        Refresh();
    }

    private void OnDisable()
    {
        onEquipmentChanged -= OnEquipmentChanged;
    }

    void OnEquipmentChanged(Equipment oldItem, Equipment newItem)
    {
        Refresh();
    }

    public void Refresh()
    {

        #region head ui setting
        if (currentEquipment[0] != null)
        {
            headSlotButton.image.overrideSprite = currentEquipment[0].sprite;
            headSlotButton.GetComponentInChildren<Text>().text = currentEquipment[0].itemName;
        }
        else
        {
            headSlotButton.image.overrideSprite = null;
            headSlotButton.GetComponentInChildren<Text>().text = "null";
        }
        #endregion

        #region weapon ui setting
        if (currentEquipment[1] != null)
        {
            weaponSlotButton.image.overrideSprite = currentEquipment[1].sprite;
            weaponSlotButton.GetComponentInChildren<Text>().text = currentEquipment[1].itemName;
        }
        else
        {
            weaponSlotButton.image.overrideSprite = null;
            weaponSlotButton.GetComponentInChildren<Text>().text = "null";
        }
        #endregion

        #region armor ui setting 
        if (currentEquipment[2] != null)
        {
            armorSlotButton.image.overrideSprite = currentEquipment[2].sprite;
            armorSlotButton.GetComponentInChildren<Text>().text = currentEquipment[2].itemName;
        }
        else
        {
            armorSlotButton.image.overrideSprite = null;
            armorSlotButton.GetComponentInChildren<Text>().text = "null";
        }
        #endregion

        #region shoes ui setting
        if (currentEquipment[3] != null)
        {
            shoesSlotButton.image.overrideSprite = currentEquipment[3].sprite;
            shoesSlotButton.GetComponentInChildren<Text>().text = currentEquipment[3].itemName;
        }
        else
        {
            shoesSlotButton.image.overrideSprite = null;
            shoesSlotButton.GetComponentInChildren<Text>().text = "null";
        }
        #endregion

        //for (int i = 0; i < currentEquipment.Length; i++)
        //{
        //    if(currentEquipment[i] != null)
        //    {
        //        buttons[i].image.overrideSprite = currentEquipment[i].sprite;
        //        buttons[i].GetComponentInChildren<Text>().text = currentEquipment[i].itemName;
        //    }
        //    else
        //    {
        //        buttons[i].image.overrideSprite = null;
        //        buttons[i].GetComponentInChildren<Text>().text = "null";
        //    }
        //}
    }

    public void Equip(Equipment equipment)
    {
        int slotIndex = (int)equipment.equipmentSlot;

        Equipment oldItem = null;

        if (currentEquipment[slotIndex] != null)
        {
            //return old equipment
            oldItem = currentEquipment[slotIndex];
            invetory.AddItemToInventory(oldItem);
        }

        currentEquipment[slotIndex] = equipment;

        if (onEquipmentChanged != null)
            onEquipmentChanged(oldItem, equipment);
        GameplayManager.instance.screenUiController.ShowStringOnScreen(string.Format("<color=blue>{0}</color> is equiped!", equipment.itemName));
    }

    public void UnEquip(int index)
    {
        //Debug.Log(index);
        if(currentEquipment[index] != null)
        {
            Equipment tempItem = currentEquipment[index];
            invetory.AddItemToInventory(tempItem);
            currentEquipment[index] = null;

            if (onEquipmentChanged != null)
                onEquipmentChanged(tempItem, null);
        }
    }

    public void UnequipAll()
    {
        for(int i = 0; i < currentEquipment.Length; i++)
        {
            UnEquip(i);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            UnequipAll();
    }
}
