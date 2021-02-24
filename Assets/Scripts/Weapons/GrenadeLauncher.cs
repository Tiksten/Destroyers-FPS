using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : MonoBehaviour
{
    public AudioSource Shoot;

    public Transform firePoint;
    public GameObject bulletPrefab;
    public Transform cartrigeEjector;
    public GameObject cartrigePrefab;

    public float firespeed = 0.1f;

    public float bulletForce = 20f;
    public float cartrigeForce = 5f;

    public float timecode = 0;
    ReloadScript ammoScript;

    // Update is called once per frame
    void Start() {
       ammoScript = GetComponent<ReloadScript>(); 
    }
    void Update()
    {

        if ((Time.time >= timecode) && Input.GetButton("Fire1") && !ammoScript.noAmmo && !ammoScript.reloading)
        {
            Shot();
            timecode = Time.time + firespeed;
            ammoScript.currentAmmo--;
        }
    }

    void Shot() 
    { 
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        GameObject cartrige = Instantiate(cartrigePrefab, cartrigeEjector.position, cartrigeEjector.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        Rigidbody rb2 = cartrige.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode.Impulse);
        rb2.AddForce(cartrigeEjector.right * cartrigeForce, ForceMode.Impulse);
        Shoot.Play ();
    }
}
