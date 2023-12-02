using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMapGameManager : MonoBehaviour
{
    public int MaxEnemyCount = 20;

    public GameObject enemyPrefab1, enemyPrefab2, enemyPrefab3;
    private int enemiesKilled;
    private GameObject [] enemies;
    [SerializeField] private TMP_Text enemiesLeft;

    public void Start(){
        enemiesKilled = -MaxEnemyCount; // Offset for the first time UpdateEnemies is called
        enemies = new GameObject[MaxEnemyCount];
    }

    public void Update(){ 
        GameObject dialogueManager = GameObject.Find("DialogueManager");
        NPCDialogue npcDialogue = dialogueManager.GetComponent<NPCDialogue>();
        if(npcDialogue.getConversationStatus()){
            Debug.Log("Loading next scene");
            //UnityEngine.SceneManagement.SceneManager.LoadScene(nextBossFight[0]);
        }
        enemiesLeft.text = ("I have killed " + enemiesKilled + "/12 disgusting human vermin!");
        if (enemiesKilled >= 12) //Set this to 0 or 1 for testing purposes
            if (SceneManager.GetActiveScene().name == "Map Gen 1")
                SceneManager.LoadScene("Stage 1 Boss Intro");
            else if (SceneManager.GetActiveScene().name == "Map Gen 2")
                SceneManager.LoadScene("Stage 2 Boss Intro");
            else if (SceneManager.GetActiveScene().name == "Map Gen 3")
                SceneManager.LoadScene("Stage 3 Boss Intro");
    }

    public void UpdateEnemies(TileType[,] map, float squareSize){
        for(int i = 0; i < enemies.Length; i++){
            if(enemies[i] != null && enemies[i].GetComponent<EnemyAI>().IsDead()){
                Destroy(enemies[i]);
                enemies[i] = null;
            }

            if(enemies[i] == null){
                float x, z;
                GetRandomValidPosition(map, squareSize, out x, out z);

                int rando = Random.Range(0, 3);

                if(rando == 0) enemies[i] = Instantiate(enemyPrefab1, new Vector3(x, .5f, z), Quaternion.identity);
                else if (rando == 2) enemies[i] = Instantiate(enemyPrefab2, new Vector3(x, .5f, z), Quaternion.identity);
                else enemies[i] = Instantiate(enemyPrefab3, new Vector3(x, .5f, z), Quaternion.identity);

                // Put enemy under GameManager
                enemies[i].transform.parent = transform;
                enemiesKilled++;    
            }
        }
    }

    private void GetRandomPosition(TileType[,] map, out float x, out float z){
        x = Random.Range(0, map.GetLength(0));
        z = Random.Range(0, map.GetLength(1));
    }

    private void GetRandomValidPosition(TileType[,] map, float squareSize, out float x, out float z){
        GetRandomPosition(map, out x, out z);
        System.Func<Vector2, bool> checkValid = (Vector2 pos) => {
            for (int i = -1; i <= 1; i++){
                for (int j = -1; j <= 1; j++){
                    int x = (int)pos.x + i;
                    int y = (int)pos.y + j;
                    if (x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1)){
                        if (map[x, y] == TileType.Wall){
                            return false;
                        }
                    }
                }
            }

            return true;
        };

        while(!checkValid(new Vector2(x, z))){
            GetRandomPosition(map, out x, out z);
        }

        // Center position over center of maze
        x -= map.GetLength(0) * squareSize / 2f;
        z -= map.GetLength(1) * squareSize / 2f;
    }


    public int GetEnemiesKilled(){
        return enemiesKilled;
    }

    public int GetMaxEnemies(){
        return MaxEnemyCount;
    }
}
