using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public GameObject bulletHolePrefab;
    public AudioSource shotSound;
    public Transform shootingPointA;
    public Transform shootingPointB;
    public GameObject cartrigePrefab;
    public Transform cartrigeEjector;
    public GameObject[] shotEffects;

    public void Shot(float damage)
    {
        shootingPointA.LookAt(shootingPointB);

        Helper.ShotForward(shootingPointA, damage, 100, shotEffects);

        shotSound.pitch = Random.Range(0.9f, 1.1f);
        shotSound.Play();

        var cartrige = Instantiate(cartrigePrefab, cartrigeEjector.position, cartrigeEjector.rotation);
        Destroy(cartrige, 2f);

        var rb = cartrige.GetComponent<Rigidbody>();
        rb.AddForce(cartrigeEjector.right * (1 + Random.Range(0, 0.2f)), ForceMode.Impulse);
        rb.AddTorque(Random.Range(-15, 15), Random.Range(-15, 15), 0, ForceMode.Impulse);
    }
}
