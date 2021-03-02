using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    public void Start()
    {
        attackDamage = -1;
    }
    public override void Attack (Agent target)
    {
        animator.SetTrigger ("Attacking");
        target.ChangeHpAmount (attackDamage);
    }

    public override bool CanAttack (Agent target)
    {
        //Can attack if player is adjacent
        return Mathf.Abs ((int) target.transform.position.x - (int) transform.position.x) + Mathf.Abs ((int) target.transform.position.y - (int) transform.position.y) == 1;
    }

    public override void ChangeHpAmount (int v)
    {
        currentHp -= v;
        if (currentHp <= 0)
        {
            gameObject.SetActive (false);
        }
    }
}