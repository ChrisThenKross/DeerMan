using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]

public class Spell : MonoBehaviour
{
    public SpellScriptableObject SpellToCast;

    private SphereCollider myCollider;
    private Rigidbody myRigidbody;
    private int pierceLeft;

    private void Awake()
    {
        pierceLeft = SpellToCast.enemiesCanPierce;

        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = SpellToCast.SpellRadius;

        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;


        Destroy(this.gameObject, SpellToCast.Lifetime);
    }

    private void Update()
    {
        if (SpellToCast.Speed > 0) transform.Translate(Vector3.forward * SpellToCast.Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "EnemyParent")
        {
            // damage the enemy based on the damage
            if (collision.GetComponent<Health>() != null)
            {
                if (collision.gameObject.name == "Weak Spot")
                {
                    Debug.Log("hit weakspot");
                    // i dont wanna do this
                }
                collision.GetComponent<Health>().TakeDamage((int)SpellToCast.Damage);
            }
        }

        //splash damage (main hit above does most damage, everything below is lingering damage
        if (SpellToCast.SplashDamage)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, SpellToCast.SplashDamageRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<Health>() != null)
                    collider.GetComponent<Health>().TakeDamage((int)SpellToCast.Damage - 3); // yeah so its -3 damage penalty for splash
            }
        }

        // wait should we destroy or pierce?
        if (pierceLeft > 0 && (collision.tag == "Enemy" || collision.tag == "EnemyParent"))
        {
            if (SpellToCast.onHitFX != null)
            {
                
                Debug.Log("doing FX");
                GameObject fx = Instantiate(SpellToCast.onHitFX, transform.position, Quaternion.identity);
                Destroy(fx, 1.5f);
            } else
            {
                Debug.Log("yeah im not d oing fx");
            }
            pierceLeft--;
        }
        else
        {
            Destroy(this.gameObject);
        }


        //Apply spell effects on hit (explosion, etc)
        if (SpellToCast.onHitFX != null)
        {
            Debug.Log("hit successfully");
            GameObject fx = Instantiate(SpellToCast.onHitFX, transform.position, Quaternion.identity);
            Destroy(fx, 1.5f);
        }

        //Knockback
        if (SpellToCast.knockback)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, SpellToCast.knockbackRadius);

            foreach (Collider nearby in colliders)
            {
                Rigidbody rig = nearby.GetComponent<Rigidbody>();
                if (rig != null)
                {
                    rig.AddExplosionForce(SpellToCast.knockbackForce, transform.position, SpellToCast.knockbackRadius);
                }
            }
        }

    }
}
