using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCameraMovement : MonoBehaviour
{
    public Transform qPosCheck;
    public Transform ePosCheck;

    public Camera cam;

    public MeshRenderer playerBody;

    public float rotChangeStep = 0.5f;
    public float posChangeStep = 0.5f;

    public float rotTiltChangeStep = 0.5f;
    public float posTiltChangeStep = 0.5f;

    public float maxRot = 4;
    public float maxPos = 1;

    public float maxPlayerTiltRot = 15;
    public float maxPlayerTiltPos = 5;

    float rotZ = 0f;

    private float camDefaultPosY;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float checkDistance;

    private void Start()
    {
        camDefaultPosY = cam.transform.localPosition.y;
    }

    void Update()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, rotZ);
        var pos = cam.transform.localPosition;
        var playerIsTiltingCam = Input.GetKey("q") || Input.GetKey("e");


        //Cam rot
        if (!playerIsTiltingCam)
        {
            if (Input.GetKey("a"))
            {
                AddRotation(rotChangeStep);
                cam.transform.localPosition = Vector3.Lerp(pos, new Vector3(-maxPos, camDefaultPosY, 0), posChangeStep);
            }
            else if (Input.GetKey("d"))
            {
                AddRotation(-rotChangeStep);
                cam.transform.localPosition = Vector3.Lerp(pos, new Vector3(maxPos, camDefaultPosY, 0), posChangeStep);
            }
            else
            {
                if (rotZ > 0)
                    AddRotation(-rotChangeStep);
                else if (rotZ < 0)
                    AddRotation(rotChangeStep);
                else if (rotZ > -0.1f && rotZ < 0.1f)
                    transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }


        //Cam pos
        var isZero = pos.x < 0.05f && pos.x > -0.05f;

        if (!isZero)
        {
            cam.transform.localPosition = Vector3.Lerp(pos, new Vector3(0, camDefaultPosY, 0), posChangeStep);
        }
        else
            cam.transform.localPosition = new Vector3(0, camDefaultPosY, 0);

        if (Input.GetKey("q") && !Physics.CheckSphere(qPosCheck.position, checkDistance, groundMask))
        {
            AddRotation(rotTiltChangeStep);
            cam.transform.localPosition = Vector3.Lerp(pos, new Vector3(-maxPlayerTiltPos, camDefaultPosY, 0), posTiltChangeStep);
            playerBody.enabled = false;
        }
        else if (Input.GetKey("e") && !Physics.CheckSphere(ePosCheck.position, checkDistance, groundMask))
        {
            AddRotation(-rotTiltChangeStep);
            cam.transform.localPosition = Vector3.Lerp(pos, new Vector3(maxPlayerTiltPos, camDefaultPosY, 0), posTiltChangeStep);
            playerBody.enabled = false;
        }
        else
            if(cam.transform.localPosition == new Vector3(0, camDefaultPosY, 0))
                playerBody.enabled = true;
    }

    public void AddRotation(float z)
    {
        rotZ += z;
        rotZ = Mathf.Clamp(rotZ, -maxRot, maxRot);
    }
}
