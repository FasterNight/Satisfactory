using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float thirdPersonDistance = 5.0f;
    [SerializeField] float height = 2.0f;
    [SerializeField] float rotationSpeed = 5.0f;
    [SerializeField] float mouseSensitivity = 2.0f;

    float rotationX = 0.0f;
    float rotationY = 0.0f;
    [SerializeField] float yMinLimit = -20f;
    [SerializeField] float yMaxLimit = 80f;

    private bool isFirstPerson = false;

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (Input.GetKeyDown(KeyCode.C))
        {
            isFirstPerson = !isFirstPerson;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX += mouseX;
        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, yMinLimit, yMaxLimit);

        if (isFirstPerson)
        {
            target.rotation = Quaternion.Euler(0f, rotationX, 0f);
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        if (isFirstPerson)
        {
            // 1ère personne
            transform.position = target.position + Vector3.up * (height - 0.5f) + transform.forward * 0.4f;
            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0f);
        }
        else
        {
            // 3ème personne
            Quaternion camRotation = Quaternion.Euler(rotationY, rotationX, 0f);
            Vector3 camOffset = camRotation * new Vector3(0, 0, -thirdPersonDistance);
            Vector3 desiredPosition = target.position + Vector3.up * height + camOffset;

            transform.position = desiredPosition;
            transform.rotation = camRotation;
        }
    }
}
