using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AK47 : Weapon
{
    public void OnEnable()
    {
        weaponAnimator.Play(weaponDraw.name);
        weaponAnimator.Rebind();
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (canAct)
            {
                weaponAnimator.Play(randomWeaponShotStart.name);
                Fire();
                ResetRandomNumberInAnimator();
            }
        }

        else if (Input.GetKeyDown(KeyCode.R))
        {
            if(canAct)
            {
                weaponAnimator.Play(randomWeaponReloadStart.name);
                Reload();
                ResetRandomNumberInAnimator();
            }
        }
    }
}