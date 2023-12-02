using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour {
    public GameObject DialogueManager;
    public GameObject enemy, enemy2, enemy3;
    public GameObject EnemyContainer;

    void Update () {
        //if (DialogueManager.GetComponent<NPCDialogue>.
        if (DialogueManager.GetComponent<NPCDialogue> ().getConversationStatus ()) 
        {
            // an animation or opening sequence should happen here, along with adding mobs
            gameObject.SetActive (false);
            enabled = false; // this is mad sus UPDATE: idk what this does

            // give the player fireball ability deprecated
            //player.gameObject.GetComponent<PlayerMagicSystem> ().enabled = true;

            // Spawn enemies
            Vector3 basePosition = new Vector3 (0, 0.5f, 30);
            for (int i = 0; i < 5; i++) {
                Vector3 position = basePosition + new Vector3 (Random.Range (-5, 5), 0, Random.Range (-5, 5));
                int rando = Random.Range(0, 3);
                GameObject instance;
                if (rando == 0) instance = Instantiate(enemy, position, Quaternion.identity);
                else if (rando == 2) instance = Instantiate(enemy2, position, Quaternion.identity);
                else instance = Instantiate(enemy3, position, Quaternion.identity);
                instance.transform.parent = EnemyContainer.transform;
            }

            GameObject.Find("EventSystem").GetComponent<EnemyEncounter>().enabled = true;
        }
    }
}