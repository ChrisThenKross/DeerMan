using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerPhase1 : MonoBehaviour
{
    public Animator anim;
    [SerializeField] private TMP_Text storyText;
    void Update()
    {
        int left = gameObject.transform.childCount;
        Debug.Log(left);
        if (left == 0) Phase2();
    }


    private void Phase2()
    {
        Debug.Log("dates down");
        anim.SetTrigger("gatesDown");
        storyText.text = "Gates came down! Time to attack!";
        Destroy(gameObject);
    }



}
