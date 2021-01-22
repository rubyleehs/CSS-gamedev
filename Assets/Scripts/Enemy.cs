using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Agent
{
    public int playerDamage;

    private Transform target;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        int xDir = 0;
        int yDir = 0;

        animator = GetComponent<Animator>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        if ((int)target.position.x - (int) transform.position.x != 0)
        {
            xDir = (int)target.position.x > (int)transform.position.x ? 1 : -1;
        }
        else
        {
            yDir = (int)target.position.y > (int)transform.position.y ? 1 : -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
