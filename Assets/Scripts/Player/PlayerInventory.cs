using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private Ammo[] ammoInPlayerInvenory;

    [System.Serializable]
    public class Ammo
    {
        public string type;
        private int _count;
        public int count
        {
            get => _count;
            set => _count = Mathf.Clamp(value, 0, maxCount);
        }
        public int maxCount;
    }

    public bool CanPickThatAmmoType(string type)
    {
        foreach(Ammo ammo in ammoInPlayerInvenory)
        {
            if(ammo.type == type)
            {
                if (ammo.count < ammo.maxCount)
                    return true;

                return false;
            }
        }

        return true;
    }

    //public bool CanPickThatGunType(string type) ******************************************************Work in progress********************************************************
    //{
    //    
    //}

    public void PickAmmo(string type, int amount)
    {
        foreach (Ammo ammo in ammoInPlayerInvenory)
        {
            if (ammo.type == type)
            {
                ammo.count += amount;
                amount = 0;
                break;
            }
        }

        if(amount > 0)
        {
            var newAmmo = new Ammo();
            newAmmo.count = amount;
            newAmmo.type = type;
            ammoInPlayerInvenory[ammoInPlayerInvenory.Length] = newAmmo;
        }
    }

    public int GetAmmoCountOfType(string type)
    {
        foreach (Ammo ammo in ammoInPlayerInvenory)
        {
            if (ammo.type == type)
            {
                return ammo.count;
            }
        }

        return 0;
    }
    
    public void SetAmmoOfType(string type, int amount)
    {
        foreach (Ammo ammo in ammoInPlayerInvenory)
        {
            if (ammo.type == type)
            {
                ammo.count = amount;
            }
        }
    }

    public int FillMagFromInventory(int currentAmmoInMag, int maxAmmoInMag, string type)
    {
        var ammo = GetAmmoCountOfType(type);
        var needToFill = maxAmmoInMag - currentAmmoInMag;

        if(ammo < needToFill)
        {
            SetAmmoOfType(type, 0);
            return currentAmmoInMag += ammo;
        }

        RemoveAmmoFromInventory(type, needToFill);
        return maxAmmoInMag;
    }

    public void RemoveAmmoFromInventory(string type, int amount)
    {
        foreach (Ammo ammo in ammoInPlayerInvenory)
        {
            if (ammo.type == type)
            {
                ammo.count -= amount;
                break;
            }
        }
    }
}
