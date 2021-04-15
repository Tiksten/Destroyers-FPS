using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public WeaponSwitching switchingScript;
    public PlayerInventory inventory;
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
        var UDS = collider.gameObject.GetComponent<UniversalDropStats>();

        UniversalDropStats.dropType objType = UniversalDropStats.dropType.None;

        if (UDS != null && Input.GetKey(KeyCode.F))
        {
            objType = UDS.type;


            switch(objType)
            {
                case UniversalDropStats.dropType.Weapon:
                    {
                        //Setting active weapon (if new)
                        foreach (Transform unactiveWeapon in unactiveWeapons)
                        {
                            if (unactiveWeapon.name == UDS.weaponName)
                            {
                                unactiveWeapon.SetParent(hands);
                                switchingScript.SelectWeapon();
                                break;
                            }
                        }


                        //Adding ammo
                        inventory.PickAmmo(UDS.weaponAmmoType, UDS.ammoInMag, UDS.weaponMaxAmmo);
                        Destroy(collider.gameObject);

                        break;
                    }
                case UniversalDropStats.dropType.Ammo:
                    {
                        inventory.PickAmmo(UDS.ammoType, UDS.ammoToAdd, UDS.ammoMax);
                        Destroy(collider.gameObject);

                        break;
                    }
                case UniversalDropStats.dropType.Health:
                    {
                        playerHealth.currentHealth += UDS.hpToAdd;
                        Destroy(collider.gameObject);

                        break;
                    }
                case UniversalDropStats.dropType.Armor:
                    {
                        Debug.Log("Armor");
                        Destroy(collider.gameObject);

                        break;
                    }
                case UniversalDropStats.dropType.CraftingMaterial:
                    {
                        Debug.Log("CraftingMaterial");
                        Destroy(collider.gameObject);

                        break;
                    }
            }    

            
            
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
