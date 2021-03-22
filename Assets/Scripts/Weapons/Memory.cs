using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    public GameObject defaultBulletHolePrefab;

    public GameObject[] defaultImpactEffect;

    public BulletHoleEffectType[] bulletHoleTypes;

    public Helper.SprayPattern[] sprayPatterns;

    [System.Serializable]
    public class BulletHoleEffectType
    {
        public PhysicMaterial material;
        public GameObject[] bulletHolePrefab;
        public GameObject[] impactEffectParticles;
    }

    public GameObject BulletHoleChose(Collider collider)
    {
        var chosenBulletHole = defaultBulletHolePrefab;

        if (bulletHoleTypes != null)
        {
            foreach (BulletHoleEffectType type in bulletHoleTypes)
            {
                if (type.material == collider.sharedMaterial)
                {
                    chosenBulletHole = type.bulletHolePrefab[Random.Range(0, type.bulletHolePrefab.Length)];
                    break;
                }
            }
        }

        return chosenBulletHole;
    }

    public GameObject[] ImpactEffectChose(Collider collider)
    {
        var chosenImpactEffect = defaultImpactEffect;

        if (bulletHoleTypes != null)
        {
            foreach (BulletHoleEffectType type in bulletHoleTypes)
            {
                if (type.material == collider.sharedMaterial)
                {
                    chosenImpactEffect = type.impactEffectParticles;
                    break;
                }
            }
        }

        return chosenImpactEffect;
    }
}
