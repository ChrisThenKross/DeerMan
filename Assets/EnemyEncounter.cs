using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEncounter : MonoBehaviour
{
    int enemiesLeft = 0;
    bool killedAllEnemies = false;
    public GameObject boss;

    void Start()
    {
        enemiesLeft = 5;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesLeft = enemies.Length;
        Debug.Log("There are " + enemiesLeft);
        if(enemiesLeft == 6 )
        {
            boss.SetActive(true);
        }
    }
}
