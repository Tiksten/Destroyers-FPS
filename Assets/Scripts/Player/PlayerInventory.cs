using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private List<Ammo> ammoInPlayerInvenory;

    [System.Serializable]
    public class Ammo
    {
        public string type;
        private int _count;
        public int count
        {
            get => _count;
            set => _count = (value <= maxCount) ? value : maxCount;
        }
        public int maxCount;
    }

    public bool CanPickThatAmmoType(string type)
    {
        var ammo = ammoInPlayerInvenory.Find(_ammo => _ammo.type == type);

        if (ammo != null)
        {
            if(ammo.count >= ammo.maxCount)
                return false;
        }

        return true;
    }

    //public bool CanPickThatGunType(string type) ******************************************************Work in progress********************************************************
    //{
    //    
    //}

    public void PickAmmo(string type, int amount, int maxAmount) 
    {
        SetMaxAmmoAmount(type, maxAmount);

        var ammo = ammoInPlayerInvenory.Find(_ammo => _ammo.type == type);

        if (ammo != null)
        {
            ammo.count += amount;
        }

        else if (amount > 0)
        {
            var newAmmo = new Ammo();
            newAmmo.count = amount;
            newAmmo.type = type;
            ammoInPlayerInvenory.Add(newAmmo);
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
                ammoInPlayerInvenory[ammoInPlayerInvenory.IndexOf(ammo)].count = amount;
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
        var ammo = ammoInPlayerInvenory.Find(_ammo => _ammo.type == type);
        if(ammo != null)
        {
            ammo.count -= amount;
        }
    }

    public void SetMaxAmmoAmount(string type, int maxAmount)
    {
        var ammo = ammoInPlayerInvenory.Find(_ammo => _ammo.type == type);

        if (ammo != null)
        {
            if(ammo.maxCount < maxAmount)
                ammo.maxCount = maxAmount;
        }

        else
        {
            var newAmmo = new Ammo();
            newAmmo.type = type;
            newAmmo.maxCount = maxAmount;
            ammoInPlayerInvenory.Add(newAmmo);
        }
    }
}
