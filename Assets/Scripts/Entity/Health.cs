using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    public int maxHealth = 100;
    // there is a better way to do this
    public int swordDamage;
    public int enemyDamage;
    public int currentHealth;

    //public HealthBar healthBar;
    public Slider healthBar;

    // Start is called before the first frame update
    void Start () {
        currentHealth = maxHealth;
        healthBar.value = maxHealth;
    }

    // Update is called once per frame
    void Update () {

    }

    void TakeDamage (int damage) {
        Debug.Log (currentHealth);
        Debug.Log (damage);
        currentHealth -= damage;
        if (currentHealth <= 0) {
            print ("Ahh! I ahve been killed");
            Destroy (gameObject);
        }
        healthBar.value = currentHealth;
    }

    private void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.name == "Spell_Fireball(Clone)") {
            Debug.Log ("I've been hit!");
            //RIGHT NOW THIS dmg IS HARDCODED BUT HAVE THIS REFERENCE THE SCRIPTABLE OBJECT LATER
            TakeDamage (10);
        }

    }
}