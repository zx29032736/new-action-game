using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {

    public static GameplayManager instance;

    private void Awake()
    {
        instance = this;
        equipmentController.Awake();
    }

    static bool isPauseInput = false;
    public static bool IsPauseInput
    {
        get
        {
            if (instance.dialogController.isActiveAndEnabled || instance.storeController.isActiveAndEnabled)
                return true;
            else
                return false;
        }
    }

    private void Update()
    {
        if (IsPauseInput)
            return;

        if (Input.GetKeyDown(KeyCode.I))
        {
            OnOffInventory();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            OnOffEquipmentPanel();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnoOffQuestPanel();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            OnOffProfile();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnOffSettingPanel();
        }

    }

    #region OnOff Panel

    public void OnOffInventory()
    {
        inventoryController.gameObject.SetActive(!inventoryController.gameObject.activeSelf);
    }

    public void OnOffEquipmentPanel()
    {
        equipmentController.gameObject.SetActive(!equipmentController.gameObject.activeSelf);
    }

    public void OnoOffQuestPanel()
    {
        questController.gameObject.SetActive(!questController.gameObject.activeSelf);
    }

    public void OnOffSettingPanel()
    {
        settingsController.gameObject.SetActive(!settingsController.gameObject.activeSelf);
    }

    public void OnOffProfile()
    {
        profileController.gameObject.SetActive(!profileController.gameObject.activeSelf);
    }

    #endregion

    public InventoryController inventoryController;
    public EquipmentController equipmentController;
    public DialogController dialogController;
    public ScreenUiController screenUiController;
    public StoreController storeController;
    public QuestController questController;
    public GameSettingController settingsController;
    public ProfileController profileController;


}
