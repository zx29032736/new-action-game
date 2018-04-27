using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {

    public GameplayManager manager;

	public static void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public static void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        manager = GameplayManager.instance;
        //Cursor.SetCursor(new Texture2D(10, 10), Vector2.zero, CursorMode.Auto);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (manager.equipmentController.isActiveAndEnabled || manager.inventoryController.isActiveAndEnabled || 
            manager.dialogController.isActiveAndEnabled || manager.storeController.isActiveAndEnabled || manager.questController.isActiveAndEnabled
            || manager.settingsController.isActiveAndEnabled || manager.profileController.isActiveAndEnabled
            )
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            if (Cursor.visible)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
 
    }
}
