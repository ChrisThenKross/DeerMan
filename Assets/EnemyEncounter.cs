using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEncounter : MonoBehaviour
{
    public GameObject boss;

    private bool bossSpawned = false;

    // Update is called once per frame
    void Update()
    {
        // Empty that contains the enemies
        GameObject empty = GameObject.Find("Enemies");
        GameObject[] enemies = new GameObject[empty.transform.childCount];

        int enemiesLeft = enemies.Length;
        if(!bossSpawned && enemiesLeft == 6 )
        {
            boss.SetActive(true);
            bossSpawned = true;
        }

        // Check if boss is dead
        if (bossSpawned && boss == null && enemiesLeft == 0)
        {
            // Load the map gen 1 scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial to Stage 1 CS");
        }
    }
}
