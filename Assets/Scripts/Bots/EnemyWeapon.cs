using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public GameObject bulletHolePrefab;
    public AudioSource shotSound;
    public Transform shootingPointA;
    public Transform shootingPointB;
    public ParticleSystem muzzleFlash;
    public GameObject cartrigePrefab;
    public Transform cartrigeEjector;
    public ParticleSystem shotEffect;

    public GameObject hitEffect;

    public void Shot(float damage)
    {
        RaycastHit hit3;

        if (Physics.Raycast(shootingPointA.position, shootingPointB.position - shootingPointA.position, out hit3, 100))
        {
            Helper.GiveDamage(hit3.collider.gameObject, damage);

            Debug.DrawRay(shootingPointA.position, shootingPointB.position - shootingPointA.position, Color.red, 1);

            Destroy(Instantiate(hitEffect, hit3.point, Quaternion.LookRotation(hit3.normal)), 1);

            var bh = Instantiate(hitEffect, hit3.point, Quaternion.LookRotation(hit3.normal));
            bh.transform.SetParent(hit3.transform);
            Destroy(bh, 30);
        }

        shotSound.pitch = Random.Range(0.9f, 1.1f);
        shotSound.Play();

        muzzleFlash.Play();
        var cartrige = Instantiate(cartrigePrefab, cartrigeEjector.position, cartrigeEjector.rotation);
        Destroy(cartrige, 2f);

        var rb = cartrige.GetComponent<Rigidbody>();
        rb.AddForce(cartrigeEjector.right * (1 + Random.Range(0, 0.2f)), ForceMode.Impulse);
        rb.AddTorque(Random.Range(-15, 15), Random.Range(-15, 15), 0, ForceMode.Impulse);
    }
}
