using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlowingUpGasPhase : MonoBehaviour
{
    public GameObject train, Explosion, Explosion2, enemies;
    private Health trainHealth;
    public Animator gates;
    [SerializeField] private TMP_Text storyText;

    private void Awake()
    {
        Explosion.SetActive(false);
        Explosion2.SetActive(false);
        trainHealth = train.GetComponent<Health>();
    }

    public void Explode()
    {
        Explosion.SetActive(true);
        Explosion2.SetActive(true);
    }

    private void Update()
    {
        if (trainHealth.currentHealth < 1666)
        {
            Explode();
            gates.SetTrigger("gatesUp");
            enemies.SetActive(true);
            storyText.text = "The conductor came out! This is it!";
            Destroy(gameObject);
        }
    }



    private void OnTriggerEnter(Collider c)
    {
        if (trainHealth.currentHealth < 5000 && c.gameObject.tag == "UserProjectiles")
        {
            trainHealth.TakeDamage(75);
        }
    }
}
