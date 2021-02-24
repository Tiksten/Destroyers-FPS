using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Animator animator;
    public SwordDamageZone damageZone;
    public float timeBetweenAttacks1 = 0.6f;
    public float timeBetweenAttacks2 = 0.6f;
    public float timeBetweenAttacks3 = 0.6f;

    public bool canAttack = true;
    public bool stopAttack = false;
    public float timeBetweenStrongAttacks1 = 0.05f;
    public float timeBetweenStrongAttacks2 = 0.05f;
    public float timeBetweenStrongAttacks3 = 0.05f;
    public int attackNum = 0;
    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0) && canAttack) 
            StartCoroutine(Attack(false));
        else if(Input.GetKey(KeyCode.Mouse1) && canAttack) 
            StartCoroutine(Attack(true));
        if(stopAttack)
        {
            animator.SetBool("StrongAttack", false);
            animator.SetBool("Attack" + attackNum, false);
            damageZone.active = false;
            stopAttack = false;
        }
    }
    public IEnumerator Attack(bool strongAttack)
    {
        canAttack = false;
        attackNum = Random.Range(1, 4);
        if(!strongAttack)
        {
            animator.SetBool("Attack" + attackNum, true);
            yield return new WaitForSeconds(timeBetweenAttacks1);
            damageZone.active = true;
            yield return new WaitForSeconds(timeBetweenAttacks2);
            damageZone.active = false;
            yield return new WaitForSeconds(timeBetweenAttacks3);
            animator.SetBool("Attack" + attackNum, false);
        }
        else
        {
            animator.SetBool("StrongAttack", true);
            yield return new WaitForSeconds(timeBetweenStrongAttacks1);
            damageZone.damage = 100;
            damageZone.active = true;
            yield return new WaitForSeconds(timeBetweenStrongAttacks2);
            damageZone.damage = 50;
            damageZone.active = false;
            yield return new WaitForSeconds(timeBetweenStrongAttacks2);
            animator.SetBool("StrongAttack", false);
        }
        canAttack = true;
        damageZone.active = false;

    }
}
