using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

public class Weapon : MonoBehaviour
{
    #region REFERENCES 
    [Separator("REFERENCES")]

    public WeaponSwitching weaponSwitching;

    public PlayerInventory inventory;

    public Animator weaponAnimator;

    public Animator HandAnimator_R;//For animation events

    public Animator HandAnimator_L;//For animation events

    public AudioSource weaponAudioSource;

    public Transform fpsCam;
    
    public Text ammoText;

    public Memory memory;
    [HideInInspector]
    public MouseLook ml;
    [Space(20)]
    #endregion


    #region STATS
    [Separator("STATS")]
    public float damage;
    [Space(20)]
    #endregion


    #region AMMO
    [Separator("AMMO")]
    public string ammoType;

    public int maxAmmoInMag;
    public int currentAmmoInMag
    {
        get => _currentAmmoInMag;
        set
        {
            _currentAmmoInMag = (value <= maxAmmoInMag) ? value : maxAmmoInMag;
        }
    }

    [HideInInspector]
    public int _currentAmmoInMag;
    [Space(20)]
    #endregion


    #region ANIMATIONS
    [Separator("ANIMATIONS")]
    public AnimationClip randomWeaponInspectionStart; //Need to be 1 frame anim (only for events)
    public AnimationClip randomWeaponShotStart;
    public AnimationClip randomWeaponReloadStart;
    public AnimationClip weaponDraw; //Normal anim
    [Space(20)]
    #endregion


    #region EFFECTS
    [Separator("EFFECTS")]
    public ParticleSystem[] barrelSmokes;
    public ParticleSystem[] muzzleFlashes;
    public ParticleSystem[] directionalShotEffects;

    public Transform cartrigeEjector;

    public GameObject cartrigePrefab;

    public float cartrigeForce = 5;
    public float impactForce = 5;
    [Space(20)]
    #endregion


    #region SPRAY PATTERN
    [Separator("SPRAY PATTERN")]

    public bool hasSprayPattern;

    [ConditionalField(nameof(hasSprayPattern))] public string sprayPatternName;
    [ConditionalField(nameof(hasSprayPattern))] public float timeToResetOneStep;
    [ConditionalField(nameof(hasSprayPattern))] public float timeToResetFullRecoil;

    [HideInInspector]
    public int currentSprayStep = 0;
    [HideInInspector]
    private float recoilMultiplier = 1;
    [HideInInspector]
    private float shotRandomnessMultiplier = 1;
    [HideInInspector]
    public Helper.SprayPattern sprayPattern;

    [Space(20)]
    #endregion


    #region PROJECTILE
    [Separator("PROJECTILE")]

    public bool hasProjectile;

    [ConditionalField(nameof(hasProjectile))] public GameObject projectile;

    [ConditionalField(nameof(hasProjectile))] public Transform firePoint;

    [ConditionalField(nameof(hasProjectile))] public float ejectForce;

    [Space(20)]
    #endregion


    #region ALTPROJECTILE
    [Separator("ALTPROJECTILE")]

    public bool hasAltProjectile;

    [ConditionalField(nameof(hasAltProjectile))] public GameObject altProjectile;

    [ConditionalField(nameof(hasAltProjectile))] public Transform altFirePoint;

    [ConditionalField(nameof(hasAltProjectile))] public float altEjectForce;

    [Space(20)]
    #endregion


    #region SOUNDS
    [Separator("SOUNDS")]
    public AudioClip[] sounds;
    [Space(20)]
    #endregion


    #region FLAGS
    [HideInInspector] public bool canAct;
    [HideInInspector] public bool gunReadyToShoot;
    #endregion


    public void Start()
    {
        ml = fpsCam.GetComponent<MouseLook>();
    }




    //Main
    public void Fire()
    {
        currentAmmoInMag--;
        var to = Helper.ShotForward(fpsCam, damage, 1000).point;
        PlayShotTrial(to);
    }

    public void FireAnObject()
    {
        var _obj = Instantiate(projectile, firePoint.position, firePoint.rotation);
        Destroy(_obj, 30);

        _obj.GetComponent<Rigidbody>().AddForce(firePoint.forward * ejectForce, ForceMode.Impulse);
    }

    public void FireShotgun()//WorkInProgress
    {
        currentAmmoInMag--;
        var to = Helper.ShotgunShotForward(fpsCam, damage, 10, 1000, 1);
        foreach(RaycastHit i in to)
        {
            PlayShotTrial(i.point);
        }
    }

    public void FireAlt()
    {
        var _obj = Instantiate(altProjectile, altFirePoint.position, altFirePoint.rotation);
        Destroy(_obj, 30);

        _obj.GetComponent<Rigidbody>().AddForce(altFirePoint.forward * altEjectForce, ForceMode.Impulse);
    }

    public void ReloadMag()
    {
        currentAmmoInMag = inventory.FillMagFromInventory(currentAmmoInMag, maxAmmoInMag, ammoType);
        StopAllCoroutines();
        ResetRecoil();
    }

    public void Aim()
    {
        //Play anim, change recoil
    }//WorkInProgress

    public void Scope()
    {
        //Change UI to scope texture, change cam fov, change recoil
    }//WorkInProgress



    //Effects
    public void EjectCartrige()
    {
        var cartrige = Instantiate(cartrigePrefab, cartrigeEjector.position, cartrigeEjector.rotation);
        Destroy(cartrige, Random.Range(7, 10));


        var rb = cartrige.GetComponent<Rigidbody>();
        rb.AddForce(cartrigeEjector.right * (cartrigeForce + Random.Range(0, 0.3f)), ForceMode.Impulse);
        rb.AddTorque(Random.Range(-15, 15), Random.Range(-15, 15), 0, ForceMode.Impulse);
    }

    public void PlayBarrelSmokes()
    {
        foreach (ParticleSystem i in barrelSmokes)
            i.Play();
    }

    public void PlayMuzzleFlashes()
    {
        foreach (ParticleSystem i in muzzleFlashes)
            i.Play();
    }

    public void PlayShotTrial(Vector3 to)
    {
        Helper.DirectionalShotEffect(to, directionalShotEffects, 5);
    }



    //Misc
    public void ReloadAmmoText()
    {
        ammoText.text = currentAmmoInMag + "/" + inventory.GetAmmoCountOfType(ammoType);
    }



    //Recoil
    public void StartRecoilResetter()
    {
        StopAllCoroutines();
        StartCoroutine(RecoilResetter());
    }

    public IEnumerator RecoilResetter()
    {
        float totalWaitTime = timeToResetFullRecoil;

        while(totalWaitTime > timeToResetOneStep)
        {
            yield return new WaitForSecondsRealtime(timeToResetOneStep);
            if(currentSprayStep > 0)
                currentSprayStep--;
            totalWaitTime -= timeToResetOneStep;
        }

        yield return new WaitForSecondsRealtime(totalWaitTime);
        currentSprayStep = 0;
    }

    public void Recoil(float horizontal, float vertical)
    {
        ml.AddRotation(horizontal * recoilMultiplier, vertical * recoilMultiplier, 0);
    }

    public void ResetRecoil()
    {
        currentSprayStep = 0;
        StopAllCoroutines();
    }

    public void MoveNextSprayStep()
    {
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
        Debug.Log(currentSprayStep);
        Debug.Log("    ");
    }
    
    public void SetSprayPattern(string name)
    {
        foreach(Helper.SprayPattern i in memory.sprayPatterns)
        {
            if(i.weaponPatternName == name)
            {
                sprayPattern = i;
                return;
            }
        }

        Debug.Log("ERROR NO SPRAY PATTERN");
    }



    //Methods for animator
    public void PlaySound(int soundNumber)
    {
        weaponAudioSource.PlayOneShot(sounds[soundNumber]);
    }

    public void SetVolume(int volume)
    {
        weaponAudioSource.volume = volume / 100;
    }

    public void ResetRandomNumberInAnimator(int maxNum = 1)
    {
        weaponAnimator.SetInteger("RandomNumber", Random.Range(0, maxNum));
    }

    public void SetHandPose(int isRightHand, string pose)
    {
        var _isRightHand = System.Convert.ToBoolean(isRightHand);

        if (_isRightHand)
            HandAnimator_R.Play(pose);
        else
            HandAnimator_L.Play(pose);
    }

    public void SetCanAct(int _canAct)
    {
        weaponAnimator.SetBool("CanAct", System.Convert.ToBoolean(_canAct));
    }
    
    public void SetGunReadyToShoot(int _gunReadyToShoot)
    {
        weaponAnimator.SetBool("GunReadyToShoot", System.Convert.ToBoolean(_gunReadyToShoot));
    }

    public IEnumerator HandPoseTransitionToFree(int isRightHand)
    {
        var _isRightHand = System.Convert.ToBoolean(isRightHand);

        if (_isRightHand)
        {
            HandAnimator_R.SetBool("Transition", true);
            yield return new WaitForSeconds(0.05f);
            HandAnimator_R.SetBool("Transition", false);
        }

        else
        {
            HandAnimator_L.SetBool("Transition", true);
            yield return new WaitForSeconds(0.05f);
            HandAnimator_L.SetBool("Transition", false);
        }
    }
}
