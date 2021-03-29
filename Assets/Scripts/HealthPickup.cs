using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class HealthPickup : MonoBehaviour, IAgentInteractable
    {
        public int healthRestoreValue = 10;

        /// <summary>
        /// Checks if the given <c>Agent</c> is able to interact with this in current conditions.
        /// Currently, only <c>Player</c> is able to interact.
        /// </summary>
        /// <param name="agent"> The <c>Agent</c> to check. </param>
        /// <returns> If the <c>Agent</c> is able to interact with this. </returns>

        public bool CanInteract(Agent agent) 
        {
        //TODO: If agent IS NOT a Player, return false.
        return false;
        //TODO: Otherwise, agent is player
        //HINT: Use (Player)agent
        //return (player.currentHp < player.maxHp);
    }

        /// <summary>
        /// Changes the given <c>Player</c> health by <c>healthRestoreValue</c>.
        /// </summary>
        /// <param name="agent">The <c>Agent</c> to give health to.</param>
        public void Interact(Agent agent)
        {
            //If agent IS a Player
            //Agent is player
            //HINT: Use (Player)agent
            //Change Player's Hp based on variable given
            
            Destroy(this.gameObject);
        }
    }

