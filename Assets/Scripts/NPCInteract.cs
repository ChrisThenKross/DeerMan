using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    public GameObject dialogueManager;

    public void Interact()
    {
        dialogueManager.GetComponent<NPCDialogue>().StartConversation();
    }
}
