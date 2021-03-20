using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public Animator legsAnimator;
    public Animator chestAnimator;

    public GameObject enemy;

    [HideInInspector]
    Vector3 previousPos;

    public GameObject mainLookArmature; //Armature that controls bot view

    public AnimationClip legsIdle;

    public AnimationClip legsWalk;

    void Start()
    {
        previousPos = enemy.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Legs
        if (previousPos == enemy.transform.position && !legsAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
        {
            legsAnimator.Play(legsIdle.name);
        }
        else if (!legsAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Walk"))
        {
            legsAnimator.Play(legsWalk.name);
        }

        //Chest


        previousPos = enemy.transform.position;
    }
}
