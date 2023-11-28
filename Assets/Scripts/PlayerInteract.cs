using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out MainMapNPCInteract mainMapInteractable))
                {
                    mainMapInteractable.Interact();
                    return;
                }
                if (collider.TryGetComponent(out NPCInteract npcInteractable))
                {
                    npcInteractable.Interact();
                }
            }
        }
    }
}
