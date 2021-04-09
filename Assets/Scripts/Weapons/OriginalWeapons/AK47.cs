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
        if (canAct)
        {
            if (Input.GetKey(KeyCode.Mouse0) && gunReadyToShoot)
            {
                weaponAnimator.Play(randomWeaponShotStart.name);
            }

            else if (Input.GetKey(KeyCode.R))
            {
                weaponAnimator.Play(randomWeaponReloadStart.name);
            }

            else if (Input.GetKey(KeyCode.F))
            {
                weaponAnimator.Play(randomWeaponInspectionStart.name);
            }
        }
    }
}