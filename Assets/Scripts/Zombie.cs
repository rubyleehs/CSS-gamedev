using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    public override void Attack(Transform targe)
    {
        animator.SetTrigger("Attacking");
        Player player = target.transform.GetComponent<Player>();

        player.ChangeHealthAmount(playerDamage);

        Vector2Int faceDirection = CalculateFaceDirection(target);
        if (faceDirection != Vector2Int.zero)
            currentFaceDir = faceDirection.ToEnum();

        Face(currentFaceDir);
    }

    public override void Move(Transform target)
    {
        Move(CalculateFaceDirection(target));
    }

    public override bool CanAttack(Transform target)
    {
        return Mathf.Abs((int)target.position.x - (int)transform.position.x) + Mathf.Abs((int)target.position.y - (int)transform.position.y) == 1;
    }

    public override void TakeDamage(int v)
    {
        hp -= v;
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

}
