using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecAbilities : MonoBehaviour
{
    [Header("HANDS")]
    public Animator handsAnim;

    public AnimationClip weaponPullAway;
    [Space(10)]


    [Header("SPEC ABILITIES")]
    public Camera fpsCam;
    public AnimationClip meleeKnife;
    public GameObject soliderGloves;
    public GameObject knife;
    public ParticleSystem[] hitEffects;
    public GameObject meleeHitHole;


    private Animator specAbilityAnim;

    // Start is called before the first frame update
    void Start()
    {
        specAbilityAnim = gameObject.GetComponent<Animator>();

        foreach (Transform i in transform)
        {
            i.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("v") && !knife.active && !soliderGloves.active)
        {
            knife.SetActive(true);
            soliderGloves.SetActive(true);
            specAbilityAnim.Rebind();
            Hit(50, 5);
            handsAnim.Play(weaponPullAway.name);
            specAbilityAnim.Play(meleeKnife.name);
            StartCoroutine(Helper.Deactivate(soliderGloves, 0.5f));
            StartCoroutine(Helper.Deactivate(knife, 0.5f));
        }
    }

    private void Hit(float damage, float force)
    {
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, 1.5f))
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
}
