using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    [System.Serializable]
    public class SprayPattern
    {
        public string weaponPatternName;
        public Vector2[] weaponSprayPattern;
        public Vector2[] weaponCyclePattern; //Pattern goes after spray ending untill not out of ammo

        [HideInInspector]
        //private float _patternShotRandomnessRange;

        //public float maxPatternShotRandomnessRange;
        
        //public float patternShotRandomnessRange //Every shot random range (very small but gets bigger)
        //{
        //    get => _patternShotRandomnessRange;
        //    set
        //    {
        //        if (value < maxPatternShotRandomnessRange)
        //        {
        //            _patternShotRandomnessRange = value;
        //        }
        //    }
        //}


        //public float shotWithoutScopeRandomRange; //From hip shooting (should be big enough gets bigger after some time)

        public float timeToResetToPreviousPose; //After this time, if its smaller than timeToResetFullPattern, patternStep goes -1 and again...
        public float timeToResetFullPattern; //After this time pattern pos resets
    }

    static public void GiveDamage(GameObject target, int damage)
    {
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
    }  

    static public void GiveRandomForce(GameObject target, float force)
    {
        Vector3 torque;

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
