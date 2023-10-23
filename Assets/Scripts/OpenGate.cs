using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour {
    [SerializeField] GameObject player;

    public GameObject DialogueManager;

    //Enemy prefab
    public GameObject enemy;

    void Update () {
        //if (DialogueManager.GetComponent<NPCDialogue>.
        if (DialogueManager.GetComponent<NPCDialogue> ().getConversationStatus ()) {
            // an animation or opening sequence should happen here, along with adding mobs
            gameObject.SetActive (false);
            enabled = false; // this is mad sus UPDATE: idk what this does

            // give the player fireball ability deprecated
            //player.gameObject.GetComponent<PlayerMagicSystem> ().enabled = true;

            // Spawn enemies
            Vector3 basePosition = new Vector3 (0, 0.5f, 30);
            for (int i = 0; i < 10; i++) {
                Vector3 position = basePosition + new Vector3 (Random.Range (-5, 5), 0, Random.Range (-5, 5));
                Instantiate (enemy, position, Quaternion.identity);
            }
        }
    }
}