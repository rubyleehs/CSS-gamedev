using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Completed
{
    public class Zombie : Enemy
    {
        /// <summary>
        /// Makes this attack the target.
        /// </summary>
        /// <param name="target"> The Agent to attack. </param>
        public override bool Attack(Agent target)
        {
            animator.SetTrigger("Attacking");
            attackSFX.Play();
            target.ChangeHpAmount(-attackDamage);
            return true;
        }

        /// <summary>
        /// Checks if this can attack the target under current conditions.
        /// </summary>
        /// <param name="target"> The Agent to attack. </param>
        /// <returns> If this is able to attack the target under current conditions. </returns>
        public override bool CanAttack(Agent target)
        {
            //Can attack if player is adjacent
            return Mathf.Abs((int)target.transform.position.x - (int)transform.position.x) + Mathf.Abs((int)target.transform.position.y - (int)transform.position.y) == 1;
        }
    }
}