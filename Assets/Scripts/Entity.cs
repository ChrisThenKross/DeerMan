using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    protected virtual void Start () {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth (maxHealth);
    }

    protected void TakeDamage (int damage) {
        currentHealth -= damage;
        healthBar.SetHealth (currentHealth);

        if (currentHealth <= 0) {
            Die ();
        }
    }

    protected virtual void Die () {
        // Destroy (gameObject);
    }
}