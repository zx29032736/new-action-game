
using UnityEngine;
using UnityEngine.EventSystems;
public class DragableUI : MonoBehaviour, IDragHandler {

    private void Update()
    {
        if (transform.position.x < 0)
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        if (transform.position.y < 0)
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        if (transform.position.x > Screen.width)
            transform.position = new Vector3(Screen.width, transform.position.y, transform.position.z);
        if (transform.position.y > Screen.height)
            transform.position = new Vector3(transform.position.x, Screen.height, transform.position.z);
    }

    public void OnDrag(PointerEventData eventData)
    {

        transform.position = Input.mousePosition;
    }
}
