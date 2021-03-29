using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy : Agent
{
    public int attackDamage = -1;

    protected Animator animator;

    public float actionWaitPeriod = 1f;
    [HideInInspector]
    public float actionTimeRemaining;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        actionTimeRemaining = actionWaitPeriod;
    }

    void FixedUpdate()
    {
        // TODO: create a timer for the enemy to move and input all what it's going to do once the timer is over.
        // HINT: remember instance in Player class and also you should be adding in the enemy facing direction (Face() which inherit from agent), Attack(), and Move().
        
    }

    /// <summary>
    /// Calculates the directions the enemy should face
    /// </summary>
    /// <param name="target">An Agent that this should face</param>
    /// <returns>Vector2Int that represents which direction an Agent should face currently</returns>
    public Vector2Int CalculateFaceDirection(Agent target)
    {
        List<Vector2Int> possibleDirectionsToMove = new List<Vector2Int>() { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down };
        Vector2Int delta = new Vector2Int((int)(target.transform.position.x - transform.position.x), (int)(target.transform.position.y - transform.position.y));
        possibleDirectionsToMove.Sort((v1, v2) => (delta - v1).sqrMagnitude.CompareTo((delta - v2).sqrMagnitude));

        return possibleDirectionsToMove[0];
    }

    /// <summary>
    /// Calculates where the enemy should move next
    /// </summary>
    /// <param name="target"></param>
    /// <returns>Vector2Int that represents which direction should the enemy(zombie) move</returns>
    public Vector2Int CalculateMoveDirection(Agent target)
    {
        List<Vector2Int> possibleDirectionsToMove = new List<Vector2Int>() { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down };
        for (int i = 0; i < possibleDirectionsToMove.Count; i++)
        {
            if (!CanMove(possibleDirectionsToMove[i]))
            {
                possibleDirectionsToMove.RemoveAt(i);
                i--;
            }
        }
        // TODO: refer from CalculateFaceDirection and try to extract what should be taken from the said function for this part.

        if (possibleDirectionsToMove.Count == 0)
            return Vector2Int.zero;

        return possibleDirectionsToMove[0];
    }

    /// <summary>
    /// these are just classes that can be overided in the inherited classes.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public abstract bool CanAttack(Agent target);
    public abstract bool Attack(Agent target);
}
