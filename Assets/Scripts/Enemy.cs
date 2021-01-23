using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Agent
{
    public int playerDamage;

    private Transform target;
    private Animator animator;
    
    int xDir;
    int yDir;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        if ((int)target.position.x - (int) transform.position.x != 0)
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
        Vector2Int Moving = new Vector2Int(xDir, yDir);
        base.Move(Moving);
    }
}
