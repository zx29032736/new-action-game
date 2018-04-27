using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryController : MonoBehaviour{

    public GameObject slotsParent;
    public ItemDetailController itemDetailController;

    [System.NonSerialized]
    public List<Item> myItems = new List<Item>();
    [System.NonSerialized]
    public int maxCount = 20;
    [System.NonSerialized]
    public ItemSlot[] slots;

    public ItemSlot selectedSlot = null;

    public delegate void OnInventoryItemChanged();
    public static event OnInventoryItemChanged onInventoryChanged;

    private void OnEnable()
    {
        if(slots == null)
            slots = slotsParent.GetComponentsInChildren<ItemSlot>();

        Init();

        onInventoryChanged += OnInventoryChanged;
    }

    private void OnDisable()
    {
        onInventoryChanged -= OnInventoryChanged;
    }

    void OnInventoryChanged()
    {
        RefreshIvenetory();
    }

    public void Init()
    {
        RefreshIvenetory();
    }

    public bool AddItemToInventory(Item item)
    {
        Item tempItem = myItems.Find(x => x.instanceID == item.instanceID);//Find(x => x.itemName == item.itemName);

        if (tempItem == null)
        {
            if (myItems.Count >= maxCount)
            {
                Debug.LogError("invetory item count > max count");
                return false;
            }

            myItems.Add(item);
        }
        else
        {
            if (tempItem.isStackable)
            {
                tempItem.Count++;
                Debug.Log("stackable!");
            }
            else
            {
                myItems.Add(ItemData.CreateItem(item.itemName));
            }
        }
        if (onInventoryChanged != null)
            onInventoryChanged();
        //RefreshIvenetory();
        //GameplayManager.instance.screenUiController.ShowStringOnScreen(string.Format("<color=blue>{0}</color> is added to the inventory!", item.itemName));
        return true;
    }

    public bool RemoveItemFormiventory(Item item)
    {
        if (myItems.Contains(item))
        {
            //Debug.Log(item.itemName + " is removed");
            myItems.Remove(item);
            if (onInventoryChanged != null)
                onInventoryChanged();
            //GameplayManager.instance.screenUiController.ShowStringOnScreen(string.Format("<color=blue>{0}</color> is removed from the inventory!", item.itemName));
            return true;
        }
        else
        {
            Debug.Log("item not exits");
            return false;
        }
    }

    public void UseItem()
    {
        selectedSlot.item.Use();

        if (selectedSlot.item.isStackable)
        {
            selectedSlot.item.Count--;
            if (selectedSlot.item.Count < 1)
            {
                DeselectItem();
                itemDetailController.Close();
                RemoveItemFormiventory(selectedSlot.item);
                selectedSlot = null;
                //return;
            }
        }
        else
        {
            DeselectItem();
            itemDetailController.Close();
            RemoveItemFormiventory(selectedSlot.item);
            selectedSlot = null;
            //return;
        }

        //RefreshIvenetory();
        if (onInventoryChanged != null)
            onInventoryChanged();
    }

    public void RefreshIvenetory()
    {
        foreach (var slot in slots)
        {
            slot.ClearButton();
        }

        for (int i = 0; i < myItems.Count; i++)
        {
            slots[i].SetupButton(myItems[i], this);
        }

    }

    public void SelectItem(ItemSlot selected)
    {
        if (selectedSlot == selected)
        {
            DeselectItem();
            itemDetailController.Close();
            selectedSlot = null;
            return;
        }

        DeselectItem();

        selectedSlot = selected;
        selectedSlot.OnItemClick();
        itemDetailController.Init();
    }

    public void DeselectItem()
    {
        foreach (var go in slots)
            go.OnItemDeselect();
    }

    public void DeleteItem()
    {
        if (selectedSlot != null)
            RemoveItemFormiventory(selectedSlot.item);
        selectedSlot = null;
        itemDetailController.Close();
    }
    public bool DeleteItem(string itemName, int count)
    {
        Item it = myItems.Find(x => x.itemName.Contains(itemName));
        if (it == null || it.Count + 1 < count) // it.count start form 0
            return false;

        it.Count -= count;

        bool success = false;
        if (it.Count < 0)
            success = RemoveItemFormiventory(it);

        return success;
    }

    public int FindItemCount(string name)
    {
        Item it = myItems.Find(x => x.itemName == name);

        if (it != null)
            return it.Count + 1;
        else
            return 0;
    }

    public void Close()
    {
        selectedSlot = null;
        gameObject.SetActive(false);
    }


}
