using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HealthPickup : MonoBehaviour, IAgentInteractable
{
    // 
    public int healthRestoreValue = 10;

    /// <summary>
    /// Checks if it's player on same tile as health pickup item
    /// </summary>
    /// <param name="agent">The agent is player</param>
    /// <returns>If agent is the player</returns>

    public bool CanInteract(Agent agent)
    {
        return (agent is Player);
    }

    /// <summary>
    /// Changes player's hp if agent is player and on same tile as pickups item
    /// </summary>
    /// <param name="agent">The agent is player</param>

    public void Interact(Agent agent)
    {
        //If boolean returns false, ends function
        if (!CanInteract(agent))
            return;

        Player player = (Player)agent;

        if (player.currentHp < player.maxHp)
        {
            player.ChangeHpAmount(healthRestoreValue);
            Destroy(this.gameObject);
        }
    }
}
