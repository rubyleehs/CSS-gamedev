using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    public override void Attack(Agent target)
    {
        animator.SetTrigger("Attacking");
        target.TakeDamage(attackDamage);

        Vector2Int faceDirection = CalculateFaceDirection(target);
        if (faceDirection != Vector2Int.zero)
            currentFaceDir = faceDirection.ToEnum();

        Face(currentFaceDir);
    }

    public override bool CanAttack(Agent target)
    {
        //Can attack if player is adjacent
        return Mathf.Abs((int)target.transform.position.x - (int)transform.position.x) + Mathf.Abs((int)target.transform.position.y - (int)transform.position.y) == 1;
    }

    public override void TakeDamage(int v)
    {
        currentHp -= v;
        if (currentHp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
