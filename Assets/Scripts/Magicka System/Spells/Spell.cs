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

    private void Awake()
    {
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
        //Apply spell effects to object / enemy once this hits something
        //Apply sound effects and particle effects
        Destroy(this.gameObject);
    }
}
