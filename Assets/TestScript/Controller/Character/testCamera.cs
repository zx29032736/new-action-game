using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class testCamera : MonoBehaviour {

    public Transform target;
    public Transform targetBody;

    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;
    public float yMinLimit = -10;
    public float yMaxLimit = 70;

    public float x = 20.0f;
    public float y = 0.0f;

    public Quaternion aim;
    public float aimAngle = 8;

    bool isCursorPointerObject = false;

    private void Start()
    {
        if (!target)
            targetBody = target;

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        //DisableCursor();
        //EnableCursor();
    }

    private void LateUpdate()
    {
        //isCursorPointerObject = EventSystem.current.IsPointerOverGameObject();

        //if (isCursorPointerObject)
           // EnableCursor();
        //else
           // DisableCursor();

        if (target == null || GameplayManager.IsPauseInput)
            return;

        if (!targetBody)
        {
            targetBody = target;
        }

        x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        CalculateRotation();

        if (Input.GetButton("Fire1") || Input.GetButton("Fire2") || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0  || !isCursorPointerObject)
        {
            targetBody.transform.rotation = Quaternion.Euler(0, x, 0);
        }

        Vector3 positiona = target.position - (transform.rotation * Vector3.forward * 2.0f + new Vector3(0.0f, -1.2f, 0.0f));
        transform.position = positiona;

    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    void CalculateRotation()
    {
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        transform.rotation = rotation;
        aim = Quaternion.Euler(y - aimAngle, x, 0);
    }

    public void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
