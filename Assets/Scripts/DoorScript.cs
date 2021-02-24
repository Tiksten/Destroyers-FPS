using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool doorOpen = false;
    private Animator doorAnimator;

    // Start is called before the first frame update
    void Start()
    {
        doorAnimator = GetComponent<Animator>();
    }


    public void Activate()
    {
        if (!doorOpen)
        {
            doorAnimator.Play("DoorOpen");
            doorOpen = true;
        }
        else
        {
            doorAnimator.Play("DoorClosed");
            doorOpen = false;
        }
    }
}
