using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{

    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] arenaChunks;
    public GameObject[] traversalChunks;

    private int chunkCount = 0;
    private int chunksBeforeArena = 3;
    private List<Vector3> availableTiles = new List<Vector3>();

    private int CHUNK_ROWS = 10;
    private int CHUNK_COLUMNS = 11;
    private Vector2 SPAWN_OFFSET = new Vector2(-4, -4);

    // Stores the level
    private Transform level;

    public void InitLevel() {

        level = new GameObject("Level").transform;
        SpawnArenaChunk();
    }

    public void SpawnArenaChunk() {

        GameObject spawnChunk = arenaChunks[Random.Range(0, arenaChunks.Length)];

        // Generates a new grid of available tiles
        GenerateTilePositions();

        // Randomly selects a tile or wall to use for this chunk
        GameObject spawnedTile = floorTiles[Random.Range(0, floorTiles.Length)];
        GameObject spawnedWall = wallTiles[Random.Range(0, floorTiles.Length)];

        // Removes all preset tiles in the new Chunk from the available tiles
        foreach (Transform preset in spawnChunk.transform) {
            availableTiles.Remove(preset.position);
        }

        // Instantiates the chunk itself
        GameObject chunkInstance = Instantiate(spawnChunk, new Vector3(0, chunkCount * CHUNK_ROWS, 0f), Quaternion.identity) as GameObject;
        chunkInstance.transform.SetParent(level);

        // Automatically fills in floor and wall tiles
        for (int i = 0; i < availableTiles.Count; i++) {

            Vector3 tilePos = availableTiles[i];
            GameObject tileInstance;

            // Checks if its a wall, instantiates a wall if so
            if (tilePos.x < 1 || tilePos.x > 9) {
                tileInstance = Instantiate(spawnedWall, tilePos, Quaternion.identity) as GameObject;
            }
            // Otherwise spawns a floor in
            else {
                tileInstance = Instantiate(spawnedTile, tilePos, Quaternion.identity) as GameObject;
            }

            tileInstance.transform.SetParent(chunkInstance.transform);
        }

        chunkCount++;
    }

    void GenerateTilePositions() {

        availableTiles.Clear();

        for(int x = 0; x < CHUNK_COLUMNS; x++) {
            for (int y = 0; y < CHUNK_ROWS; y++) {
                availableTiles.Add(new Vector3(x + (chunkCount * CHUNK_COLUMNS), y + (chunkCount * CHUNK_ROWS), 0f));
            }
        }
    }

}