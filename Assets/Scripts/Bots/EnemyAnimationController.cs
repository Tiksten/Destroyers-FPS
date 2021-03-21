using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{   
    public GameObject enemy;

    public GameObject mainLookArmature; //Armature that controls bot view
    

    public Animator legsAnimator;

    public AnimationClip legsIdle;

    public AnimationClip legsWalk;



    public Animator chestAnimator;

    public AnimationClip chestShoot;

    public AnimationClip[] chestIdle;

    public AnimationClip chestHit;

    public AnimationClip[] chestLookingForPlayer;

    public AnimationClip chestWalk;


    [HideInInspector]
    Vector3 previousPos;


    void Start()
    {
        previousPos = enemy.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Legs
        if (previousPos == enemy.transform.position)
        {
            legsAnimator.SetBool("IsStaying", true);
            chestAnimator.SetBool("IsStaying", true);
        }
        else
        {
            legsAnimator.SetBool("IsStaying", false);
            chestAnimator.SetBool("IsStaying", false);
        }

        previousPos = enemy.transform.position;
    }

    public void DamageGiven(float npcHP, float maxHP)
    {
        chestAnimator.Play(chestHit.name);

        if (npcHP / maxHP <= 0.5f)
            chestAnimator.SetBool("Wounded", true);
    }

    public void Shoot(Vector3 target)
    {
        mainLookArmature.transform.LookAt(target);

        chestAnimator.Play(chestShoot.name);
        chestAnimator.SetBool("HaveSeenPlayer", true);
    }
}
