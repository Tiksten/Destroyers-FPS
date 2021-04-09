using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public WeaponSwitching switchingScript;
    public Transform hands;
    public Transform unactiveWeapons;
    public string[] startWeapons;

    void Start()
    {
        //Set unactive all weapons without startWeapons
        foreach (Transform child in unactiveWeapons)
        {
            child.gameObject.SetActive(false);
        }

        foreach (string weaponName in startWeapons)
        {
            AddNewWeapon(weaponName);
        }

        hands.transform.GetChild(0).gameObject.SetActive(true);
        switchingScript.SelectWeapon();
    }

    private void OnTriggerStay(Collider collider)
    {
        var UDS = collider.GetComponent<UniversalDropStats>();
        string objName = null;

        if (UDS != null)
        {
            objName = UDS._name;


            //Ammo Health Armor Crafting material etc
            if (collider.tag == "CanBePickedUp")
            {
                if (objName == "Health")
                {
                    playerHealth.currentHealth += UDS.hpToAdd;
                }

                else if (objName == "Armor")
                {
                    Debug.Log("Armor");
                }

                else if (objName == "Crafting material")
                {
                    Debug.Log("Crafting material");
                }
            }


            //Weapon
            else if (collider.tag == "Weapon" && Input.GetKey("f"))
            {
                Debug.Log("trying to pick up weapon");
                bool weaponIsNew = false;

                //Setting active weapon (if new)
                foreach (Transform unactiveWeapon in unactiveWeapons)
                {
                    if (unactiveWeapon.name == objName)
                    {
                        unactiveWeapon.SetParent(hands);
                        weaponIsNew = true;
                        break;
                    }
                }


                //Adding ammo
                foreach (Transform activeWeapon in hands)
                {
                    if (activeWeapon.name == objName)
                    {
                        //var UWS = activeWeapon.GetComponent<UniversalWeaponScript>();
                        var weaponStats = collider.gameObject.GetComponent<UniversalDropStats>();

                        //if (UWS != null)
                        //{
                        //    if (weaponIsNew)
                        //        UWS.AmmoInMag = weaponStats.ammoInMag;
                        //    else
                        //        UWS.Ammo += weaponStats.ammoInMag;
                        //}

                        Destroy(collider.gameObject);
                    }
                }
            }

            switchingScript.SelectWeapon();
        }
    }

    public void AddNewWeapon(string weaponName)
    {
        foreach (Transform weapon in unactiveWeapons)
        {
            if (weaponName == weapon.gameObject.name)
            {
                weapon.SetParent(hands);
                switchingScript.SelectWeapon();
            }
        }
    }

    public void DropWeapon(string weaponName)
    {
        foreach (Transform weapon in hands)
        {
            if (weaponName == weapon.gameObject.name)
            {
                weapon.SetParent(unactiveWeapons);
                switchingScript.SelectWeapon();

                //Drop weapon ent ***********************************************************
            }
        }
    }
}
