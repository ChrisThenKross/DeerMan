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

    public void UpdateEnemies(int[,] map){
        for(int i = 0; i < enemies.Length; i++){
            if(enemies[i] != null && enemies[i].GetComponent<EnemyAI>().IsDead()){
                Destroy(enemies[i]);
                enemies[i] = null;
            }

            if(enemies[i] == null){
                Debug.Log("Spawning enemy " + i);
                int x, y;
                GetRandomValidPosition(map, out x, out y);
                enemies[i] = Instantiate(enemyPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }

    private void GetRandomPosition(int[,] map, out int x, out int y){
        x = Random.Range(0, map.GetLength(0));
        y = Random.Range(0, map.GetLength(1));
    }

    private void GetRandomValidPosition(int[,] map, out int x, out int y){
        GetRandomPosition(map, out x, out y);
        while(map[x,y] != 0){
            GetRandomPosition(map, out x, out y);
        }
    }
}
