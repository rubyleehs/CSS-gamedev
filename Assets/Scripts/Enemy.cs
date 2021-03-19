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

    private void Awake ()
    {
        animator = GetComponent<Animator> ();
    }

    protected override void Start ()
    {
        base.Start();
        actionTimeRemaining = actionWaitPeriod;
    }

    void Update ()
    {
        actionTimeRemaining -= Time.deltaTime;
        if (actionTimeRemaining <= 0)
        {
            Agent target = Player.instance;

            if (CanAttack(target))
            {
                Face(CalculateFaceDirection(target).ToEnum());
                Attack(target);
            }
            else Move(CalculateMoveDirection(target));

            actionTimeRemaining = actionWaitPeriod;
        }
    }

    /// <summary>
    /// Calculates the directions the enemy should face
    /// </summary>
    /// <param name="target">An Agent that this should face</param>
    /// <returns>Vector2Int that represents which direction an Agent should face currently</returns>
    public Vector2Int CalculateFaceDirection (Agent target)
    {
        List<Vector2Int> possibleDirectionsToMove = new List<Vector2Int> () { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down };
        Vector2Int delta = new Vector2Int ((int) (target.transform.position.x - transform.position.x), (int) (target.transform.position.y - transform.position.y));
        possibleDirectionsToMove.Sort ((v1, v2) => (delta - v1).sqrMagnitude.CompareTo ((delta - v2).sqrMagnitude));

        return possibleDirectionsToMove[0];
    }

    public Vector2Int CalculateMoveDirection(Agent target)
    {
        List<Vector2Int> possibleDirectionsToMove = new List<Vector2Int>() { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down };
        for (int i = 0; i < possibleDirectionsToMove.Count; i++)
        {
            if (!CanMove(possibleDirectionsToMove[i]))
            {
                possibleDirectionsToMove.RemoveAt (i);
                i--;
            }
        }
        Vector2Int delta = new Vector2Int((int)(target.transform.position.x - transform.position.x), (int)(target.transform.position.y - transform.position.y));
        possibleDirectionsToMove.Sort((v1, v2) => (delta - v1).sqrMagnitude.CompareTo((delta - v2).sqrMagnitude));

        if (possibleDirectionsToMove.Count == 0)
            return Vector2Int.zero;

        return possibleDirectionsToMove[0];
    }

    public abstract bool CanAttack (Agent target);
    public abstract void Attack (Agent target);
}