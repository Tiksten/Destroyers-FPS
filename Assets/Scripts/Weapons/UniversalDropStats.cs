using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class UniversalDropStats : MonoBehaviour
{
    public enum dropType
    {
        None,
        Ammo,
        Weapon,
        Health,
        Armor,
        CraftingMaterial
    }

    private int defaultMaxAmmo = 15;

    public dropType type = dropType.None;

    [ConditionalField(nameof(type), false, dropType.Ammo)]public string ammoType;
    [ConditionalField(nameof(type), false, dropType.Ammo)]public int ammoToAdd;
    [ConditionalField(nameof(type), false, dropType.Ammo)]public bool changesMaxAmmo = false;
    [ConditionalField(nameof(changesMaxAmmo), false, true)]public int ammoMax;

    [ConditionalField(nameof(type), false, dropType.Armor)]public int armorToAdd;

    [ConditionalField(nameof(type), false, dropType.CraftingMaterial)]public string[] craftingMaterialToAdd;

    [ConditionalField(nameof(type), false, dropType.Health)]public int hpToAdd;

    [ConditionalField(nameof(type), false, dropType.Weapon)]public string weaponName;
    [ConditionalField(nameof(type), false, dropType.Weapon)]public int ammoInMag;
    [ConditionalField(nameof(type), false, dropType.Weapon)]public int weaponMaxAmmo;
    [ConditionalField(nameof(type), false, dropType.Weapon)]public string weaponAmmoType;


    private void Awake()
    {
        if (!changesMaxAmmo)
            ammoMax = 15;
    }
}