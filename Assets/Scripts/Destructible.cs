using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject DestroyedVersion;
    public GameObject WoodParticleSystem1;
    public GameObject WoodParticleSystem2;
    public GameObject WoodParticleSystem3;
    public GameObject WoodParticleSystem4;
    public GameObject WoodParticleSystem5;
    public GameObject WoodParticleSystem6;
    public GameObject WoodParticleSystem7;
    public GameObject WoodParticleSystem8;
    public GameObject WoodParticleSystem9;
    public GameObject WoodParticleSystem10;
    public Vector3 torque;
    public void Break()
    {
        Instantiate(DestroyedVersion, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
        Instantiate(WoodParticleSystem1, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WoodParticleSystem1, 5f);
        Instantiate(WoodParticleSystem2, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WoodParticleSystem2, 5f);
        Instantiate(WoodParticleSystem3, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WoodParticleSystem3, 5f);
        Instantiate(WoodParticleSystem4, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WoodParticleSystem4, 5f);
        Instantiate(WoodParticleSystem5, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WoodParticleSystem5, 5f);
        Instantiate(WoodParticleSystem6, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WoodParticleSystem6, 5f);
        Instantiate(WoodParticleSystem7, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WoodParticleSystem7, 5f);
        Instantiate(WoodParticleSystem8, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WoodParticleSystem8, 5f);
        Instantiate(WoodParticleSystem9, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WoodParticleSystem9, 5f);
        Instantiate(WoodParticleSystem10, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(WoodParticleSystem10, 5f);
    }
}