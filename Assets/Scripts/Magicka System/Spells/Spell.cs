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
        // not sure why this needs to be offset by 1 but ok
        pierceLeft = SpellToCast.enemiesCanPierce + 1;

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
        if (collision.tag == "Enemy")
        {
            // damage the enemy based on the damage
            if (collision.GetComponent<Health>() != null)
                collision.GetComponent<Health>().TakeDamage((int)SpellToCast.Damage);
        }

        //splash damage (main hit above does most damage, everything below is lingering damage
        if (SpellToCast.SplashDamage)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, SpellToCast.SplashDamageRadius);
            foreach(Collider collider in colliders)
            {
                if (collider.GetComponent<Health>() != null)
                    collider.GetComponent<Health>().TakeDamage((int)SpellToCast.Damage - 3); // yeah so its -3 damage penalty for splash
            }
        }


        //Apply spell effects to object / enemy once this hits something
        //Apply sound effects and particle effects

        // wait should we destroy or pierce?
        
        if (pierceLeft == 0)
            Destroy(this.gameObject);
        else
        {
            pierceLeft--;
        }
    }
}
