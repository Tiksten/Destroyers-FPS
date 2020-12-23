using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyParticleSystem: MonoBehaviour
{
    public GameObject itemToSpawn;
    public Vector3 torque;
    public float force = 5f;
    public int itemNumber = 5;
    public int timeToDestroy;
    public Quaternion angle;
    public float degrees = 1024;
    public void Start()
    {
        while (itemNumber > 0)
        {
            angle.x = Random.Range (-degrees, degrees);
            angle.y = Random.Range (-degrees, degrees);
            angle.z = Random.Range (-degrees, degrees);
            Instantiate(itemToSpawn, gameObject.transform.position, angle);
            itemNumber -= 1;
        }
    }
}