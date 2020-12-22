using UnityEngine;

public class Synchronize : MonoBehaviour {

    public Transform player;

    void Update() 
    {
        if (player != null)
        {
            transform.position = player.position;
        }
    }
}