using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {

    //[System.NonSerialized]
    public Item item;
    public InventoryController iController;

    public Button itemButton;
    public Image itemImage;
    public Text countText;

    private void Awake()
    {
        itemButton.onClick.RemoveAllListeners();
        itemButton.onClick.AddListener(() => iController.SelectItem(this));
    }

    public void SetupButton(Item it, InventoryController controller)
    {
        item = it;
        iController = controller;
        countText.gameObject.SetActive(true);
        itemImage.overrideSprite = item.sprite;
        countText.text = "count : " + item.Count;

        itemButton.GetComponentInChildren<Text>().text = item.itemName;
        itemButton.interactable = true;
    }

    public void ClearButton()
    {
        item = null;
        countText.gameObject.SetActive(false);
        itemButton.GetComponentInChildren<Text>().text = "null";
        itemImage.overrideSprite = null;
        itemButton.interactable = false;
    }

    public void OnItemClick()
    {
        itemButton.image.color = Color.red;
    }

    public void OnItemDeselect()
    {
        itemButton.image.color = Color.black;
    }
}
