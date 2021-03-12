using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCameraMovement : MonoBehaviour
{

    float rotZ = 0f;

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, rotZ);

        if (Input.GetKey("a"))
        {
            AddRotation(0.15f);
        }
        else if(Input.GetKey("d"))
        {
            AddRotation(-0.15f);
        }
        else
        {
            if(rotZ < 180 && rotZ != 0)
                AddRotation(-0.15f);
            else if(rotZ > 180 && rotZ != 0)
                AddRotation(0.15f);
        }
    }

    public void AddRotation(float z)
    {
        rotZ += z;
        rotZ = Mathf.Clamp(rotZ, -4f, 4f);
    }
}
