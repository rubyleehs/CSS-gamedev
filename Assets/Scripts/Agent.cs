using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Anythign that can move or have Ai
public class Agent : MonoBehaviour
{
    //A bunch of code you bring over form player once you start making Enemy class

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2d;

    public void Move(Vector2Int direction)
    {
        transform.position += (Vector3Int)direction;

        //Move towards player
        //check the tile i want to move to
        //if the tile is negative outcome. find 2nd closest tile?     
    }

    internal Direction DirChange(Vector2Int inputDir, Direction dir)
    {
        if (inputDir.y > 0)
            return Direction.North;
        else if (inputDir.y < 0)
            return Direction.South;
        else if (inputDir.x < 0)
            return Direction.West;
        else if (inputDir.x > 0)
            return Direction.East;

        return dir;
    }
}
