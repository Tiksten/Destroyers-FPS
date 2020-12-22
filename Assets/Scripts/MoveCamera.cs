using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public Transform player;
    public Transform deathCam;

    void Update() 
    {
        if (player != null)
        {
            transform.position = player.position;
        }
        else
        {
            transform.position = deathCam.position;
            transform.rotation = deathCam.rotation;
        }
    }
}
