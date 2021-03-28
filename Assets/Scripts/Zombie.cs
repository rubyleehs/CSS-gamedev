using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Zombie : Enemy
    {
        /// <summary>
        /// Makes this attack the target.
        /// </summary>
        /// <param name="target"> The Agent to attack. </param>
        public override bool Attack(Agent target)
        {
        // TODO: trigger an animation called... and also the sound of... what should be delt to the player...?
            return true;
        }

        /// <summary>
        /// Checks if this can attack the target under current conditions.
        /// </summary>
        /// <param name="target"> The Agent to attack. </param>
        /// <returns> If this is able to attack the target under current conditions. </returns>
        public override bool CanAttack(Agent target)
        {
            // TODO: change the true so that the enemy can attack the player is adjacent else it will just keep on getting the green light to attack
            return true;
        }
    }
