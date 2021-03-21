using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public EnemyWeapon weaponScript;

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


    public Vector3 currentTarget;


    public float damage;

    [HideInInspector]
    Vector3 previousPos;


    void Start()
    {
        previousPos = enemy.transform.position;
        currentTarget = transform.position + new Vector3(0, 0, 2);
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

        //Look
        var look = mainLookArmature.transform;
        look.LookAt(currentTarget);
        var lookInDegrees = look.rotation.eulerAngles;

        if (lookInDegrees.x < 320 && lookInDegrees.x > 180)
            lookInDegrees.x = 320;
        else if (lookInDegrees.x > 40 && lookInDegrees.x < 180)
            lookInDegrees.x = 40;

        mainLookArmature.transform.rotation = Quaternion.Euler(-lookInDegrees.x, lookInDegrees.y + 180, lookInDegrees.z);
        Debug.Log(-lookInDegrees.x);
    }

    public void DamageGiven(float npcHP, float maxHP)
    {
        chestAnimator.Play(chestHit.name);

        if (npcHP / maxHP <= 0.5f)
            chestAnimator.SetBool("Wounded", true);
    }

    public void Shoot(Vector3 target)
    {
        currentTarget = target + new Vector3(0, 0.5f, 0);
        chestAnimator.Play(chestShoot.name);
        chestAnimator.SetBool("HaveSeenPlayer", true);
    }
}
