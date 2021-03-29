using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    private static Memory mem = GameObject.FindGameObjectWithTag("Memory").GetComponent<Memory>();

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

    static public void GiveDamage(GameObject target, float damage)
    {
        Destructible target1 = target.GetComponent<Destructible>();
        PlayerHealth target2 = target.GetComponent<PlayerHealth>();
        SC_NPCEnemy target3 = target.GetComponent<SC_NPCEnemy>();
        ButtonScript target4 = target.GetComponent<ButtonScript>();

        var _damage = Mathf.RoundToInt(damage);

        if (target1 != null)
            target1.TakeDamage(_damage);

        else if (target2 != null)
            target2.TakeDamage(_damage);

        else if (target3 != null)
            target3.ApplyDamage(_damage);

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

    static public IEnumerator Deactivate(GameObject gameObject, float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    static public Collider Shot(Vector3 posFrom, Vector3 posTo, float damage, float shotDistance)
    {
        var shotDir = posTo - posFrom;

        RaycastHit hit;

        if (Physics.Raycast(posFrom, shotDir, out hit, shotDistance))
        {
            //Damage
            GiveDamage(hit.collider.gameObject, damage);


            //Impact Effects
            var chosenImpactEffect = mem.ImpactEffectChose(hit.collider);

            foreach (GameObject i in chosenImpactEffect)
                Destroy(Instantiate(i, hit.point, Quaternion.LookRotation(hit.normal)), Random.Range(10f, 15f));


            //Bullet Hole
            if (hit.collider.gameObject.tag != "Player")
            {
                var chosenBulletHole = mem.BulletHoleChose(hit.collider);

                GameObject bulletHole = Instantiate(chosenBulletHole, hit.point, Quaternion.LookRotation(hit.normal));

                bulletHole.transform.parent = hit.transform;

                Destroy(bulletHole, Random.Range(45f, 60f));
            }


            //Impact
            if (hit.rigidbody != null)
                hit.rigidbody.AddForceAtPosition((hit.point - posFrom) * (damage / 4), hit.point, ForceMode.Impulse);

            return hit.collider;
        }

        else return null;
    }

    static public Collider Shot(Vector3 posFrom, Vector3 posTo, float damage, float shotDistance, GameObject[] shotEffects)
    {
        var shotDir = posTo - posFrom;
        foreach(GameObject i in shotEffects)
            Destroy(Instantiate(i, posFrom, Quaternion.Euler(shotDir)), 2);

        RaycastHit hit;

        if (Physics.Raycast(posFrom, shotDir, out hit, shotDistance))
        {
            //Damage
            GiveDamage(hit.collider.gameObject, damage);


            //Impact Effects
            var chosenImpactEffect = mem.ImpactEffectChose(hit.collider);

            foreach(GameObject i in chosenImpactEffect)
                Destroy(Instantiate(i, hit.point, Quaternion.LookRotation(hit.normal)), Random.Range(10f, 15f));


            //Bullet Hole
            if (hit.collider.gameObject.tag != "Player")
            {
                var chosenBulletHole = mem.BulletHoleChose(hit.collider);

                GameObject bulletHole = Instantiate(chosenBulletHole, hit.point, Quaternion.LookRotation(hit.normal));

                bulletHole.transform.parent = hit.transform;

                Destroy(bulletHole, Random.Range(45f, 60f));
            }


            //Impact
            if (hit.rigidbody != null)
                hit.rigidbody.AddForceAtPosition((hit.point - posFrom) * (damage / 4), hit.point, ForceMode.Impulse);

            return hit.collider;
        }

        else return null;
    }

    static public Collider ShotForward(Transform transformFrom, float damage, float shotDistance)
    {
        var posFrom = transformFrom.position;
        var shotDir = transformFrom.forward;

        RaycastHit hit;

        if (Physics.Raycast(posFrom, shotDir, out hit, shotDistance))
        {
            //Damage
            GiveDamage(hit.collider.gameObject, damage);


            //Impact Effects
            var chosenImpactEffect = mem.ImpactEffectChose(hit.collider);

            foreach (GameObject i in chosenImpactEffect)
                Destroy(Instantiate(i, hit.point, Quaternion.LookRotation(hit.normal)), Random.Range(10f, 15f));


            //Bullet Hole
            if (hit.collider.gameObject.tag != "Player")
            {
                var chosenBulletHole = mem.BulletHoleChose(hit.collider);

                GameObject bulletHole = Instantiate(chosenBulletHole, hit.point, Quaternion.LookRotation(hit.normal));

                bulletHole.transform.parent = hit.transform;

                Destroy(bulletHole, Random.Range(45f, 60f));
            }


            //Impact
            if (hit.rigidbody != null)
                hit.rigidbody.AddForceAtPosition((hit.point - posFrom) * (damage / 4), hit.point, ForceMode.Impulse);

            return hit.collider;
        }

        else return null;
    }

    static public Collider ShotForward(Transform transformFrom, float damage, float shotDistance, GameObject[] shotEffects)
    {
        var posFrom = transformFrom.position;
        var shotDir = transformFrom.forward;
        var fwd = transformFrom.rotation;

        foreach (GameObject i in shotEffects)
            Destroy(Instantiate(i, posFrom, fwd), 2);

        RaycastHit hit;

        if (Physics.Raycast(posFrom, shotDir, out hit, shotDistance))
        {
            //Damage
            GiveDamage(hit.collider.gameObject, damage);


            //Impact Effects
            var chosenImpactEffect = mem.ImpactEffectChose(hit.collider);

            foreach (GameObject i in chosenImpactEffect)
                Destroy(Instantiate(i, hit.point, Quaternion.LookRotation(hit.normal)), Random.Range(10f, 15f));


            //Bullet Hole
            if (hit.collider.gameObject.tag != "Player")
            {
                var chosenBulletHole = mem.BulletHoleChose(hit.collider);

                GameObject bulletHole = Instantiate(chosenBulletHole, hit.point, Quaternion.LookRotation(hit.normal));

                bulletHole.transform.parent = hit.transform;

                Destroy(bulletHole, Random.Range(45f, 60f));
            }


            //Impact
            if (hit.rigidbody != null)
                hit.rigidbody.AddForceAtPosition((hit.point - posFrom) * (damage / 4), hit.point, ForceMode.Impulse);

            return hit.collider;
        }

        else return null;
    }
}
