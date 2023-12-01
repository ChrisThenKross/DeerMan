using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnDeathBossFinal : MonoBehaviour
{
    public Animator gates;
    private Health bossHealth;
    [SerializeField] private TMP_Text storyText;
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
            storyText.text = "Time to end this!";
            Destroy(gameObject);
        }
    }
}
