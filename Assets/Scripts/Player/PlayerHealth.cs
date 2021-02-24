using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Transform bloodPoint;
    public GameObject bloodEffect;
    public GameObject healthEffect;
    public HealthBar healthBar;
    public GameObject gun;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) & (currentHealth < maxHealth))
        {
            TakeDamage(-20);
            GameObject effectH = Instantiate(healthEffect, bloodPoint.position, bloodPoint.rotation);
            Destroy(effectH, 7f);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            GameObject effectB = Instantiate(bloodEffect, bloodPoint.position, bloodPoint.rotation);
            Destroy(effectB, 1f);
            currentHealth = 0;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}