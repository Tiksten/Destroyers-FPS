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
    }

    //public class DroppedWeapon
    //{
    //    public string weaponName;
    //    public int ammoInMag;
    //}

    static public void GiveDamage(GameObject target, float damage)
    {   
        var _damage = Mathf.RoundToInt(damage);

        target.GetComponent<Destructible>()?.TakeDamage(_damage);
        target.GetComponent<PlayerHealth>()?.TakeDamage(_damage);
        target.GetComponent<SC_NPCEnemy>()?.ApplyDamage(_damage);
        target.GetComponent<ButtonScript>()?.ClickButton();
    }

    static public void GiveRandomForce(GameObject target, float force)
    {
        target.GetComponent<Rigidbody>()?.AddForce(force * Random.onUnitSphere, ForceMode.Impulse);
    }

    static public IEnumerator Deactivate(GameObject gameObject, float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    static public RaycastHit Shot(Vector3 posFrom, Vector3 posTo, float shotDistance, float damage = 10, float impactForce = 1, params ParticleSystem[] shotEffects)
    {
        var shotDir = posTo - posFrom;

        RaycastHit hit;

        if (Physics.Raycast(posFrom, shotDir, out hit, shotDistance))
        {
            //Damage
            GiveDamage(hit.collider.gameObject, damage);


            //Impact Effects
            var chosenImpactEffect = mem.ImpactEffectChose(hit.collider);

            foreach(GameObject i in chosenImpactEffect)
                Destroy(Instantiate(i, hit.point, Quaternion.LookRotation(hit.normal)), Random.Range(10f, 15f));


            //ShotEffects
            foreach (ParticleSystem i in shotEffects)
                i.Play();


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
                hit.rigidbody.AddForceAtPosition((hit.point - posFrom) * impactForce, hit.point, ForceMode.Impulse);
        }

        return hit;
    }

    static public RaycastHit ShotForward(Transform transformFrom, float shotDistance, float damage = 10, float impactForce = 1, params ParticleSystem[] shotEffects)
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


            //ShotEffects
            foreach (ParticleSystem i in shotEffects)
                i.Play();


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
                hit.rigidbody.AddForceAtPosition((hit.point - posFrom) * impactForce, hit.point, ForceMode.Impulse);
        }

        return hit;
    }

    static public void DirectionalShotEffect(Vector3 posFrom, Vector3 posTo, ParticleSystem[] directionalShotEffects, float minDistance, Vector3 defaultRot)
    {
        if(Vector3.Distance(posFrom, posTo) >= minDistance)
        {
            foreach (ParticleSystem i in directionalShotEffects)
            {
                if (posTo != new Vector3(0, 0, 0))
                {
                    i.gameObject.transform.LookAt(posTo);
                    i.Play();
                }
                else
                {
                    i.gameObject.transform.localRotation = Quaternion.Euler(defaultRot);
                    //Debug.Log((i.gameObject.transform.localRotation).eulerAngles);
                    i.Play();
                }
            }
        }
    }
}