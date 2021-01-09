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
    [HideInInspector]
    public bool active = true;
    [HideInInspector]
    public bool canDamage = true;
    private IEnumerator OnTriggerStay(Collider hitInfo)
    {
        GameObject target = hitInfo.gameObject;
        var damageScript = target.GetComponent<Destructible>();
        var playerDamageScript = target.GetComponent<PlayerHealth>();
        if(!startActive) active = false;
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

            }
            else if(playerDamageScript != null) 
            {
                PlayerHealth targetScript = playerDamageScript;
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
            }
        }
    }
}
