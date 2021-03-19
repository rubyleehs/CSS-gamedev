using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    /// <summary>
    /// This sets an animation and deals damage (Change the health stats) on the player
    /// </summary>
    /// <param name="target"></param>
    public override void Attack (Agent target)
    {
        animator.SetTrigger ("Attacking");
        target.ChangeHpAmount (-attackDamage);
    }

    /// <summary>
    /// This class determines if the zombie could attack the player (next to it)
    /// </summary>
    /// <param name="target"></param>
    /// <returns>A boolean which makes it true or false if it is next to the player</returns>
    public override bool CanAttack (Agent target)
    {
        //Can attack if player is adjacent
        return Mathf.Abs ((int) target.transform.position.x - (int) transform.position.x) + Mathf.Abs ((int) target.transform.position.y - (int) transform.position.y) == 1;
    }

    /// <summary>
    /// this class deals the ammount of damage to the player.
    /// </summary>
    /// <param name="v"></param>
    public override void ChangeHpAmount (int v)
    {
        currentHp -= v;
        if (currentHp <= 0)
        {
            gameObject.SetActive (false);
        }
    }
}