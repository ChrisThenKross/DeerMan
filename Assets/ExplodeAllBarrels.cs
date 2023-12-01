using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExplodeAllBarrels : MonoBehaviour
{
    public ExplosionBarrel ExplodeScript;
    public GameObject Train, GameElements, Enemies;
    public Animator gates;
    [SerializeField] private TMP_Text storyText;
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
                healthScript.TakeDamage(1667);
            }
            Enemies.SetActive(true);
            gates.SetTrigger("gatesUp");
            storyText.text = "Hmm, looks like the gates are powered by those towers!";
            gameObject.SetActive(false);
        }
    }
}
