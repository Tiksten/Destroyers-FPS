using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public float objectHealth;
    public GameObject DestroyedVersion;
    public GameObject WoodParticleSystem1;
    public GameObject WoodParticleSystem2;
    public GameObject WoodParticleSystem3;
    public GameObject WoodParticleSystem4;
    public GameObject WoodParticleSystem5;
    public GameObject WoodParticleSystem6;
    public GameObject WoodParticleSystem7;
    public GameObject WoodParticleSystem8;
    public GameObject WoodParticleSystem9;
    public GameObject WoodParticleSystem10;
    public Vector3 torque;
    public bool isExplosive = false;
    public float radius = 5f;
    public float force = 500f;
    public ParticleSystem sparksPrefab;
    public ParticleSystem smokePrefab;
    public void Break()
    {
        Instantiate(DestroyedVersion, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
        var WPS1 = Instantiate(WoodParticleSystem1, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WPS1, 5f);
        var WPS2 = Instantiate(WoodParticleSystem2, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WPS2, 5f);
        var WPS3 = Instantiate(WoodParticleSystem3, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WPS3, 5f);
        var WPS4 = Instantiate(WoodParticleSystem4, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WPS4, 5f);
        var WPS5 = Instantiate(WoodParticleSystem5, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WPS5, 5f);
        var WPS6 = Instantiate(WoodParticleSystem6, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WPS6, 5f);
        var WPS7 = Instantiate(WoodParticleSystem7, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WPS7, 5f);
        var WPS8 = Instantiate(WoodParticleSystem8, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WPS8, 5f);
        var WPS9 = Instantiate(WoodParticleSystem9, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WPS9, 5f);
        var WPS10 = Instantiate(WoodParticleSystem10, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WPS10, 5f);
        if(isExplosive)
            Explode();
    }
    public void TakeDamage(int damage)
    {
        objectHealth -= damage;
        if (objectHealth <= 0)
        {
            Break();
        } 
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
    var sparks = Instantiate(sparksPrefab, gameObject.transform.position, gameObject.transform.rotation);
    Destroy(sparks, 5f);
    var smoke = Instantiate(smokePrefab, gameObject.transform.position, gameObject.transform.rotation);
    Destroy(smoke, 5f);
    }
}