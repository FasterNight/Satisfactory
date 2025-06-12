using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float distance = 5.0f;
    [SerializeField] float height = 2.0f;
    [SerializeField] float rotationSpeed = 5.0f;

    float currentX = 0.0f;
    float currentY = 0.0f;
    [SerializeField] float yMinLimit = -20f;
    [SerializeField] float yMaxLimit = 80f;


    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        currentX += Input.GetAxis("Mouse X") * rotationSpeed;
        currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);


        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 negDistance = new Vector3(0, 0, -distance);
        Vector3 position = rotation * negDistance + target.position + new Vector3(0, height, 0);

        transform.position = position;
        transform.LookAt(target.position + Vector3.up * height);
    }
}
