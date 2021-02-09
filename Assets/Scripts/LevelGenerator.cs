using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] battleChunks;
    public GameObject[] obstacleChunks;
    public GameObject[] enemies;

    [HideInInspector] public int chunkCount = 0;
    [HideInInspector] public int chunksDestroyed = 0;

    private int chunksBeforeBattle = 3;
    private List<Vector3> availableTiles = new List<Vector3>();
    private List<Vector3> enemyTiles = new List<Vector3>();
    private List<GameObject> chunkList = new List<GameObject>();

    public int CHUNK_ROWS = 15;
    public int CHUNK_COLUMNS = 15;

    // Stores the level
    public Transform level;

    public void InitLevel(int difficultyLevel) {
        chunkCount = 0;
        chunksDestroyed = 0;
        SpawnChunk(difficultyLevel);
    }

    public void SpawnChunk(int difficultyLevel) {

        int enemiesToSpawn;
        GameObject spawnChunk;

        // Determines type of chunk to spawn and enemy count
        if (chunkCount % chunksBeforeBattle == 0) {
            spawnChunk = battleChunks[Random.Range(0, battleChunks.Length)];
            enemiesToSpawn = difficultyLevel;
        }
        else {
            spawnChunk = obstacleChunks[Random.Range(0, obstacleChunks.Length)];
            enemiesToSpawn = (int)Mathf.Ceil(Mathf.Log(difficultyLevel, 2));
        }

        // Generates a new grid of available tiles minus the wall tiles
        GenerateTilePositions(spawnChunk);

        // Instantiates the chunk itself
        GameObject chunkInstance = Instantiate(spawnChunk, new Vector3(0, chunkCount * CHUNK_ROWS, 0f), Quaternion.identity) as GameObject;
        chunkInstance.transform.SetParent(level);
        chunkList.Add(chunkInstance);

        // Gets the entity list of the spawned chunk
        Transform entityList = chunkInstance.transform.Find("Entities");

        // Spawns in enemies
        GenerateEnemyPositions(enemiesToSpawn);
        for (int i = 0; i < enemyTiles.Count; i++) {

            Vector3 enemyPos = enemyTiles[i];
            GameObject spawnedEnemy = enemies[Random.Range(0, enemies.Length)];

            enemyPos.y += (chunkCount * CHUNK_ROWS);

            GameObject enemyInstance = Instantiate(spawnedEnemy, enemyPos, Quaternion.identity) as GameObject;

            enemyInstance.transform.SetParent(entityList);
            
        }

        chunkCount++;
    }

    // Destroys the earliest chunk in the list.
    public void DestroyEarliestChunk() {
        Destroy(chunkList[0]);
        chunkList.RemoveAt(0);
        chunksDestroyed++;
    }

    // Destroys everything in the level GameObject
    public void DestroyLevel() {
        foreach (Transform child in level) {
            GameObject.Destroy(child.gameObject);
        }
    }

    // Generates tile positions for generation in chunks
    void GenerateTilePositions(GameObject spawnChunk) {
        
        availableTiles.Clear();

        // Gets the walls from the chunk to be spawned
        GameObject walls= spawnChunk.transform.Find("Walls").gameObject;
        Tilemap wallTilemap = walls.GetComponent<Tilemap>();

        for (int x = 0; x < CHUNK_COLUMNS; x++) {
            for (int y = 0; y < CHUNK_ROWS; y++) {
                // If the tile does not exist in the wall tilemap, add it into list of available tiles
                Vector3Int tileTBAdded = new Vector3Int(x, y, 0);
                if (!wallTilemap.HasTile(tileTBAdded))
                    availableTiles.Add((Vector3)tileTBAdded);
            }
        }

    }

    // Generates enemy positions from available tiles
    void GenerateEnemyPositions(int enemiesToSpawn) {

        enemyTiles.Clear();
 
        for(int i = 0; i < enemiesToSpawn; i++) {

            // Gets a random tile 
            Vector3 enemySpawnLocation = availableTiles[Random.Range(0, availableTiles.Count)];
            
            enemyTiles.Add(enemySpawnLocation);
           
            // Removes tile from pool
            availableTiles.Remove(enemySpawnLocation);
        }
    
    }

}