using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Agent
{
    public int playerDamage = -1;
    public int hp = 3;

    private Transform target;
    private Animator animator;
    private bool dirChanged;
    private Vector2Int dir;
    private Direction faceDir;
    private int xDir;
    private int yDir;
    private float timer;
    private float waitTime = 1.0f;

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
        timer += Time.deltaTime;
        if (timer > waitTime) {
            if (Mathf.Abs((int)target.position.x - (int)transform.position.x + (int)target.position.y - (int)transform.position.y) == 1)
            {
                Player player = target.transform.GetComponent<Player>();

                player.ChangeHealthAmount(playerDamage);
            }
            else if ((int)target.position.x - (int)transform.position.x != 0)
            {
                xDir = (int)target.position.x > (int)transform.position.x ? 1 : -1;
                yDir = 0;
            }
            else
            {
                yDir = (int)target.position.y > (int)transform.position.y ? 1 : -1;
                xDir = 0;
            }

            Vector2Int moving = new Vector2Int(xDir, yDir);
            dirChanged = (moving != dir);
            dir = moving;

            if (dirChanged)
            {
                faceDir = base.DirChange(dir, faceDir);

                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90 * (int)faceDir));
            }
            base.Move(moving);
            timer = 0;
        }
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
