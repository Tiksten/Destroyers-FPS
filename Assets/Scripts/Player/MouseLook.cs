using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public Transform playerBody;

    float xRotation = 0f;
    float zRotation = 0f;

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

        transform.localRotation = Quaternion.Euler(xRotation, 0f, zRotation);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void MoveTo(float x, float y)
    {
        transform.localRotation = Quaternion.Euler(-y, 0f, 0f);
        playerBody.Rotate(Vector3.up * x);
    }

    public void AddRotation(float x, float y, float z)
    {
        xRotation -= y;
        playerBody.Rotate(Vector3.up * (Input.GetAxis("Mouse Y") + x) * mouseSensitivity * Time.deltaTime);
        zRotation += z;
        zRotation = Mathf.Clamp(zRotation, -4f, 4f);
    }

    public void AddRotation(Vector3 rot)
    {
        xRotation -= rot.y;
        playerBody.Rotate(Vector3.up * (Input.GetAxis("Mouse Y") + rot.x) * mouseSensitivity * Time.deltaTime);
        zRotation -= rot.z;
    }

    public void LerpRotation(Vector3 rotA, Vector3 rotB, float t)
    {
        var rotLerp = Vector3.Lerp(rotA, rotB, t);

        xRotation -= rotLerp.y;
        playerBody.Rotate(Vector3.up * (Input.GetAxis("Mouse Y") + rotLerp.x) * mouseSensitivity * Time.deltaTime);
        zRotation -= rotLerp.z;
    }
}
