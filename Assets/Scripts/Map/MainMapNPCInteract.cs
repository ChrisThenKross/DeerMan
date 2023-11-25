using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMapNPCInteract : NPCInteract
{
    public TextMeshProUGUI textDisplay;

    public int enemiesKilledToProgessRatio = 2;

    public new void Interact()
    {
        GameObject gameManager = GetGameManager();
        int enemiesKilled = gameManager.GetComponent<MainMapGameManager>().GetEnemiesKilled();
        int maxEnemies = gameManager.GetComponent<MainMapGameManager>().GetMaxEnemies();
        if(enemiesKilled < maxEnemies * enemiesKilledToProgessRatio){
            textDisplay.text = $"{enemiesKilled} out of {maxEnemies * enemiesKilledToProgessRatio} enemies killed to progress";

            // Remove the text after 3 seconds
            StartCoroutine(RemoveTextAfterSeconds(3));
            return;
        }

        Debug.Log("Starting conversation");
        dialogueManager.GetComponent<NPCDialogue>().StartConversation();
    }

    IEnumerator RemoveTextAfterSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        textDisplay.text = "";
    }

    public GameObject GetGameManager()
    {
        return GameObject.Find("GameManager");
    }
}
