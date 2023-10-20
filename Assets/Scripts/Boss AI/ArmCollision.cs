using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmCollision : MonoBehaviour
{
    BoxCollider boxCollider;
    public int damage;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (col.GetComponent<Health>() != null)
                col.GetComponent<Health>().TakeDamage(damage);
            Debug.Log("DOUCHE!!");
        }
    }
}
