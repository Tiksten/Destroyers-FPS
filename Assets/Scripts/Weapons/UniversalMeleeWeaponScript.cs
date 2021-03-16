using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniversalMeleeWeaponScript : MonoBehaviour
{
    [Header("GENERAL")]

    [HideInInspector]
    public Camera fpsCam;
    [HideInInspector]
    public Memory memory;
    [HideInInspector]
    public Animator animator;

    public bool strongHitTypeIsMomental = true;

    public GameObject meleeHitHole;
    [Space(10)]

    [Header("ANIMATIONS")]
    public AnimationClip[] tagHitVariations;
    public AnimationClip[] tagStrongHitVariations;
    public AnimationClip[] tagIdleVariations;
    public AnimationClip strongHitGettingReady;
    [Space(10)]

    [Header("SOUNDS")]
    public AudioClip[] hitSoundsVariations;
    public AudioClip[] strongHitSoundsVariations;

    public float hitPitch = 1;
    public float strongHitPitch = 0.9f;
    [Space(10)]

    [Header("STATS")]
    public float meleeRange = 0.8f;
    public float damage = 20;
    public int strongDamage = 40;
    public int weaponWeight = 1;
    public float hitForce = 5;
    public float strongHitForce = 8;
    public float strongHitReadyTime = 1;
    [Space(10)]

    [Header("PARTICLES")]
    public ParticleSystem[] hitEffects;

    [HideInInspector]
    AudioSource audioSource;

    void Start()
    {
        foreach (AnimationClip i in tagHitVariations)
        {
            i.wrapMode = WrapMode.Once;
        }
        foreach (AnimationClip i in tagStrongHitVariations)
        {
            i.wrapMode = WrapMode.Once;
        }
        foreach (AnimationClip i in tagIdleVariations)
        {
            i.wrapMode = WrapMode.Once;
        }

        fpsCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        animator = gameObject.GetComponent<Animator>();

        memory = FindObjectOfType<Memory>();

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        var canHit = !animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit");

        if (canHit)
        {
            if (Input.GetKey("mouse 0"))
            {
                var animation = tagHitVariations[Random.Range(0, tagHitVariations.Length)];
                animator.Play(animation.name);

                Hit(damage, hitForce);

                if(hitSoundsVariations.Length != 0)
                    audioSource.clip = hitSoundsVariations[Random.Range(0, hitSoundsVariations.Length)];
                audioSource.pitch = hitPitch;
                audioSource.Play();
            }
            else if (Input.GetKey("mouse 1") && strongHitTypeIsMomental)
            {
                var animation = tagStrongHitVariations[Random.Range(0, tagStrongHitVariations.Length)];
                animator.Play(animation.name);

                Hit(strongDamage, strongHitForce);

                if (strongHitSoundsVariations.Length != 0)
                    audioSource.clip = strongHitSoundsVariations[Random.Range(0, strongHitSoundsVariations.Length)];
                audioSource.pitch = strongHitPitch;
                audioSource.Play();
            }
            else if (Input.GetKey("mouse 1") && !strongHitTypeIsMomental)
            {
                StartCoroutine(StartStrongHitTotalSumCounter());
            }
        }
    }

    private void Hit(float damage, float force)
    {
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, meleeRange))
        {
            Helper.GiveDamage(hit.collider.gameObject, damage);

            Debug.Log(hit.collider.gameObject.name);


            //Impact Effects
            if (hitEffects.Length != 0)
            {
                foreach (ParticleSystem ps in hitEffects)
                    Instantiate(ps, hit.point, Quaternion.LookRotation(hit.normal));
            }



            //Meele Hit Hole
            GameObject bulletHole = Instantiate(meleeHitHole, hit.point, Quaternion.LookRotation(hit.normal));

            bulletHole.transform.parent = hit.transform;



            //Impact
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForceAtPosition((hit.point - gameObject.transform.position) * force, hit.point, ForceMode.Impulse);
            }
        }
    }

    private IEnumerator StartStrongHitTotalSumCounter()
    {
        var totalTime = strongHitReadyTime;
        var _damage = (strongDamage - damage)/(totalTime * 10);
        var totalDamage = damage;


        animator.Play(strongHitGettingReady.name);

        while (Input.GetKey("mouse 1"))
        {
            if(totalTime > 0)
                yield return new WaitForSeconds(0.1f);
                totalDamage += _damage;
                totalTime -= 0.1f;
        }

        var animation = tagStrongHitVariations[Random.Range(0, tagStrongHitVariations.Length)];
        animator.Play(animation.name);

        Hit(strongDamage, totalDamage);

        if (strongHitSoundsVariations.Length != 0)
            audioSource.clip = strongHitSoundsVariations[Random.Range(0, strongHitSoundsVariations.Length)];
        audioSource.pitch = strongHitPitch;
        audioSource.Play();
    }
}
