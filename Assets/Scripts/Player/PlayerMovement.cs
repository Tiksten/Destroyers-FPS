using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public AudioClip[] steps;
    public AudioSource audioSource;
    private bool stepPlayed;
    public float timeBetweenSteps;

    public float speed = 8f;
    public float runSpeed = 14f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform cellingCheck;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Animator handsAnim;

    Vector3 velocity;
    bool isGrounded;

    [HideInInspector]
    private Vector3 previousPos;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < -2f)
            velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;


        //Moving


        if (Input.GetKey("left shift"))
        {
            controller.Move(move * runSpeed * Time.deltaTime);
            handsAnim.SetBool("Running", true);
        }
        else
        {
            controller.Move(move * speed * Time.deltaTime);
            handsAnim.SetBool("Running", false);
            handsAnim.SetBool("Walking", true);
        }

        if (previousPos == gameObject.transform.position)
        {
            handsAnim.SetBool("Running", false);
            handsAnim.SetBool("Walking", false);
        }

        if (Input.GetKeyDown("space") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);


        //Crouching


        if (Input.GetKeyDown("left ctrl") && transform.localScale != new Vector3(1, 0.4f, 1))
        {
            transform.localScale = new Vector3(1, 0.4f, 1);
            foreach (Transform i in transform)
                i.localScale = new Vector3(1, i.localScale.y * 2.5f, 1);
        }

        else if (!Input.GetKey("left ctrl") && !Physics.CheckSphere(cellingCheck.position, 0.5f, groundMask) && transform.localScale != new Vector3(1, 1, 1))
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            gameObject.transform.position += new Vector3(0, gameObject.transform.position.y + 1, 0);
            foreach (Transform i in transform)
                i.localScale = new Vector3(1, i.localScale.y / 2.5f, 1);
        }


        //Other


        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        StepSoundPlay();

        previousPos = gameObject.transform.position;
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
        yield return new WaitForSeconds(timeBetweenSteps);
        stepPlayed = false;
    }
}