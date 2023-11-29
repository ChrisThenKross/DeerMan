using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int gameStartScene;
    public GameObject Panel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameStartScene);
    }

    public void Settings()
    {
        if (Panel != null)
        {
            Panel.SetActive(true);
        }
    }

    public void BackToMain()
    {
        if (Panel.active == true)
        {
            Panel.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
