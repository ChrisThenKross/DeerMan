using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plower : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("BOOM BITCH!");
            //damage the enemy based on the damage
            if (collision.gameObject.GetComponent<Health>() != null)
                collision.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
