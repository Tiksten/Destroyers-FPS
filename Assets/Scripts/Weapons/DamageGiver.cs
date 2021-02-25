using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    static public void GiveDamage(GameObject target, int damage)
    {
        Vector3 torque;
        float force = 50f;

        Destructible target1 = target.GetComponent<Destructible>();
        PlayerHealth target2 = target.GetComponent<PlayerHealth>();
        SC_NPCEnemy target3 = target.GetComponent<SC_NPCEnemy>();
        ButtonScript target4 = target.GetComponent<ButtonScript>();

        if (target1 != null)
            target1.TakeDamage(damage);

        else if (target2 != null)
            target2.TakeDamage(damage);

        else if (target3 != null)
            target3.ApplyDamage(damage);

        else if (target4 != null)
            target4.ClickButton();

        if (target.GetComponent<Rigidbody>() != null)
        {
            Rigidbody rb = target.GetComponent<Rigidbody>();
            torque.x = Random.Range(-force, force);
            torque.y = Random.Range(-force, force);
            torque.z = Random.Range(-force, force);
            rb.AddForce(torque, ForceMode.Impulse);
        }
    }  
}
