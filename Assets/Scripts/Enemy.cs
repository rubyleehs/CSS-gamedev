using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Agent
{
    public int playerDamage = -1;
    public int hp = 3;

    protected Transform target;
    protected Animator animator;
    protected float timer;
    protected float waitTime = 1.0f;

    // Start is called ONCE before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        //target = GameObject.FindGameObjectWithTag("Player").transform;
        target = Player.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        target = Player.instance.transform;
        timer += Time.deltaTime;
        if (timer > waitTime) {

            Vector2Int faceDirection = CalculateFaceDirection();
            if (CanAttack(target))
            {
                Attack(target, faceDirection);
                StartCoroutine(AttackAnim());
            }
            else
                Move(faceDirection);

            timer = 0;
        }
    }

    public virtual void Attack(Transform target, Vector2Int faceDirection)
    {
        animator.SetTrigger("Attacking");
        Player player = target.transform.GetComponent<Player>();

        player.ChangeHealthAmount(playerDamage);

        Face(GetFaceDirection(faceDirection, currentFaceDir));
    }

    public virtual IEnumerator AttackAnim()
    {
        yield return null;
    }

    public Vector2Int CalculateFaceDirection()
    {
        Vector2Int delta = new Vector2Int((int)(target.position.x - transform.position.x), (int)(target.position.y - transform.position.y));

        Vector2Int faceDirection = Vector2Int.zero;

        if (delta.x != 0 && CanMove(Vector2Int.right * (delta.x > 0 ? 1 : -1)))
        {
            faceDirection.x = (int)target.position.x > (int)transform.position.x ? 1 : -1;
        }
        else if (delta.y != 0 && CanMove(Vector2Int.up * (delta.y > 0 ? 1 : -1)))
        {
            faceDirection.y = (int)target.position.y > (int)transform.position.y ? 1 : -1;
        }
        return faceDirection;
    }

    public virtual bool CanAttack(Transform target)
    {
        if (target == null) return false;
        return Mathf.Abs((int)target.position.x - (int)transform.position.x) + Mathf.Abs((int)target.position.y - (int)transform.position.y) == 1;
    }

    internal void TakeDamage(int v)
    {
        hp -= v;
        if(hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
