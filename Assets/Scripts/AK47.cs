using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK47 : MonoBehaviour
{
    public GameObject bulletHolePrefab;
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
    public Animator animator;

    private float timecode = 0;
    ReloadScript ammoScript;
    void Start()
    {
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
        rb.AddForce(cartrigeEjector.right * (cartrigeForce + Random.Range(0, 0.2f)), ForceMode.Impulse);
        rb.AddTorque(Random.Range(-15, 15), Random.Range(-15, 15), 0, ForceMode.Impulse);
        Shoot.pitch = Random.Range(1.3f, 1.5f);
        Shoot.Play ();
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Destructible target = hit.transform.GetComponent<Destructible>();
            PlayerHealth targetPlayer = hit.transform.GetComponent<PlayerHealth>();    //Optimize******************************************************
            SC_NPCEnemy targetEnemy = hit.transform.GetComponent<SC_NPCEnemy>();
            ButtonScript button = hit.transform.GetComponent<ButtonScript>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            if (targetPlayer != null)
            {
                targetPlayer.TakeDamage(damage);
            }
            if (targetEnemy != null)
            {
                targetEnemy.ApplyDamage(damage);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            if (button != null)
                button.ClickButton();

            Instantiate(impactEffect1, hit.point, Quaternion.LookRotation(hit.normal));
            Instantiate(impactEffect2, hit.point, Quaternion.LookRotation(hit.normal));
            Instantiate(impactEffect3, hit.point, Quaternion.LookRotation(hit.normal));
            GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
            bulletHole.transform.parent = hit.transform;
            
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
