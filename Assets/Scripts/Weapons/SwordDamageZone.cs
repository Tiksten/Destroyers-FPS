using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamageZone : MonoBehaviour
{
    public int damage = 15;
    public bool active = true;
    public Sword sword;
    public ParticleSystem hitEffect;
    public ParticleSystem hitEnemyEffect;
    public AudioSource swordAudioSource;
    public AudioClip[] swordHitSounds;

    [HideInInspector]
    public bool canDamage = true;
    void OnTriggerStay(Collider hitInfo)
    {
        if(hitInfo.tag != "Player")
        {
            GameObject target = hitInfo.gameObject;
            var damageScript = target.GetComponent<Destructible>();
            var targetEnemy = target.GetComponent<SC_NPCEnemy>();
            if (active && canDamage)
            {
                sword.stopAttack = true;
                if(damageScript != null)
                {
                    damageScript.TakeDamage(damage);
                    Instantiate(hitEffect, hitInfo.transform.position, hitInfo.transform.rotation);
                }
                if (targetEnemy != null)
                {
                    targetEnemy.ApplyDamage(damage);
                    Instantiate(hitEnemyEffect, hitInfo.transform.position, hitInfo.transform.rotation);
                }
                swordAudioSource.clip = swordHitSounds[Random.Range(0, swordHitSounds.Length)];
                swordAudioSource.Play();
            }
        }
    }
}
