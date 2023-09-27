using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    public GameObject DialogueManager;

    void Update()
    {
        //if (DialogueManager.GetComponent<NPCDialogue>.
        if (DialogueManager.GetComponent<NPCDialogue>().getConversationStatus())
        {
            // an animation or opening sequence should happen here, along with adding mobs
            gameObject.SetActive(false);
            enabled = false; // this is mad sus
        }
    }
}
