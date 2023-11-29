using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeAllBarrels : MonoBehaviour
{
    public GameObject GameElements;
    public ExplosionBarrel ExplodeScript;
    public GameObject Train;
    public Animator gates;
    private void OnTriggerEnter(Collider other)
    {
        Transform[] allChildren = GameElements.GetComponentsInChildren<Transform>();
        if (other.gameObject.layer == 10)
        {
            // explode all barrel
            foreach (Transform child in allChildren)
            {
                ExplodeScript = child.GetComponent<ExplosionBarrel>();
                if (ExplodeScript != null)
                {
                    ExplodeScript.Explode();
                }
            }

            //take damage
            Health healthScript = Train.GetComponent<Health>();
            if (healthScript != null)
            {
                Debug.Log("TAKING TADAMING)");
                healthScript.TakeDamage(1250);
            }
            gates.SetTrigger("gatesUp");
            gameObject.SetActive(false);
        }
    }
}
