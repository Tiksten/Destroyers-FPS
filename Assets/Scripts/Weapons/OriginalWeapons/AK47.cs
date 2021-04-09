using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AK47 : MonoBehaviour
{
    //References


    [Header("REFERENCES")]
    public WeaponSwitching weaponSwitching;

    public PlayerInventory inventory;

    public Animator weaponAnimator;

    public Transform fpsCam;


    //Stats


    [Header("STATS")]
    public float damage;


    //Ammo


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


    //Animations


    [Header("ANIMATIONS")]
    public Animation randomWeaponInspectionStart; //Need to be 1 frame anim (only for events)
    public Animation randomWeaponShotStart;
    public Animation randomWeaponIdleStart;
    public Animation randomWeaponReloadStart;
    public Animation weaponDraw; //Normal anim


    //Effects


    [Header("EFFECTS")]
    public ParticleSystem[] barrelSmokes;
    public ParticleSystem[] muzzleFlashes;
    public Transform cartrigeEjector;
    public GameObject cartrigePrefab;
    public float cartrigeForce = 5f;
    public float impactForce = 5;


    //Spray


    [Header("SPRAY PATTERN")]
    public string sprayPatternName;
    public float timeToResetOneStep;
    public float timeToResetFullRecoil;
    [HideInInspector]
    public int currentSprayStep;
    [HideInInspector]
    public float recoilMultiplier;


    //Sounds


    public AudioSource weaponAudioSource;


    private void Start()
    {
        //Set all references
        weaponAnimator = gameObject.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        weaponAnimator.Rebind();
        weaponAnimator.Play("weaponDraw");
    }



    private void Fire()
    {
        StopAllCoroutines();

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

        StartCoroutine(SprayStabilize());

        ReloadAmmoText();
    }

    private void Inspect()
    {
        weaponAnimator.Play("randomWeaponInspectionStart");
    }

    private void Reload()
    {
        currentAmmoInMag = inventory.FillMagFromInventory(currentAmmoInMag, maxAmmoInMag, ammoType);
        StopAllCoroutines();
        currentSprayStep = 0;
    }

    private void SetAnimRandomNumber(int maxNum = 1)
    {
        weaponAnimator.SetInteger("RandomNumber", Random.Range(0, maxNum));
    }

    private void CanAct(bool canAct)
    {
        weaponSwitching.canChange = canAct;
        weaponAnimator.SetBool("CanAct", canAct);
    }

    private void ReloadAmmoText()
    {
        ammoText.text = currentAmmoInMag + "/" + inventory.GetAmmoCountOfType(ammoType);
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
            if (totalTime < timeToResetOneStep)
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

    private void Recoil(float horizontal, float vertical)
    {
        var ml = fpsCam.GetComponent<MouseLook>();
        ml.AddRotation(horizontal * recoilMultiplier, vertical * recoilMultiplier, 0);
    }
}
