using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public GameObject sparksEffect;
    public GameObject smokeEffect;
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
        GameObject box = hitInfo.gameObject;
        var sparks = Instantiate(sparksEffect, transform.position, Quaternion.identity);
        var smoke = Instantiate(smokeEffect, transform.position, Quaternion.identity);
        Destroy(sparks, 0.5f);
        Destroy(smoke, 5f);
        Destructible boxScript = box.GetComponent<Destructible>();
        if(boxScript != null)
        {
            boxScript.Break();
        }
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
            Destructible ds = nearbyObject.GetComponent<Destructible>();
            if (ds != null && (Random.Range(1, 100) < 40))
            {
                ds.Break();
            }
            PlayerHealth ph = nearbyObject.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(20);
            }
        }
    }
}