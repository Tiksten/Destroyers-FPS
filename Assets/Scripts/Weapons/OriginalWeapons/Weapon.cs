using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    #region REFERENCES 
    [Header("REFERENCES")]

    public WeaponSwitching weaponSwitching;

    public PlayerInventory inventory;

    public Animator weaponAnimator;

    public Animator HandAnimator_R;//For animation events

    public Animator HandAnimator_L;//For animation events

    public AudioSource weaponAudioSource;

    public Transform fpsCam;
    [Space(20)]
    #endregion


    #region STATS
    [Header("STATS")]
    public float damage;
    [Space(20)]
    #endregion


    #region AMMO
    [Header("AMMO")]
    public string ammoType;
    public Text ammoText;

    public int maxAmmoInMag;
    public int currentAmmoInMag
    {
        get => _currentAmmoInMag;
        set
        {
            currentAmmoInMag = (value <= maxAmmoInMag) ? value : maxAmmoInMag;
            ReloadAmmoText();
        }
    }
    [HideInInspector]
    public int _currentAmmoInMag;
    [Space(20)]
    #endregion


    #region ANIMATIONS
    [Header("ANIMATIONS")]
    public AnimationClip randomWeaponInspectionStart; //Need to be 1 frame anim (only for events)
    public AnimationClip randomWeaponShotStart;
    public AnimationClip randomWeaponIdleStart;
    public AnimationClip randomWeaponReloadStart;
    public AnimationClip weaponDraw; //Normal anim
    [Space(20)]
    #endregion


    #region EFFECTS
    [Header("EFFECTS")]
    public ParticleSystem[] barrelSmokes;
    public ParticleSystem[] muzzleFlashes;

    public Transform cartrigeEjector;

    public GameObject cartrigePrefab;

    public float cartrigeForce = 5;
    public float impactForce = 5;
    [Space(20)]
    #endregion


    #region SPRAY PATTERN
    [Header("SPRAY PATTERN")]
    public string sprayPatternName;
    public float timeToResetOneStep;
    public float timeToResetFullRecoil;

    [HideInInspector]
    public int currentSprayStep;
    [HideInInspector]
    public float recoilRandomnessMultiplier;
    #endregion


    #region PROJECTILE
    [Header("PROJECTILE")]
    public GameObject projectile;

    public Transform firePoint;

    public float ejectForce;
    #endregion


    #region ALTPROJECTILE
    [Header("PROJECTILE")]
    public GameObject altProjectile;

    public Transform altFirePoint;

    public float altEjectForce;
    #endregion


    #region FLAGS
    public bool canAct;
    public bool gunReadyToShoot;
    #endregion





    //Main
    public void Fire()
    {
        Helper.ShotForward(fpsCam, damage, 1000);
    }

    public void FireAnObject()
    {
        var _obj = Instantiate(projectile, firePoint.position, firePoint.rotation);
        Destroy(_obj, 30);

        _obj.GetComponent<Rigidbody>().AddForce(firePoint.forward * ejectForce, ForceMode.Impulse);
    }

    public void FireShotgun()//WorkInProgress
    {

    }

    public void FireAlt()
    {
        var _obj = Instantiate(altProjectile, altFirePoint.position, altFirePoint.rotation);
        Destroy(_obj, 30);

        _obj.GetComponent<Rigidbody>().AddForce(altFirePoint.forward * altEjectForce, ForceMode.Impulse);
    }

    public void Reload()
    {
        currentAmmoInMag = inventory.FillMagFromInventory(currentAmmoInMag, maxAmmoInMag, ammoType);
        StopAllCoroutines();
    }

    public void Aim()
    {
        //Play anim, change recoil
    }//WorkInProgress

    public void Scope()
    {
        //Change UI to scope texture, change cam fov, change recoil
    }//WorkInProgress

    private void Inspect()
    {
        weaponAnimator.Play(randomWeaponInspectionStart.name);
    }



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



    //Misc
    public void ReloadAmmoText()
    {
        ammoText.text = currentAmmoInMag + "/" + inventory.GetAmmoCountOfType(ammoType);
    }



    //Recoil
    public void StartRecoilResetter()
    {
        StopCoroutine(RecoilResetter());
        StartCoroutine(RecoilResetter());
    }

    public IEnumerator RecoilResetter()
    {
        float totalWaitTime = timeToResetFullRecoil;

        while(totalWaitTime > timeToResetOneStep)
        {
            yield return new WaitForSecondsRealtime(timeToResetOneStep);
            currentSprayStep--;
            totalWaitTime -= timeToResetOneStep;
        }

        yield return new WaitForSecondsRealtime(totalWaitTime);
        currentSprayStep = 0;
    }

    public void Recoil(float horizontal, float vertical)
    {
        var ml = fpsCam.GetComponent<MouseLook>();
        ml.AddRotation(horizontal * recoilRandomnessMultiplier, vertical * recoilRandomnessMultiplier, 0);
    }



    //Methods for animator
    public void PlaySound(AudioClip sound, float volume)
    {
        weaponAudioSource.PlayOneShot(sound, volume);
    }

    public void ResetRandomNumberInAnimator(int maxNum = 1)
    {
        weaponAnimator.SetInteger("RandomNumber", Random.Range(0, maxNum));
    }

    public void SetHandPose(bool rightHand, string pose)
    {
        if (rightHand)
            HandAnimator_R.Play(pose);
        else
            HandAnimator_L.Play(pose);
    }

    public void CanAct(bool canAct)
    {
        weaponSwitching.canChange = canAct;
        weaponAnimator.SetBool("CanAct", canAct);
    }
}
