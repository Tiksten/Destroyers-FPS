using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK47 : MonoBehaviour
{
    public AudioSource Shoot;
    public ParticleSystem muzzleFlash;
    public Camera fpsCam;
    public int range;
    public int damage;
    public float impactForce;
    public Transform cartrigeEjector;
    public GameObject cartrigePrefab;
    public ParticleSystem impactEffect1;
    public ParticleSystem impactEffect2;
    public ParticleSystem impactEffect3;
    public ParticleSystem barrelSmoke;
    public float firespeed = 0.1f;
    public float cartrigeForce = 5f;
    public float force = 1f;
    public float radius = 1f;

    private float timecode = 0;
    void Update()
    {
        if ((Time.time >= timecode) && Input.GetButton("Fire1"))
        {
            Shot();
            timecode = Time.time + firespeed;
        }
    }

    void Shot() 
    { 
        muzzleFlash.Play();
        GameObject cartrige = Instantiate(cartrigePrefab, cartrigeEjector.position, cartrigeEjector.rotation);
        Rigidbody rb = cartrige.GetComponent<Rigidbody>();
        rb.AddForce(cartrigeEjector.right * cartrigeForce, ForceMode.Impulse);
        Shoot.Play ();
        RaycastHit hit;
        barrelSmoke.Play();
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Destructible target = hit.transform.GetComponent<Destructible>();
            PlayerHealth targetPlayer = hit.transform.GetComponent<PlayerHealth>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            if (targetPlayer != null)
            {
                targetPlayer.TakeDamage(damage);
            }
            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            ParticleSystem impactGO1 = Instantiate(impactEffect1, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO1, 3f);
            ParticleSystem impactGO2 = Instantiate(impactEffect2, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO2, 3f);
            ParticleSystem impactGO3 = Instantiate(impactEffect3, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO3, 3f);

            Collider[] colliders = Physics.OverlapSphere(hit.point, radius);
            foreach (Collider nearbyObject in colliders)
            {
                Rigidbody rd = nearbyObject.GetComponent<Rigidbody>();
                if (rd != null)
                {
                    rd.AddExplosionForce(force, hit.point, radius);
                }
            }
        }
    }
}
