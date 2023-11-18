using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;

    Animator animator;

    // Start is called before the first frame update
    public virtual void Start()
    {
        currentHealth = maxHealth;
        // healthBar.value = maxHealth;
        animator = GetComponent<Animator>();
    }

    // public virtual void Update()
    // {
    //     healthBar.value = currentHealth;
    // }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            animator.SetTrigger("Die");
            // stop all interaction
            this.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
            // stops the ai from shooting
            if (this.gameObject.GetComponent<EnemyAI>() != null)
                this.gameObject.GetComponent<EnemyAI>().enabled = false;
            // stop player from moveing
            if (this.gameObject.GetComponent<PlayerController>() != null)
                this.gameObject.GetComponent<PlayerController>().enabled = false;
            // remove the healthbar
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);

            //remove the object
            Destroy(gameObject,3);
        }
        
        healthBar.value = currentHealth;
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}