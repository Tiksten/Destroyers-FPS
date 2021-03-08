using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    public GameObject defaultBulletHolePrefab;

    public BulletHoleType[] bulletHoleTypes;

    [System.Serializable]
    public class BulletHoleType
    {
        public PhysicMaterial material;
        public GameObject[] bulletHolePrefab;
    }

    public GameObject BulletHoleChose(Collider collider)
    {
        var chosenBulletHole = defaultBulletHolePrefab;

        if (bulletHoleTypes != null)
        {
            foreach (BulletHoleType type in bulletHoleTypes)
            {
                if (type.material == collider.sharedMaterial)
                    chosenBulletHole = type.bulletHolePrefab[Random.Range(0, type.bulletHolePrefab.Length)];
            }
        }

        return chosenBulletHole;
    }
}
