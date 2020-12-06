using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    public float minTime = 5f;
    public float maxTime = 10f;
    public Vector3 torque;
    public float force = 50f;
    void Start()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        torque.x = Random.Range (-force, force);
        torque.y = Random.Range (-force, force);
        torque.z = Random.Range (-force, force);
        rb.AddForce(torque, ForceMode.Impulse);
        Destroy(gameObject, Random.Range(minTime, maxTime));
    }

}
