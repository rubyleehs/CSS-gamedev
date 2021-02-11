using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : Agent
{
    public int playerDamage = -2;

    private Transform target;
    private Animator animator;
    private float timer;
    private float waitTime = 2.0f;
    private bool loaded = false;

    protected Direction currentFaceDir = Direction.East;

    // Start is called ONCE before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        //target = GameObject.FindGameObjectWithTag("Player").transform;
        target = Player.instance.transform;
    }

    // Update is called once per frame
    void Update()
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
        currentFaceDir = GetFaceDirection(faceDirection, currentFaceDir);
        Face(currentFaceDir);

        timer += Time.deltaTime;
        if (timer > waitTime && !loaded)
        {
            animator.SetTrigger("Loading");

            loaded = true;

            timer = 0;
        }

        if (CanAttack(target) && loaded)
        {
            animator.SetTrigger("Shooting");

            loaded = false;

            Player player = target.transform.GetComponent<Player>();

            player.ChangeHealthAmount(playerDamage);

            animator.SetTrigger("Shot");
        }
    }

    internal bool CanAttack(Transform target)
    {
        return Mathf.Abs((int)target.position.x - (int)transform.position.x) == 0 || Mathf.Abs((int)target.position.y - (int)transform.position.y) == 0;
    }
}
