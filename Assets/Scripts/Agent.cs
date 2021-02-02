using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    East = 0, North = 1, West = 2, South = 3
}

//Anythign that can move or have Ai
public class Agent : MonoBehaviour
{
    private static Object moveLock = new Object();
    //A bunch of code you bring over form player once you start making Enemy class
    protected Direction currentFaceDir = Direction.East;

    public void Move(Vector2Int direction)
    {
        currentFaceDir = GetFaceDirection(direction, currentFaceDir);
        Face(currentFaceDir);

        if (CanMove(direction))
        {

            transform.position += (Vector3Int)direction;
        }

        //Move towards player
        //check the tile i want to move to
        //if the tile is negative outcome. find 2nd closest tile?
    }
    internal bool CanMove(Vector2Int direction)
    {
        lock (moveLock)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 1, ~9);

            if (hits.Length > 0)
            {
                for (int x = 0; x < hits.Length; x++)
                {
                    if (hits[x].transform != this.transform)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    protected Direction GetFaceDirection(Vector2Int targetDirection, Direction defaultDirection)
    {
        if (targetDirection.y > 0)
            return Direction.North;
        else if (targetDirection.y < 0)
            return Direction.South;
        else if (targetDirection.x < 0)
            return Direction.West;
        else if (targetDirection.x > 0)
            return Direction.East;
        else
            return defaultDirection;
    }

    public void Face(Direction direction)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90 * (int)direction));
    }
}
