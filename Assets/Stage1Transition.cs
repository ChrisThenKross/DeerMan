using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1Transition : MonoBehaviour
{
    public GameObject bossObject;
    void Update()
    {
        Health bossHealth = bossObject.GetComponent<Health>();
        Debug.Log(bossHealth.currentHealth);
        if (bossHealth.currentHealth <= 0)
        {
            Debug.Log("yo he died");
            SceneManager.LoadScene("Stage 1 Conclusion");
        }
    }
}
