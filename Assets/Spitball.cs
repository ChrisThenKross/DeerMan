using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitball : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            // damage the enemy based on the damage
            if (collision.GetComponent<Health>() != null)
                collision.GetComponent<Health>().TakeDamage(damage);
        }
        //Apply spell effects to object / enemy once this hits something
        //Apply sound effects and particle effects
        Destroy(this.gameObject);
    }
}
