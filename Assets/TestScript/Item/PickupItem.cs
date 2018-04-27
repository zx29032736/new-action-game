using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour {

    public string itemName = "";
    public bool isOnScene = false;
    public Item item;

    private void Start()
    {
        if (isOnScene)
        {
            item = ItemData.CreateItem(itemName);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetupItem(string iName)
    {
        itemName = iName;
        isOnScene = true;
        //
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool isPickedUp = GameplayManager.instance.inventoryController.AddItemToInventory(item);
            if (isPickedUp)
            {
                //gameObject.SetActive(false);
                GameplayManager.instance.screenUiController.ShowStringOnScreen(string.Format("<color=blue>{0}</color> is picked up!", itemName));
                Destroy(gameObject);

            }
            else
                Debug.Log("bag is full");
        }
    }
}
