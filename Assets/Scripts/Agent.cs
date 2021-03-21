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

    /// <summary>
    /// Resets the <c>Agent</c> stats so it can be reused.
    /// </summary>
    public virtual void ResetStats()
    {
        currentHp = maxHp;
    }

    /// <summary>
    /// Move the <c>Agent</c> in given direction.
    /// </summary>
    /// <param name="direction"> Direction in move in. </param>
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

    /// <summary>
    /// Checks if <c>Agent</c> is able to move in a given direction in current conditions.
    /// </summary>
    /// <param name="direction"> Direciton to move in. </param>
    /// <returns> If <c>Agent</c> is able to move in given direction in currenct conditions. </returns>
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

    /// <summary>
    /// Makes this <c>Agent</c> face a given direction.
    /// </summary>
    /// <param name="direction"> Direction to face. </param>
    public void Face(Direction direction)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90 * (int)direction));
    }

    /// <summary>
    /// Alters this <c>Agent</c> <c>currentHp</c>, capped to <c>maxHp</c>.
    /// Calls <c>Die()</c> if <c>currentHp < 0</c>
    /// </summary>
    /// <param name="delta"> Amount to change by. </param>
    public virtual void ChangeHpAmount(int delta)
    {
        currentHp = Mathf.Min(currentHp + delta, maxHp);
        if (currentHp <= 0)
        {
            Die();
        }
    }
    
    /// <summary>
    /// Method called when this <c>Agent</c> dies
    /// </summary>
    public virtual void Die()
    {
        Destroy(this.gameObject);
    }
}
