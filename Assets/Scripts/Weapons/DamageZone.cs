using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damagePerTime = 10;
    public float timeBetweenDamage = 1f;
    public int damage = 15;
    public bool damageOnce = false;
    public bool startActive = true;
    public bool active = true;
    public ParticleSystem hitEffect;
    [HideInInspector]
    public bool canDamage = true;
    void Start()
    {
        if(!startActive) active = false;
    }
    private IEnumerator OnTriggerStay(Collider hitInfo)
    {
        GameObject target = hitInfo.gameObject;
        var damageScript = target.GetComponent<Destructible>();
        if(active && canDamage)
        {
            if(damageScript != null) 
            {
                Destructible targetScript = damageScript;
                if(damageOnce)
                {
                    active = false;
                    targetScript.TakeDamage(damage);
                }
                else if(!damageOnce)
                {
                    targetScript.TakeDamage(damagePerTime);
                    canDamage = false;
                    yield return new WaitForSeconds(timeBetweenDamage);
                    canDamage = true;
                }
                Instantiate(hitEffect, hitInfo.transform);
            }
        }
    }
}
