using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerExplode : MonoBehaviour
{
    public GameObject Tower, Explosion, Train;
    private Health trainHealth;

    private void Awake()
    {
        Tower.SetActive(true);
        Explosion.SetActive(false);
        trainHealth = Train.GetComponent<Health>();
    }

    public void Explode()
    {
        Tower.SetActive(false);
        Explosion.SetActive(true);
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter(Collider c)
    {
        if (trainHealth.currentHealth < 5000 && c.gameObject.tag == "UserProjectiles")
        {
            Explode();
        }
    }
}
