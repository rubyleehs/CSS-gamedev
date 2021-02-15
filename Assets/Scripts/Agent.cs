using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Anythign that can move or have Ai
public class Agent : MonoBehaviour
{
    private static Object moveLock = new Object();
    //A bunch of code you bring over form player once you start making Enemy class
    protected Direction currentFaceDir = Direction.East;

    public virtual void Move(Vector2Int direction)
    {
        if (direction != Vector2Int.zero)
            currentFaceDir = direction.ToEnum();

        Face(currentFaceDir);

        if (CanMove(direction))
        {
            transform.position += (Vector3Int)direction;
        }
    }

    protected bool CanMove(Vector2Int direction)
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

    public void Face(Direction direction)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90 * (int)direction));
    }
}
