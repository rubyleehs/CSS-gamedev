using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
           //Change HP to -currentHp
           //HINT: Use agent.ChangeHpAmount
        }
    }

