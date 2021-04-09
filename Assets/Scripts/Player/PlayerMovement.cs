using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;


    public WeaponSwitching weaponSwitching;

    public AudioClip[] steps;
    public AudioSource audioSource;
    private bool stepPlayed;
    public float timeBetweenSteps_Walk;
    public float timeBetweenSteps_Run;
    public float timeBetweenSteps_Crouch;

    public float walkSpeed = 8f;
    public float crouchSpeed = 5f;
    public float runSpeed = 14f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform cellingCheck;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Animator handsAnim;

    Vector3 velocity;


    public Camera cam;

    [HideInInspector]
    public bool isGrounded;

    [HideInInspector]
    public float currentSpeed;

    [HideInInspector]
    private Vector3 previousPos;

    [HideInInspector]
    public bool isStaying;

    [HideInInspector]
    public bool isRunning;

    [HideInInspector]
    public float currentTimeBetweenSteps;

    [HideInInspector]
    public float inAirMultiplier = 1f;

    [HideInInspector]
    public float minInAirMultiplier = 0.5f;

    [HideInInspector]
    bool canAddVelocity = true;

    // Update is called once per frame
    void Update()
    {


        //Bool defenition


        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (previousPos == gameObject.transform.position)
        {
            isStaying = true;
        }
        else
            isStaying = false;

        isRunning = Input.GetKey("left shift");
        if (Input.GetKey("left ctrl") || isStaying)
            isRunning = false;

        previousPos = gameObject.transform.position;


        //Some manipulation


        if (isGrounded && velocity.y < -2f)
            velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x * inAirMultiplier + transform.forward * z * inAirMultiplier;


        //Moving


        handsAnim.SetBool("Falling", false);
        if (isStaying)
        {
            //StayingAnim
            handsAnim.SetBool("Running", false);
            handsAnim.SetBool("Walking", false);
            handsAnim.SetBool("RunAndGun", false);
        }
        else
        {
            if (isRunning)
            {
                //RunningAnim
                handsAnim.SetBool("Running", true);
            }
            else
            {
                //WalkingAnim
                handsAnim.SetBool("Running", false);
                handsAnim.SetBool("Walking", true);
            }
        }


        //Move


        if (!Input.GetKey("left ctrl") && Input.GetKey("left shift"))
        {
            //Run
            currentSpeed = runSpeed;
            currentTimeBetweenSteps = timeBetweenSteps_Run;
        }
        else if (!Input.GetKey("left ctrl"))
        {
            //Walk
            currentSpeed = walkSpeed;
            currentTimeBetweenSteps = timeBetweenSteps_Walk;
        }
        else if (Input.GetKey("left ctrl"))
        {
            //Crouch
            currentSpeed = crouchSpeed;
            currentTimeBetweenSteps = timeBetweenSteps_Crouch;
        }
            
        if(isStaying && !Input.GetKey("left ctrl"))

        controller.Move(move * currentSpeed * Time.deltaTime);


        //Jump


        if (Input.GetKeyDown("space") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);

        if (isGrounded)
            inAirMultiplier = 1f;
        else
        {
            if(inAirMultiplier > minInAirMultiplier)
                inAirMultiplier -= 0.001f;
        }


        //Crouching


        if (Input.GetKeyDown("left ctrl") && transform.localScale != new Vector3(1, 0.4f, 1))
        {
            transform.localScale = new Vector3(1, 0.4f, 1);
            foreach (Transform i in transform)
                i.localScale = new Vector3(1, i.localScale.y * 2.5f, 1);
        }

        else if (!Input.GetKey("left ctrl") && transform.localScale != new Vector3(1, 1, 1))
        {
            if (!Physics.CheckSphere(cellingCheck.position, 0.5f, groundMask))
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                gameObject.transform.position += new Vector3(0, 1, 0);
                foreach (Transform i in transform)
                    i.localScale = new Vector3(1, i.localScale.y / 2.5f, 1);
            }
        }


        //Other


        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        StepSoundPlay();
    }

    void OnTriggerEnter(Collider info)
    {
        var rb = info.GetComponent<Rigidbody>();
        if (rb != null && canAddVelocity == true)
        {
            canAddVelocity = false;
            rb.velocity = new Vector3(0, 2, 0);
            StartCoroutine(VelocityWait());
        }
    }

    private IEnumerator VelocityWait()
    {
        yield return new WaitForSeconds(0.5f);
        canAddVelocity = true;
    }

    private void StepSoundPlay()
    {
        if (!stepPlayed)
        {
            if (isGrounded && (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0))
            {
                stepPlayed = true;
                audioSource.clip = steps[Random.Range(0, steps.Length)];
                audioSource.Play();
                StartCoroutine(Wait());
            }
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(currentTimeBetweenSteps);
        stepPlayed = false;
    }
}