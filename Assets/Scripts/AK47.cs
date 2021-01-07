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
    public Vector3 upRecoil;
    Vector3 originalRotation;
    public Vector3 maxRecoil;
    public bool recoil = false;
    public Animator animator;

    private float timecode = 0;
    ReloadScript ammoScript;
    void Start()
    {
        originalRotation = transform.localEulerAngles;
        ammoScript = GetComponent<ReloadScript>();
    }
    void Update()
    {
        if(Input.GetButton("Fire1") && !ammoScript.noAmmo && !ammoScript.reloading)
        {
            animator.SetBool("Shooting", true);
        }
        else{
            animator.SetBool("Shooting", false);
        }
        if ((Time.time >= timecode) && Input.GetButton("Fire1") && !ammoScript.noAmmo && !ammoScript.reloading)
        {
            Shot();
            timecode = Time.time + firespeed;
            barrelSmoke.Stop();
            ammoScript.currentAmmo--;
        }
        else if (Input.GetButtonUp("Fire1") & !Input.GetButton("Fire1"))
        {
            barrelSmoke.Play();
        }
    }
    void Shot() 
    { 
        muzzleFlash.Play();
        GameObject cartrige = Instantiate(cartrigePrefab, cartrigeEjector.position, cartrigeEjector.rotation);
        Destroy(cartrige, 2f);
        Rigidbody rb = cartrige.GetComponent<Rigidbody>();
        rb.AddForce(cartrigeEjector.right * cartrigeForce, ForceMode.Impulse);
        Shoot.pitch = Random.Range(1.3f, 1.5f);
        Shoot.Play ();
        RaycastHit hit;
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

            Instantiate(impactEffect1, hit.point, Quaternion.LookRotation(hit.normal));
            Instantiate(impactEffect2, hit.point, Quaternion.LookRotation(hit.normal));
            Instantiate(impactEffect3, hit.point, Quaternion.LookRotation(hit.normal));

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
