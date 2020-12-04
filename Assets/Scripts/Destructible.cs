using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject DestroyedVersion;
    public Vector3 torque;
    public void Break()
    {
        Instantiate(DestroyedVersion, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
    }
}