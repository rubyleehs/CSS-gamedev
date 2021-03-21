using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour, IAgentInteractable
{
    /// <summary>
    /// Checks if it's player on same tile as ammo pickup item
    /// </summary>
    /// <param name="agent">The agent is player</param>
    /// <returns>Agent is the player</returns>
    public bool CanInteract(Agent agent)
    {
        return (agent is Player);
    }

    /// <summary>
    /// Changes player's ammo count if agent is player and on same tile as pickups item
    /// </summary>
    /// <param name="agent">The agent is player</param>

    public void Interact(Agent agent)
    {
        if (!CanInteract(agent))
            return;

        Player player = (Player)agent;

        if (player.currentAmmo < 5)
        {
            //change player ammo
            player.ChangeAmmoAmount(5); 
            //destroy item
            Destroy(this);
        }
    }
}
