using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractingController : MonoBehaviour {

    public Transform interactingOBJ = null;

    private void OnTriggerStay(Collider other)
    {
        if (GameplayManager.IsPauseInput)
            return;

        if (other.CompareTag("NPC"))
        {
            if (interactingOBJ != null)
            {
                if (Distance(other.transform) > Distance(interactingOBJ))
                    interactingOBJ = other.transform;
            }
            else
                interactingOBJ = other.transform;


            GameplayManager.instance.screenUiController.DisplayInteractUI(interactingOBJ.transform);

            if (Input.GetKeyDown(KeyCode.Z))
                interactingOBJ.SendMessage("Interact", null, SendMessageOptions.DontRequireReceiver);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        interactingOBJ = null;
        GameplayManager.instance.screenUiController.DisableInteractUI();
    }

    float Distance(Transform target)
    {
        return (transform.position - target.position).magnitude;
    }
}
