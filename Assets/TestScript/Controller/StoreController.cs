using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreController : MonoBehaviour {

    public GameObject slotParent;
    public StoreItemSlot[] itemSlot;

    System.Action actionCallback = null;
    public StoreItemSlot selectedItem = null;

    public List<Item> containsItem;


    public void Init(List<string> itemList,System.Action callback = null)
    {
        selectedItem = null;
        actionCallback = callback;

        ItemDeselected();

        for (int i = 0; i < itemList.Count; i++)
        {
            Item it = ItemData.allItemDictionary[itemList[i]];//CreateItem(itemList[i]);

            if (it != null)
            {
                itemSlot[i].ClearButton();

                containsItem.Add(it);
                itemSlot[i].SetupButton(it);
            }
        }
        gameObject.SetActive(true);
    }

    public void OnItemSelected(StoreItemSlot selected)
    {
        ItemDeselected();
        if (selectedItem == selected)
            return;

        selectedItem = selected;
        selectedItem.myButton.image.color = Color.green;
    }

    public void ItemDeselected()
    {
        selectedItem = null;
        foreach (var bt in itemSlot)
        {
            bt.myButton.image.color = Color.white;
        }
    }

    public void PurchaseItem()
    {
        if (selectedItem != null)
        {
            Item item = ItemData.CreateItem(selectedItem.myItem.itemName);

            if(GameplayManager.instance.inventoryController.AddItemToInventory(item))
            {           
                // cost money
                ItemDeselected();
            }


        }
    }

    public void ClosePanel()
    {
        foreach (var bt in itemSlot)
        {
            bt.ClearButton();
        }
        selectedItem = null;

        if (actionCallback != null)
            actionCallback();

        gameObject.SetActive(false);
    }
}
