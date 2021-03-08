using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public Transform playerBody;

    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void MoveTo(float x, float y)
    {
        transform.localRotation = Quaternion.Euler(-y, 0f, 0f);
        playerBody.Rotate(Vector3.up * x);
    }

    public void AddRotation(float x, float y)
    {
        xRotation -= y;
        playerBody.Rotate(Vector3.up * (Input.GetAxis("Mouse Y") + x) * mouseSensitivity * Time.deltaTime);
    }
}
