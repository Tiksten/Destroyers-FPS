using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Ammo[] ammoInPlayerInvenory;

    [System.Serializable]
    public class Ammo
    {
        public string type;
        private int _count;
        public int count
        {
            get => _count;
            set => _count = (value + _count <= maxCount) ? _count + value : maxCount;
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
                break;
            }
        }

        return false;
    }

    //public bool CanPickThatGunType(string type) ******************************************************Work in progress********************************************************
    //{
    //    
    //}

    public void PickAmmo(string type, int count)
    {
        foreach (Ammo ammo in ammoInPlayerInvenory)
        {
            if (ammo.type == type)
            {
                ammo.count += count;
                break;
            }
        }
    }
}
