using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Completed
{
    public class PitfallTrap : MonoBehaviour, IAgentInteractable
    {
        /// <summary>
        /// Always return true as all (currently implemented) agents should be able to fall into pitfall traps. 
        /// </summary>
        /// <param name="agent"> Agent to check if it is able to interact with this under current conditions. </param>
        /// <returns> If the agent is able to interact with this. </returns>
        public bool CanInteract(Agent agent)
        {
            return true;
        }

        /// <summary>
        /// Kills any agent interacting with this.
        /// </summary>
        /// <param name="agent">The agent interact with this. </param>
        public void Interact(Agent agent)
        {
            if (!CanInteract(agent))
                return;

            agent.ChangeHpAmount(-agent.currentHp);

            // Ideally, player should have flags for death (aka it knows what kills/damage it)
            // And based on that, lookup what sort of animation should play.
            if (agent is Player)
            {
                ((Player)agent).animator.SetTrigger("FallingInHoleAnim");
            }
        }
    }
}