using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFriendly : MonoBehaviour
{
    public int lifeTime;
    public int damage;

    private void Update()
    {
        Destroy(this.gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "EnemyParent")
        {
            Debug.Log("Gotcha! ENEMY");
            //damage the enemy based on the damage
            if (collision.gameObject.GetComponent<Health>() != null)
                collision.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
        //Apply spell effects to object / enemy once this hits something
        //Apply sound effects and particle effects
        Destroy(this.gameObject);
    }
}