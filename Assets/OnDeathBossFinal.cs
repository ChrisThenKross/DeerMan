using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathBossFinal : MonoBehaviour
{
    public Animator gates;
    private Health bossHealth;
    // Start is called before the first frame update
    void Awake()
    {
        bossHealth = gameObject.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bossHealth.currentHealth < 0) 
        {
            gates.SetTrigger("gatesDown");
            Destroy(gameObject);
        }
    }
}
