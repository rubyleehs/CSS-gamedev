using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Anythign that can move or have Ai
public abstract class Agent : MonoBehaviour
{
    private static Object moveLock = new Object();

    protected Direction currentFaceDir = Direction.East;
    [HideInInspector]
    public int currentHp = 10;
    public int maxHp = 10;

    protected virtual void Start()
    {
        ResetStats();
    }

    public virtual void ResetStats()
    {
        currentHp = maxHp;
    }

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

    protected virtual bool CanMove(Vector2Int direction)
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

    public virtual void ChangeHpAmount(int value)
    {
        currentHp += value;
        if (currentHp <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(this);
    }
}
