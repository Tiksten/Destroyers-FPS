﻿using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;
    ReloadScript ammoScript;
    [HideInInspector]
    public bool canChange = true;
    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {

        int previousSelectedWeapon = selectedWeapon;
        ReloadScript ammoScript = GetComponentInChildren<ReloadScript>();
        if(ammoScript != null)
        {
            if(ammoScript.reloading == false)
                canChange = true;
            else
                canChange = false;
        }
        else
            canChange = true;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f & (canChange))
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f & (canChange))
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }
        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon ()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
