using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public WeaponSwitching switchingScript;
    public Transform hands;
    public bool AK47StartActive = true;
    public GameObject AK47;
    public bool GrenadeLauncherStartActive = true;
    public GameObject GrenadeLauncher;
    public bool SwordStartActive = true;
    public GameObject Sword;
    void Start()
    {
        if(AK47StartActive == false)
        {
            AK47.SetActive(false);
            AK47.transform.SetParent(gameObject.transform);
        }
        if(GrenadeLauncherStartActive == false)
        {
            GrenadeLauncher.SetActive(false);
            GrenadeLauncher.transform.SetParent(gameObject.transform);
        }
        if(SwordStartActive == false)
        {
            Sword.SetActive(false);
            Sword.transform.SetParent(gameObject.transform);
        }
    }

    // Update is called once per frame
private void OnTriggerEnter(Collider other) {
    if(other.tag == "Weapon" && Input.GetKey("f"))
    {
        if(other.name == "AK47")
        {
            Destroy(other);
            AK47.transform.SetParent(hands);

        }
        if(other.name == "GrenadeLauncher")
        {
            Destroy(other);
            GrenadeLauncher.transform.SetParent(hands);
        }
        if(other.name == "Sword")
        {
            Destroy(other);
            Sword.transform.SetParent(hands);
        }
    }
}
}
