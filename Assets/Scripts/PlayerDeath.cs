using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject deathPanel;
    public UnityEngine.SceneManagement.Scene currentScene;
    public bool UpdateBypass = true;

    void Start()
    {
        deathPanel.SetActive(false);
        currentScene = SceneManager.GetActiveScene();
    }
    // Update is called once per frame
    void Update()
    {
        if (UpdateBypass)
            DeadCheck();
    }

    public bool DeadCheck()
    {
        Health playerHealth = playerObject.GetComponent<Health>();
        if (playerHealth.currentHealth <= 0)
        {
            deathPanel.SetActive(true);
            UpdateBypass = false;
            return true;
        }
        else
            return false;
        
    }
    public void RestartTrigger()
    {
        SceneManager.LoadScene(currentScene.name);
    }
}
