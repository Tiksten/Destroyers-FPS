using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniversalWeaponScript : MonoBehaviour
{


    [Header("GENERAL")]
    public Camera fpsCam;
    public Animator animator;
    public bool pistolShootingType;
    public Memory memory;
    public string weaponPatternName;
    [Space(10)]

    [Header("ANIMATIONS")]
    //public AnimationClip weaponDraw; Not working
    public AnimationClip[] tagShootVariations;
    public AnimationClip[] tagReloadVariations;
    public AnimationClip[] tagIdleVariations;
    [Space(10)]

    [Header("STATS")]
    public int range;
    public int damage;
    public float impactForce;
    //public float firespeed = 0.1f; Firespeed based on shot animation time
    [Space(10)]

    [Header("SOUNDS")]
    public AudioSource weaponAudioSource;
    public AudioClip[] shots;
    public SoundPlan[] reloadSoundsPlan;
    [Space(10)]

    [Header("PARTICLES")]
    public ParticleSystem[] muzzleFlashes;
    [Space(10)]
    public ParticleSystem[] impactEffects;
    [Space(10)]
    public ParticleSystem[] barrelSmokes;
    [Space(10)]

    [Header("AMMO")]
    public int maxAmmo;
    public int startAmmo;
    public Text ammoText;
    [Space(10)]

    [Header("CARTRIGES")]
    public Transform cartrigeEjector;
    public GameObject cartrigePrefab;
    public float cartrigeForce = 5f;
    [Space(10)]

    [Header("SPRAY PATTERN")]
    public Helper.SprayPattern sprayPattern;



    [HideInInspector]
    public Transform shootingPoint;

    [HideInInspector]
    public int currentSprayStep;

    [HideInInspector]
    public bool weaponPutAway;

    [HideInInspector]
    public int currentAmmo;

    [System.Serializable]
    public class SoundPlan
    {
        public float timeToWait;
        public AudioClip sound;
    }

    [System.Serializable]
    public class BulletHoleType
    {
        public Material material;
        public GameObject[] bulletHolePrefab;
    }


    void Start()
    {
        currentAmmo = startAmmo;
        foreach (AnimationClip i in tagShootVariations)
        {
            i.wrapMode = WrapMode.Once;
        }
        foreach (AnimationClip i in tagReloadVariations)
        {
            i.wrapMode = WrapMode.Once;
        }
        foreach (AnimationClip i in tagIdleVariations)
        {
            i.wrapMode = WrapMode.Once;
        }
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
        {
            if (!pistolShootingType)
            {
                if (Input.GetButton("Fire1"))
                {
                    if (currentAmmo != 0)
                    {
                        StartCoroutine(Shot());

                        if (barrelSmokes.Length != 0)
                        {
                            foreach (ParticleSystem ps in barrelSmokes)
                                ps.Stop();
                        }

                        currentAmmo--;

                        ammoText.text = currentAmmo + "/" + maxAmmo;
                    }

                    else
                        StartCoroutine(Reload());
                }

                else if (Input.GetButtonUp("Fire1"))
                {
                    if (barrelSmokes.Length != 0)
                    {
                        foreach (ParticleSystem ps in barrelSmokes)
                            ps.Play();
                    }
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    if (currentAmmo != 0)
                    {
                        StartCoroutine(Shot());

                        if (barrelSmokes.Length != 0)
                        {
                            foreach (ParticleSystem ps in barrelSmokes)
                                ps.Play();
                        }

                        currentAmmo--;

                        ammoText.text = currentAmmo + "/" + maxAmmo;
                    }

                    else
                        StartCoroutine(Reload());
                }
            }

            if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
                StartCoroutine(Reload());
        }
    }


    private IEnumerator Shot()
    {
        var animation = tagShootVariations[Random.Range(0, tagShootVariations.Length)];
        animator.Play(animation.name);

        if (muzzleFlashes.Length != 0)
        {
            foreach (ParticleSystem ps in muzzleFlashes)
                ps.Play();
        }

        var cartrige = Instantiate(cartrigePrefab, cartrigeEjector.position, cartrigeEjector.rotation);
        Destroy(cartrige, 2f);


        var rb = cartrige.GetComponent<Rigidbody>();
        rb.AddForce(cartrigeEjector.right * (cartrigeForce + Random.Range(0, 0.2f)), ForceMode.Impulse);
        rb.AddTorque(Random.Range(-15, 15), Random.Range(-15, 15), 0, ForceMode.Impulse);

        weaponAudioSource.pitch = Random.Range(1.3f, 1.5f);
        weaponAudioSource.clip = shots[Random.Range(0, shots.Length)];
        weaponAudioSource.Play();



        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Helper.GiveDamage(hit.collider.gameObject, 40);

            //Impact Effects
            if (impactEffects.Length != 0)
            {
                foreach (ParticleSystem ps in impactEffects)
                    Instantiate(ps, hit.point, Quaternion.LookRotation(hit.normal));
            }


            //Bullet Hole
            var chosenBulletHole = memory.BulletHoleChose(hit.collider);

            GameObject bulletHole = Instantiate(chosenBulletHole, hit.point, Quaternion.LookRotation(hit.normal));

            bulletHole.transform.parent = hit.transform;



            //Impact
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }

        
        yield return new WaitForSeconds(animation.length);

        NextIdle();
    }


    private IEnumerator Reload()
    {
        ammoText.text = "Reload/" + maxAmmo;

        var totalWaitTime = 0f;
        var animation = tagReloadVariations[Random.Range(0, tagReloadVariations.Length)];


        //Playing random reloading animation
        animator.Play(animation.name);

        foreach (SoundPlan sp in reloadSoundsPlan)
        {
            yield return new WaitForSeconds(sp.timeToWait);
            totalWaitTime = totalWaitTime + sp.timeToWait;

            weaponAudioSource.clip = sp.sound;
            weaponAudioSource.Play();
        }


        //Animation time - sound plan time
        totalWaitTime = animation.length - totalWaitTime;

        //If sound plan shorter then animation
        if (totalWaitTime > 0)
            yield return new WaitForSeconds(totalWaitTime);

        currentAmmo = maxAmmo;

        ammoText.text = currentAmmo + "/" + maxAmmo;

        NextIdle();
    }

    private void NextIdle()
    {
        animator.Play(tagIdleVariations[Random.Range(0, tagIdleVariations.Length)].name);
    }

    private void MoveNextSprayStep()
    {
        //Moves shootingPoint to next step
        if(sprayPattern.weaponSprayPattern.Length > currentSprayStep + 1)
            shootingPoint.transform.localPosition = Vector3.Lerp(
                sprayPattern.weaponSprayPattern[currentSprayStep],
                sprayPattern.weaponSprayPattern[currentSprayStep + 1],
                sprayPattern.shootingSpeed);
        else
            shootingPoint.transform.localPosition = Vector3.Lerp(
                sprayPattern.weaponCyclePattern[currentSprayStep - sprayPattern.weaponSprayPattern.Length],
                sprayPattern.weaponCyclePattern[currentSprayStep - sprayPattern.weaponSprayPattern.Length + 1],
                sprayPattern.shootingSpeed);
    }

    private void ResetSpray()
    {
        shootingPoint.transform.localPosition = Vector3.Lerp(
            shootingPoint.transform.localPosition,
            sprayPattern.weaponSprayPattern[0],
            sprayPattern.shootingSpeed);
    }
}
