using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class AmmoPickup : MonoBehaviour, IAgentInteractable
    {
        public int ammoRestoreValue;

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
        //return (player.currentAmmo < player.maxAmmo);
        }

        /// <summary>
        /// Changes the given <c>Player</c> ammo by <c>ammoRestoreValue</c>.
        /// </summary>
        /// <param name="agent">The <c>Agent</c> to give ammo to.</param>
        public void Interact(Agent agent)
        {
            //If agent IS a Player
            //Agent is player
            //HINT: Use (Player)agent
            //Change Player's Hp based on variable given
        
            Destroy(this.gameObject);
        }
    }

