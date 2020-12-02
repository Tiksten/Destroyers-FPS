using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public GameObject hitEffect;
    public int damage = 40;

    void OnTriggerEnter(Collider hitInfo)
    {
        //PlayerHealth enemy = hitInfo.GetComponent<PlayerHealth>();
        //if (enemy != null)
        //{
        //    enemy.TakeDamage(damage);
        //}
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
        Destroy(gameObject);
    }
    void Start()
    {
        Destroy(gameObject, 5f);
    }
}