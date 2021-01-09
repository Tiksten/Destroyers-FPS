using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Animator animator;
    public DamageZone damageZone;
    public float timeBetweenAttacks = 0.5f;
    public bool canAttack = true;
    public float timeBetweenStrongAttacks = 1f;
    void Update()
    {
        if(Input.GetButton("Fire1") && canAttack) 
            StartCoroutine(Attack(false));
        else if(Input.GetButton("Fire2") && canAttack) 
            StartCoroutine(Attack(true));
    }
    private IEnumerator Attack(bool strongAttack)
    {
        damageZone.active = true;
        canAttack = false;
        var attackNum = Random.Range(1, 3);
        if(!strongAttack)
        {
            animator.SetBool("Attack" + attackNum, true);
            yield return new WaitForSeconds(timeBetweenAttacks);
            animator.SetBool("Attack" + attackNum, false);
        }
        else
        {
            animator.SetBool("StrongAttack", true);
            damageZone.damagePerTime = 100;
            yield return new WaitForSeconds(timeBetweenStrongAttacks);
            damageZone.damagePerTime = 50;
            animator.SetBool("StrongAttack", false);
        }
        canAttack = true;
        damageZone.active = false;

    }
}
