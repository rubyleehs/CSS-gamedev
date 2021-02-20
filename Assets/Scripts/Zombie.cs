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
        Vector2Int delta = new Vector2Int((int)(target.position.x - transform.position.x), (int)(target.position.y - transform.position.y));

        Vector2Int direction = Vector2Int.zero;

        if (delta.y > delta.x)
        {
            if (delta.x != 0 && CanMove(Vector2Int.right * (delta.x > 0 ? 1 : -1)))
            {
                direction.x = (int)target.position.x > (int)transform.position.x ? 1 : -1;
            }
            else if (delta.y != 0 && CanMove(Vector2Int.up * (delta.y > 0 ? 1 : -1)))
            {
                direction.y = (int)target.position.y > (int)transform.position.y ? 1 : -1;
            }
        }
        else
        {
            if (delta.y != 0 && CanMove(Vector2Int.up * (delta.y > 0 ? 1 : -1)))
            {
                direction.y = (int)target.position.y > (int)transform.position.y ? 1 : -1;
            }
            else if (delta.x != 0 && CanMove(Vector2Int.right * (delta.x > 0 ? 1 : -1)))
            {
                direction.x = (int)target.position.x > (int)transform.position.x ? 1 : -1;
            }
        }

        Move(direction);
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
