using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public void OnEnable()
    {
        weaponAnimator.Rebind();
    }

    public void Update()
    {
        ReloadAmmoText();

        if (weaponAnimator.GetBool("CanAct"))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (weaponAnimator.GetBool("GunReadyToShoot") && currentAmmoInMag > 0)
                {

                    weaponAnimator.Play(randomWeaponShotStart.name);

                }
            }

            else if (Input.GetKey(KeyCode.R))
            {
                if (currentAmmoInMag < maxAmmoInMag && inventory.GetAmmoCountOfType(ammoType) > 0)
                {

                    weaponAnimator.Play(randomWeaponReloadStart.name);

                }
            }

            else if (Input.GetKey(KeyCode.F))
            {

                weaponAnimator.Play(randomWeaponInspectionStart.name);
            }
        }
    }
}
