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

    public GameObject meleeHitHole;
    [Space(10)]

    [Header("ANIMATIONS")]
    public AnimationClip[] tagHitVariations;
    public AnimationClip[] tagStrongHitVariations;
    public AnimationClip[] tagIdleVariations;
    [Space(10)]

    [Header("SOUNDS")]
    public AudioClip[] hitSoundsVariations;
    public AudioClip[] strongHitSoundsVariations;
    [Space(10)]

    [Header("STATS")]
    public float meleeRange;
    public int damage;
    public int strongDamage;
    public int weaponWeight;
    public float hitForce;
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
                Hit(damage);
                if(hitSoundsVariations.Length != null)
                    audioSource.clip = hitSoundsVariations[Random.Range(0, hitSoundsVariations.Length)];
                audioSource.pitch = 1;
                audioSource.Play();
            }
            else if (Input.GetKey("mouse 1"))
            {
                var animation = tagStrongHitVariations[Random.Range(0, tagStrongHitVariations.Length)];
                animator.Play(animation.name);
                Hit(strongDamage);
                if (strongHitSoundsVariations.Length != null)
                    audioSource.clip = strongHitSoundsVariations[Random.Range(0, strongHitSoundsVariations.Length)];
                audioSource.pitch = 0.9f;
                audioSource.Play();
            }
        }
    }

    private void Hit(int damage)
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
                hit.rigidbody.AddForceAtPosition((hit.point - gameObject.transform.position) * hitForce, hit.point, ForceMode.Impulse);
            }
        }
    }
}
