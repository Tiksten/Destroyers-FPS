using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public float objectHealth;
    public GameObject destroyedVersion;
    public GameObject[] particleSystems;

    [HideInInspector]
    public Vector3 torque;

    public bool isExplosive = false;

    public float radius = 5f;

    public float force = 500f;

    public AudioSource audioSource;

    public AudioClip woodHit;
    public void Break()
    {
        if(destroyedVersion != null)
            Instantiate(destroyedVersion, gameObject.transform.position, gameObject.transform.rotation);

        foreach (GameObject gmObj in particleSystems)
        {
            var gmObjV = Instantiate(gmObj, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gmObjV, 5);
        }

        if(isExplosive)
            Explode();

        Destroy(gameObject);
    }
    public void TakeDamage(int damage)
    {
        objectHealth -= damage;
        if (objectHealth <= 0)
        {
            Break();
        }
        audioSource.clip = woodHit;
        audioSource.Play();

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

            SC_NPCEnemy eh = nearbyObject.GetComponent<SC_NPCEnemy>();
            if (eh != null)
            {
                eh.ApplyDamage(100);
            }
        }
        foreach (GameObject gmObj in particleSystems)
        {
            Instantiate(gmObj, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gmObj, 5);
        }
    }
}