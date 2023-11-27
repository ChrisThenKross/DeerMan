using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    public GameObject gate;
    public GameObject boss;

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        gate.SetActive(true);
        boss.SetActive(true);
    }
}
