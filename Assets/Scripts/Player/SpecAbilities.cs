using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecAbilities : MonoBehaviour
{
    [Header("HANDS")]
    public Animator handsAnim;

    public AnimationClip weaponPullAway;

    public AnimationClip weaponPullOut;
    [Space(10)]


    [Header("SPEC ABILITIES")]
    public AnimationClip meleeKnife;
    public GameObject soliderGloves;
    public GameObject knife;


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
            handsAnim.Play(weaponPullAway.name);
            specAbilityAnim.Play(meleeKnife.name);
            handsAnim.Play(weaponPullOut.name);
            StartCoroutine(Helper.Deactivate(soliderGloves, 0.5f));
            StartCoroutine(Helper.Deactivate(knife, 0.5f));
        }
    }
}
