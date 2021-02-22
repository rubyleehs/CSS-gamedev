using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Agent
{
    public int attackDamage = 1;

    protected Animator animator;

    public float actionWaitPeriod = 1f;
    [HideInInspector]
    public float actionTimeRemaining;

    private void Awake ()
    {
        animator = GetComponent<Animator> ();
    }

    void Start ()
    {
        actionTimeRemaining = actionWaitPeriod;
    }

    void Update ()
    {
        actionTimeRemaining -= Time.deltaTime;
        if (actionTimeRemaining <= 0)
        {
            Agent target = Player.instance;

            if (CanAttack (target)) Attack (target);
            else Move (CalculateFaceDirection (target));

            actionTimeRemaining = actionWaitPeriod;
        }
    }

    public Vector2Int CalculateFaceDirection (Agent target)
    {
        List<Vector2Int> possibleDirectionsToMove = new List<Vector2Int> () { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down };
        for (int i = 0; i < possibleDirectionsToMove.Count; i++)
        {
            if (!CanMove (possibleDirectionsToMove[i]))
            {
                possibleDirectionsToMove.RemoveAt (i);
                i--;
            }
        }
        Vector2Int delta = new Vector2Int ((int) (target.transform.position.x - transform.position.x), (int) (target.transform.position.y - transform.position.y));
        possibleDirectionsToMove.Sort ((v1, v2) => (delta - v1).sqrMagnitude.CompareTo ((delta - v2).sqrMagnitude));
        for (int i = 0; i < possibleDirectionsToMove.Count; i++)

            if (possibleDirectionsToMove.Count == 0)
                return Vector2Int.zero;

        return possibleDirectionsToMove[0];
    }

    public abstract bool CanAttack (Agent target);
    public abstract void Attack (Agent target);
}