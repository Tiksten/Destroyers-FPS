using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    private float timeToDestroy = 5;

    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}