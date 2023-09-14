using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    // there is a better way to do this
    public int swordDamage;
    public int enemyDamage;
    public int currentHealth;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // there is a better way to do this aka health/damage for each entity
        // omg this is awful
        if (collision.collider.gameObject.CompareTag("Sword"))
        {
            TakeDamage(swordDamage);
        }
        if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(enemyDamage);
        }
    }
}
