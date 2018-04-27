using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemSlot : MonoBehaviour {

    public Item myItem;
    public StoreController storeController;

    public Button myButton;
    public Text nameText;
    public Text priceText;
    public Image icon;

    private void Awake()
    {
        myButton.onClick.RemoveAllListeners();
        myButton.onClick.AddListener(() => storeController.OnItemSelected(this));
    }

    public void SetupButton(Item it)
    {
        myItem = it;

        nameText.text = myItem.itemName;
        priceText.text = myItem.purchasePrice.ToString();
        icon.overrideSprite = it.sprite;
        gameObject.SetActive(true);
    }

    public void ClearButton()
    {
        myItem = null;
        nameText.text = "";
        priceText.text = "";
        icon.overrideSprite = null;
        gameObject.SetActive(false);
    }
}
