using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Health))]
public class Entity : Health {
    // public int maxHealth = 100;
    // public int currentHealth;
    public override void Start () {
        base.Start();
        //     currentHealth = maxHealth;
        //     healthBar.SetMaxHealth (maxHealth);
    }

    // protected void TakeDamage (int damage) {
    //     currentHealth -= damage;
    //     healthBar.SetHealth (currentHealth);

    //     if (currentHealth <= 0) {
    //         Die ();
    //     }
    // }

    // protected virtual void Die () {
    //     // Destroy (gameObject);
    // }
}