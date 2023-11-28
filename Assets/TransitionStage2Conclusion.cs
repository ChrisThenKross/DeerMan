using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionStage2Conclusion : MonoBehaviour
{
    public GameObject bossObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Health bossHealth = bossObject.GetComponent<Health>();
        if (bossHealth.currentHealth <= 0)
        {
            callScene();
        }
    }

    public void callScene()
    {
        Invoke(nameof(SceneCalling), 3.0f);
    }

    public void SceneCalling()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage 2 Conclusion");
    }
}