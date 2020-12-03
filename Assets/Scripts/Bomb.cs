using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public GameObject hitEffect;
    public int damage = 40;
    public float radius = 5f;
    public float force = 700f;
    void OnTriggerEnter(Collider hitInfo)
    {
        //PlayerHealth enemy = hitInfo.GetComponent<PlayerHealth>();
        //if (enemy != null)
        //{
        //    enemy.TakeDamage(damage);
        //}
        Explode();
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
        Destroy(gameObject);
    }
    void Start()
    {
        Destroy(gameObject, 10f);
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }
        }
    }
}