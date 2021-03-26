using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;

/// <summary>
/// Class for generating levels.
/// OOP Challenge:
/// GameManager is currently very tightly coupled with this.
/// Try resolving it.
/// </summary>
public class LevelGenerator : MonoBehaviour
{
    public GameObject[] battleChunks;
    public GameObject[] obstacleChunks;
    public GameObject[] enemies;
    public GameObject[] pickups;

    [HideInInspector] public int chunkCount = 0;
    [HideInInspector] public int chunksDestroyed = 0;

    private int chunksBeforeBattle = 3;
    private List<Vector3> availableTiles = new List<Vector3> ();
    private List<Vector3> entityTiles = new List<Vector3> ();
    private List<GameObject> chunkList = new List<GameObject> ();

    public int CHUNK_ROWS = 15;
    public int CHUNK_COLUMNS = 15;

    // Stores the level
    public Transform level;

    /// <summary>
    /// Initializes the level. Spawns an initial chunk. 
    /// </summary>
    /// <param name="difficultyLevel">The initial difficulty of the game.</param>
    public void InitLevel (int difficultyLevel)
    {
        chunkCount = 0;
        chunksDestroyed = 0;
        SpawnChunk (difficultyLevel);
    }

    /// <summary>
    /// Spawns a new level chunk ahead of the player.
    /// </summary>
    /// <param name="difficultyLevel">Difficulty level of the chunk to be spawned. 
    /// Enemies are more numerous as difficulty increases.</param>
    public void SpawnChunk (float difficultyLevel)
    {

        int enemiesToSpawn;
        GameObject spawnChunk;

        // Determines type of chunk to spawn and enemy count
        if (chunkCount % chunksBeforeBattle == 0)
        {
            spawnChunk = battleChunks[Random.Range (0, battleChunks.Length)];
            enemiesToSpawn = (int) difficultyLevel;
        }
        else
        {
            spawnChunk = obstacleChunks[Random.Range (0, obstacleChunks.Length)];
            enemiesToSpawn = (int) Mathf.Ceil (Mathf.Log (difficultyLevel, 2));
        }

        // Generates a new grid of available tiles minus the wall tiles
        GenerateTilePositions (spawnChunk);

        // Instantiates the chunk itself
        GameObject chunkInstance = Instantiate (spawnChunk, new Vector3 (0, chunkCount * CHUNK_ROWS, 0f), Quaternion.identity) as GameObject;
        chunkInstance.transform.SetParent (level);
        chunkList.Add (chunkInstance);

        // Gets the entity list of the spawned chunk
        Transform entityList = chunkInstance.transform.Find ("Entities");

        // TODO: Decides how many pickups to spawn, this is a placeholder
        int pickupsToSpawn = 1;

        // Generates positions for spawning enemies and pickups.
        GenerateEntityPositions (enemiesToSpawn + pickupsToSpawn);

        // Spawns in enemies
        for (int i = 0; i < enemiesToSpawn; i++)
        {

            Vector3 enemyPos = entityTiles[i];
            GameObject spawnedEnemy = enemies[Random.Range (0, enemies.Length)];

            enemyPos.y += (chunkCount * CHUNK_ROWS);

            GameObject enemyInstance = Instantiate (spawnedEnemy, enemyPos, Quaternion.identity) as GameObject;

            enemyInstance.transform.SetParent (entityList);

        }

        // Spawns in pickups
        for (int i = enemiesToSpawn; i < entityTiles.Count; i++)
        {

            Vector3 pickupPos = entityTiles[i];
            GameObject spawnedPickup = pickups[Random.Range (0, enemies.Length)];

            pickupPos.y += (chunkCount * CHUNK_ROWS);

            GameObject pickupInstance = Instantiate (spawnedPickup, pickupPos, Quaternion.identity) as GameObject;

            pickupInstance.transform.SetParent (entityList);
        }

        chunkCount++;
    }

    /// <summary>
    /// Destroys the rearmost chunk behind the player.
    /// This is done to avoid clutter.
    /// </summary>
    public void DestroyEarliestChunk ()
    {
        Destroy (chunkList[0]);
        chunkList.RemoveAt (0);
        chunksDestroyed++;
    }

    /// <summary>
    /// Destroys the entire level.
    /// </summary>
    public void DestroyLevel ()
    {
        foreach (Transform child in level)
        {
            GameObject.Destroy (child.gameObject);
        }
    }

    /// <summary>
    /// Determines where entities can spawn within a chunk by scanning for walls.
    /// Open tile locations are temporarily stored in the availableTiles list.
    /// </summary>
    /// <param name="spawnChunk">The chunk to scan for spawnpoints.</param>
    void GenerateTilePositions (GameObject spawnChunk)
    {
        availableTiles.Clear ();

        // Gets the walls from the chunk to be spawned
        GameObject walls = spawnChunk.transform.Find ("Walls").gameObject;
        Tilemap wallTilemap = walls.GetComponent<Tilemap> ();

        for (int x = 0; x < CHUNK_COLUMNS; x++)
        {
            for (int y = 0; y < CHUNK_ROWS; y++)
            {
                // If the tile does not exist in the wall tilemap, add it into list of available tiles
                Vector3Int tileTBAdded = new Vector3Int (x, y, 0);
                if (!wallTilemap.HasTile (tileTBAdded))
                    availableTiles.Add ((Vector3) tileTBAdded);
            }
        }
    }

    /// <summary>
    /// Randomly generates entity spawnpoints from available tiles.
    /// Tiles to be spawned are temporarily stored in the entityTiles list.
    /// </summary>
    /// <param name="entitiesToSpawn">The number of entities to spawn</param>
    void GenerateEntityPositions (int entitiesToSpawn)
    {
        entityTiles.Clear ();

        for (int i = 0; i < entitiesToSpawn; i++)
        {

            // Gets a random tile 
            Vector3 entitySpawnLocation = availableTiles[Random.Range (0, availableTiles.Count)];

            entityTiles.Add (entitySpawnLocation);

            // Removes tile from pool
            availableTiles.Remove (entitySpawnLocation);
        }
    }
}