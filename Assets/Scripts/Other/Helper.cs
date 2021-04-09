using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    private static Memory mem = GameObject.FindGameObjectWithTag("Memory").GetComponent<Memory>();

    public enum WeaponType
    {
        Pistol,
        Rifle,
        Rifle_Burst,
        Shotgun,
        Singleshot
    }

    public enum TargetType
    {
        HideFromDanger = 11,
        Player = 10,
        Shot = 9,
        DeadBody = 8,
        Sound = 7,
        LookingForPlayer = 6,
        Patrolling = 5,
        Staying = 4,
        Nothing = 3
    }

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

    [System.Serializable]
    public class DroppedWeapon
    {
        public string weaponName;
        public int ammoInMag;
    }

    static public void GiveDamage(GameObject target, int damage)
    {
        target.GetComponent<Destructible>()?.TakeDamage(damage);
        target.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        target.GetComponent<SC_NPCEnemy>()?.ApplyDamage(damage);
        target.GetComponent<ButtonScript>()?.ClickButton();
    }

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

    static public AudioClip ChooseRandomAudioClip(AudioClip[] audioClips)
    {
        return audioClips[Random.Range(0, audioClips.Length)];
    }

    static public Animation ChooseRandomAnimation(Animation[] animations)
    {
        return animations[Random.Range(0, animations.Length)];
    }
}