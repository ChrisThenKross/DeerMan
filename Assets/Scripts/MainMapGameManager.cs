using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMapGameManager : MonoBehaviour
{
    public int MaxEnemyCount = 20;

    public GameObject enemyPrefab;
    private GameObject [] enemies;

    public void Start(){
        enemies = new GameObject[MaxEnemyCount];
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
                enemies[i] = Instantiate(enemyPrefab, new Vector3(x, .5f, z), Quaternion.identity);
            }
        }
    }

    private void GetRandomPosition(TileType[,] map, out float x, out float z){
        x = Random.Range(0, map.GetLength(0));
        z = Random.Range(0, map.GetLength(1));
    }

    private void GetRandomValidPosition(TileType[,] map, float squareSize, out float x, out float z){
        GetRandomPosition(map, out x, out z);
        while(map[(int)x,(int)z] != 0){
            GetRandomPosition(map, out x, out z);
        }

        // Center position over center of maze
        x -= map.GetLength(0) * squareSize / 2f;
        z -= map.GetLength(1) * squareSize / 2f;
    }
}
