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

                // Put enemy under GameManager
                enemies[i].transform.parent = transform;
            }
        }
    }

    private void GetRandomPosition(TileType[,] map, out float x, out float z){
        x = Random.Range(0, map.GetLength(0));
        z = Random.Range(0, map.GetLength(1));
    }

    private void GetRandomValidPosition(TileType[,] map, float squareSize, out float x, out float z){
        GetRandomPosition(map, out x, out z);
        System.Func<Vector2, bool> checkValid = (Vector2 pos) =>       {
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
}
