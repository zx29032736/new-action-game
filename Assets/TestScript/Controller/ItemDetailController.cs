using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailController : MonoBehaviour {

    public GameObject potionPanel;
    public Text name_text;
    public Text description_text;
    public Image image;
    public Button useButton;
    public Button deleteButton;

    public InventoryController invetory;

    private void OnEnable()
    {
        InventoryController.onInventoryChanged += OnInventoryChanged;
    }
    private void OnDisable()
    {
        InventoryController.onInventoryChanged -= OnInventoryChanged;
    }

    void OnInventoryChanged()
    {
        if (invetory.selectedSlot == null)
            gameObject.SetActive(false);
    }

    public void Init()
    {
        Item selectedItem = invetory.selectedSlot.item;

        if (selectedItem == null)
        {
            return;
        }

        name_text.text = selectedItem.itemName;
        description_text.text = selectedItem.description;
        image.overrideSprite = selectedItem.sprite;

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
