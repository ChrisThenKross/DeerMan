using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPhase1 : MonoBehaviour
{
    public Animator anim;
    void Update()
    {
        int left = gameObject.transform.childCount;
        Debug.Log(left);
        if (left == 0) Phase2();
    }


    private void Phase2()
    {
        anim.SetTrigger("gatesDown");
        Destroy(gameObject, 2);
    }



}
