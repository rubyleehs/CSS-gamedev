using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Agent
{
    public int playerDamage = -1;

    private Transform target;
    private Animator animator;
    private bool inputChanged;//renamed this pls
    private Vector2Int dir;
    private Direction faceDir;
    public int hp = 3;

    int xDir;
    int yDir;

    // Start is called ONCE before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        //target = GameObject.FindGameObjectWithTag("Player").transform;
        target = Player.instance.transform;

        if (Mathf.Abs((int)target.position.x - (int)transform.position.x + (int)target.position.y - (int)transform.position.y) == 1)
        {
            Player player = GetComponent<Player>();

            player.ChangeHealthAmount(playerDamage);
        }

        else if ((int)target.position.x - (int) transform.position.x != 0)
        {
            xDir = (int)target.position.x > (int)transform.position.x ? 1 : -1;
            yDir = 0;
        }
        else
        {
            yDir = (int)target.position.y > (int)transform.position.y ? 1 : -1;
            xDir = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2Int moving = new Vector2Int(xDir, yDir);
        inputChanged = (moving != dir);
        dir = moving;

        if (inputChanged)
        {
            faceDir = base.DirChange(dir, faceDir);

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90 * (int)faceDir));
        }
        
        base.Move(moving);
    }

    internal void TakeDamage(int v)
    {
        hp -= 1;
        if(hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
