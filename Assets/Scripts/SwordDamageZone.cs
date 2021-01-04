using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamageZone : MonoBehaviour
{
    public int damage = 15;
    public bool active = true;
    public Sword sword;
    public ParticleSystem hitEffect;
    [HideInInspector]
    public bool canDamage = true;
    void OnTriggerStay(Collider hitInfo)
    {
        if(hitInfo.tag != "Player")
        {
            GameObject target = hitInfo.gameObject;
            var damageScript = target.GetComponent<Destructible>();
            if(active && canDamage)
            {
                sword.stopAttack = true;
                if(damageScript != null)
                {
                    Destructible targetScript = damageScript;
                    targetScript.TakeDamage(damage);
                    Instantiate(hitEffect, hitInfo.transform);
                }
            }
        }
    }
}
