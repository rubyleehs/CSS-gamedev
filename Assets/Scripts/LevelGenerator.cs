using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{

    public GameObject[] chunkList;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;

    private chunkCount = 0;

    // This whole script is TODO
    void AddChunk(){

        GameObject newChunk = chunkList[Random.Range(0, chunkList.Length)];
        GameObject newChunkInstance = Instantiate(newChunk, new Vector3())

    }

    void DeleteChunk() {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
