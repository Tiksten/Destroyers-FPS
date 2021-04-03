using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniversalWeaponScript : MonoBehaviour
{
    [Header("GENERAL")]

    public Camera fpsCam;

    public Animator animator;

    public Memory memory;

    public bool pistolShootingType;
    [Space(20)]

    [Header("ANIMATIONS")]
    public AnimationClip[] tagShootVariations;
    public AnimationClip[] tagReloadVariations;
    public AnimationClip[] tagIdleVariations;
    [Space(20)]

    [Header("STATS")]
    public int range;
    public int damage;
    public float impactForce;
    public float firespeed = 0.1f;
    public float patternSizeMultiplier = 1f;
    [Space(20)]

    [Header("SOUNDS")]
    public AudioSource weaponAudioSource;
    public AudioClip[] shots;
    public SoundPlan[] reloadSoundsPlan;
    [Space(20)]

    [Header("PARTICLES")]
    public ParticleSystem[] shotEffects;
    [Space(10)]
    public ParticleSystem[] directionalShotEffects;
    public Vector3 defaultRot;
    [Space(10)]
    public ParticleSystem[] barrelSmokes;
    [Space(10)]
    public Transform shootingPoint;
    [Space(20)]

    [Header("AMMO")]
    public int maxAmmo;
    private int ammo;
    public int Ammo
    {
        set
        {
            ammo = (value <= maxAmmo) ? value : maxAmmo;
            ammoText.text = ammoInMag + "/" + ammo;
        }
        get => ammo;
    }

    public int maxAmmoInMag;
    private int ammoInMag;
    public int AmmoInMag
    {
        set => ammoInMag = (value <= maxAmmoInMag) ? value : maxAmmoInMag;
        get => ammoInMag;
    }

    public Text ammoText;

    public AudioClip noAmmo;
    [Space(20)]

    [Header("CARTRIGES")]
    public Transform cartrigeEjector;
    public GameObject cartrigePrefab;
    public float cartrigeForce = 5f;
    [Space(20)]

    [Header("SPRAY PATTERN")]
    public string sprayPatternName;
    public float timeToResetOneStep;
    public float timeToResetFullRecoil;



    [HideInInspector]
    public int currentSprayStep;

    [HideInInspector]
    public bool weaponPutAway;

    public float recoilMultiplier = 1f;

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

    [HideInInspector]
    Helper.SprayPattern sprayPattern;

    void Start()
    {
        ammoText.text = ammoInMag + "/" + ammo;
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

        foreach (Helper.SprayPattern i in memory.sprayPatterns)
        {
            if (i.weaponPatternName == sprayPatternName)
            {
                sprayPattern = i;
                break;
            }
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
                    if (ammoInMag != 0)
                    {
                        StartCoroutine(Shot());

                        if (barrelSmokes.Length != 0)
                        {
                            foreach (ParticleSystem ps in barrelSmokes)
                                ps.Stop();
                        }

                        ammoInMag--;

                        ammoText.text = ammoInMag + "/" + ammo;
                    }

                    else if (ammo > 0)
                        StartCoroutine(Reload());

                    else
                    {
                        weaponAudioSource.clip = noAmmo;
                        weaponAudioSource.pitch = 1;
                        weaponAudioSource.Play();
                    }
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
                    if (ammoInMag != 0)
                    {
                        StartCoroutine(Shot());

                        if (barrelSmokes.Length != 0)
                        {
                            foreach (ParticleSystem ps in barrelSmokes)
                                ps.Play();
                        }

                        ammoInMag--;

                        ammoText.text = ammoInMag + "/" + ammo;
                    }

                    else if (ammo > 0)
                        StartCoroutine(Reload());

                    else
                    {
                        weaponAudioSource.clip = noAmmo;
                        weaponAudioSource.pitch = 1;
                        weaponAudioSource.Play();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
                if(ammoInMag < maxAmmoInMag && ammo > 0)
                    StartCoroutine(Reload());
        }
    }


    private IEnumerator Shot()
    {
        var hit = Helper.ShotForward(fpsCam.transform, 1000, damage, impactForce, shotEffects);

        foreach (ParticleSystem i in shotEffects)
            i.Play();

        Helper.DirectionalShotEffect(shootingPoint.position, hit.point, directionalShotEffects, 5, defaultRot);

        var animation = tagShootVariations[Random.Range(0, tagShootVariations.Length)];
        animator.Play(animation.name);

        var cartrige = Instantiate(cartrigePrefab, cartrigeEjector.position, cartrigeEjector.rotation);
        Destroy(cartrige, 2f);


        var rb = cartrige.GetComponent<Rigidbody>();
        rb.AddForce(cartrigeEjector.right * (cartrigeForce + Random.Range(0, 0.2f)), ForceMode.Impulse);
        rb.AddTorque(Random.Range(-15, 15), Random.Range(-15, 15), 0, ForceMode.Impulse);

        weaponAudioSource.pitch = Random.Range(1.3f, 1.5f);
        weaponAudioSource.clip = shots[Random.Range(0, shots.Length)];
        weaponAudioSource.Play();

        MoveNextSprayStep();
        
        yield return new WaitForSeconds(firespeed);

        NextIdle();

        StartCoroutine(SprayStabilize());
    }


    private IEnumerator Reload()
    {
        ammoText.text = ammoInMag + "/" + ammo;

        var totalWaitTime = 0f;
        var animation = tagReloadVariations[Random.Range(0, tagReloadVariations.Length)];


        //Playing random reloading animation
        animator.Play(animation.name);

        weaponAudioSource.pitch = 1;

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
        {
            yield return new WaitForSeconds(totalWaitTime);
            ammo -= (maxAmmoInMag - ammoInMag);
            ammoInMag = maxAmmoInMag;
            if (ammo < 0)
            {
                ammoInMag += ammo;
                ammo = 0;
            }
        }

        else
        {
            ammo -= maxAmmoInMag - ammoInMag;
            ammoInMag = maxAmmoInMag;
        }

        ammoText.text = ammoInMag + "/" + ammo;

        NextIdle();

        currentSprayStep = 0;
    }

    private void NextIdle()
    {
        animator.Play(tagIdleVariations[Random.Range(0, tagIdleVariations.Length)].name);
    }

    private void MoveNextSprayStep()
    {
        var ml = fpsCam.GetComponent<MouseLook>();

        //Moves shootingPoint to next step
        if (sprayPattern.weaponSprayPattern.Length >= currentSprayStep + 1)
        {
            var step = sprayPattern.weaponSprayPattern[currentSprayStep];

            Recoil(step.x, step.y);
        }
        else
        {
            if (currentSprayStep == sprayPattern.weaponCyclePattern.Length + sprayPattern.weaponSprayPattern.Length)
                currentSprayStep = sprayPattern.weaponSprayPattern.Length;

            var step = sprayPattern.weaponCyclePattern[currentSprayStep - sprayPattern.weaponSprayPattern.Length];

            Recoil(step.x, step.y);
        }

        currentSprayStep++;
    }

    private void Recoil(float horizontal, float vertical)
    {
        var ml = fpsCam.GetComponent<MouseLook>();
        ml.AddRotation(horizontal * recoilMultiplier, vertical * recoilMultiplier, 0);
    }

    private IEnumerator SprayStabilize()
    {
        var currentStep = currentSprayStep;
        var totalTime = timeToResetFullRecoil;
        var totalMinus = 0;
        while (currentSprayStep + totalMinus == currentStep && currentStep != -1)
        {
            yield return new WaitForSeconds(timeToResetOneStep);
            if (currentSprayStep + totalMinus == currentStep)
            {
                if (currentSprayStep != 0)
                {
                    currentSprayStep--;
                    totalMinus++;
                }
                else
                    currentStep = -1;
            }
            else
                currentStep = -1;

            totalTime -= timeToResetOneStep;
            if(totalTime < timeToResetOneStep)
            {
                yield return new WaitForSeconds(totalTime);
                if (currentSprayStep + totalMinus == currentStep)
                {
                    currentSprayStep = 0;
                    currentStep = -1;
                }
            }
        }
    }
}