using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniversalWeaponScript : MonoBehaviour
{


    [Header("General")]
    public Camera fpsCam;
    public Animator animator;
    public GameObject bulletHolePrefab;
    [Space(10)]

    [Header("Animations")]
    public AnimationClip weaponDraw;
    public AnimationClip[] shootingVariations;
    public AnimationClip[] reloadingVariations;
    public AnimationClip[] idleVariations;
    public AnimationClip weaponPutAway;
    [Space(10)]

    [Header("Stats")]
    public int range;
    public int damage;
    public float impactForce;
    public float firespeed = 0.1f;
    [Space(10)]

    [Header("Sounds")]
    public AudioSource weaponAudioSource;
    public AudioClip[] shots;
    public SoundPlan[] reloadSoundsPlan;
    [Space(10)]

    [Header("Particles")]
    public ParticleSystem[] muzzleFlashes;
    [Space(10)]
    public ParticleSystem[] impactEffects;
    [Space(10)]
    public ParticleSystem[] barrelSmokes;
    [Space(10)]

    [Header("Ammo")]
    public int maxAmmo = 45;
    public int startAmmo = 40;
    public Text ammoText;
    [Space(10)]

    [Header("Cartriges")]
    public Transform cartrigeEjector;
    public GameObject cartrigePrefab;
    public float cartrigeForce = 5f;
    [Space(10)]

    

    [HideInInspector]
    public int currentAmmo;

    [HideInInspector]
    private float timecode = 0;

    [System.Serializable]
    public class SoundPlan
    {
        public float timeToWait;
        public AudioClip sound;
    }



    void Start()
    {
        currentAmmo = startAmmo;
        weaponDraw.wrapMode = WrapMode.Once;
        weaponPutAway.wrapMode = WrapMode.Once;
        foreach (AnimationClip i in shootingVariations)
            i.wrapMode = WrapMode.Once;
        foreach (AnimationClip i in reloadingVariations)
            i.wrapMode = WrapMode.Once;
        foreach (AnimationClip i in idleVariations)
            i.wrapMode = WrapMode.Once;
    }
    void Update()
    {
        //From HERE***********************************************

        if (Input.GetButton("Fire1") && currentAmmo != 0 && !animator.GetCurrentAnimatorStateInfo(0).IsName("Reloading"))
        {
            animator.SetBool("Shooting", true);
        }
        else
        {
            animator.SetBool("Shooting", false);
        }
        if ((Time.time >= timecode) && Input.GetButton("Fire1") && currentAmmo == 0 && !animator.GetCurrentAnimatorStateInfo(0).IsName("Reloading"))
        {
            Shot();
            timecode = Time.time + firespeed;

            foreach (ParticleSystem ps in barrelSmokes)
                ps.Stop();

            currentAmmo--;
        }
        else if (Input.GetButtonUp("Fire1") & !Input.GetButton("Fire1"))
        {
            foreach (ParticleSystem ps in barrelSmokes)
                ps.Play();
        }
        if (currentAmmo <= 0)
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo != maxAmmo && animator.GetCurrentAnimatorStateInfo(0).IsName("Reloading") != true)
            StartCoroutine(Reload());
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Reloading"))
        {
            ammoText.text = "Reload/" + maxAmmo;
        }
        else
        {
            ammoText.text = currentAmmo + "/" + maxAmmo;
        }

        //To HERE**************************************************************
    }
    void Shot()
    {
        foreach (ParticleSystem ps in muzzleFlashes)
            ps.Play();

        var cartrige = Instantiate(cartrigePrefab, cartrigeEjector.position, cartrigeEjector.rotation);
        Destroy(cartrige, 2f);

        var rb = cartrige.GetComponent<Rigidbody>();
        rb.AddForce(cartrigeEjector.right * (cartrigeForce + Random.Range(0, 0.2f)), ForceMode.Impulse);
        rb.AddTorque(Random.Range(-15, 15), Random.Range(-15, 15), 0, ForceMode.Impulse);

        weaponAudioSource.pitch = Random.Range(1.3f, 1.5f);
        weaponAudioSource.Play();

        RaycastHit hit;



        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Helper.GiveDamage(hit.collider.gameObject, 40);

            foreach (ParticleSystem ps in impactEffects)
                Instantiate(ps, hit.point, Quaternion.LookRotation(hit.normal));

            GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
            bulletHole.transform.parent = hit.transform;
        }



    }
    private IEnumerator Reload()
    {
        var totalWaitTime = 0f;
        var animation = reloadingVariations[Random.Range(0, reloadingVariations.Length)];


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
    }
}
