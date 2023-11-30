using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTrain : MonoBehaviour
{
    public GameObject train, Explosion;
    private Health trainHealth;

    // Start is called before the first frame update
    void Awake()
    {
        Explosion.SetActive(false);
        trainHealth = train.GetComponent<Health>();
    }

    public void Explode()
    {
        Explosion.SetActive(true);
    }

    private void Update()
    {
        if (trainHealth.currentHealth < 1)
        {
            Debug.Log("game is done");
            Explode();
            Destroy(gameObject,1);

            //FINAL ENDING TRIGGER GOES HERE!!!!
        }
    }


    private void OnTriggerEnter(Collider c)
    {
        if (trainHealth.currentHealth < 1667 && c.gameObject.tag == "UserProjectiles")
        {
            trainHealth.TakeDamage(75);
        }
    }
}
