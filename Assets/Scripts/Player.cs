using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Agent
{

    private Animator animator;
    private int hp, ammo;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // shooting projectile
        if (Input.GetKeyDown("k") && ammo != 0)
        {
            animator = GetComponent<Animator>();
            ammo--;
        }

        int horizontal = (int) (Input.GetAxisRaw("Horizontal"));
        int vertical = (int) (Input.GetAxisRaw("Vertical"));

        if (vertical != 0)
            horizontal = 0;

        Vector2Int direction = new Vector2Int(horizontal, vertical);

        if(vertical != 0 || horizontal != 0) {
            base.Move(direction);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ammo")
        {
            ammo++;

            collision.gameObject.SetActive(false);
        }

        if (collision.tag == "HealthKit")
        {
            hp++;

            collision.gameObject.SetActive(false);
        }

        if (collision.tag == "Hole")
            hp = 0;
    }

    private void Lost()
    {
        if (hp <= 0)
        {
            
        }
    }
}
