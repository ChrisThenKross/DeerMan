using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArenaEncounter : MonoBehaviour
{
    public GameObject gate;
    private bool isOpen = false;

    void Update()
    {
        // Empty that contains the enemies
        GameObject empty = GameObject.Find("Enemies");
        GameObject[] enemies = new GameObject[empty.transform.childCount];

        int enemiesLeft = enemies.Length;
        Debug.Log(enemiesLeft);
        if (enemiesLeft == 0 & !isOpen)
        {
            OpenGate();
        }
    }

    private void OpenGate()
    {
        gate.SetActive(false);
        isOpen = true;
    }
}
