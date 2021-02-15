using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Agent
{
    public int playerDamage = -1;
    public int hp = 3;

    protected Transform target;
    protected Animator animator;

    public float actionWaitPeriod = 1f;
    [HideInInspector]
    public float actionTimeRemaining;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        target = Player.instance.transform;
        actionTimeRemaining = actionWaitPeriod;
    }

    void Update()
    {
        actionTimeRemaining -= Time.deltaTime;
        if (actionTimeRemaining <= 0)
        {
            target = Player.instance.transform;

            if (CanAttack(target)) Attack(target);
            else Move(target);

            actionTimeRemaining = actionWaitPeriod;
        }
    }

    public Vector2Int CalculateFaceDirection(Transform target)
    {
        Vector2Int delta = new Vector2Int((int)(target.position.x - transform.position.x), (int)(target.position.y - transform.position.y));

        Vector2Int faceDirection = Vector2Int.zero;

        if (delta.y > delta.x)
        {
            if (delta.x != 0 && CanMove(Vector2Int.right * (delta.x > 0 ? 1 : -1)))
            {
                faceDirection.x = (int)target.position.x > (int)transform.position.x ? 1 : -1;
            }
            else if (delta.y != 0 && CanMove(Vector2Int.up * (delta.y > 0 ? 1 : -1)))
            {
                faceDirection.y = (int)target.position.y > (int)transform.position.y ? 1 : -1;
            }
        }
        else
        {
            if (delta.y != 0 && CanMove(Vector2Int.up * (delta.y > 0 ? 1 : -1)))
            {
                faceDirection.y = (int)target.position.y > (int)transform.position.y ? 1 : -1;
            }
            else if (delta.x != 0 && CanMove(Vector2Int.right * (delta.x > 0 ? 1 : -1)))
            {
                faceDirection.x = (int)target.position.x > (int)transform.position.x ? 1 : -1;
            }
        }
        return faceDirection;
    }

    public abstract bool CanAttack(Transform target);
    public abstract void Attack(Transform target);
    public abstract void Move(Transform target);
    public abstract void TakeDamage(int delta); 
}
