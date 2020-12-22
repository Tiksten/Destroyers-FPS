using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Transform bloodPoint;
    public GameObject bloodEffect;

    public HealthBar healthBar;
    public GameObject gun;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(-20);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            GameObject effect = Instantiate(bloodEffect, bloodPoint.position, bloodPoint.rotation);
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}