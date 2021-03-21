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
        if (!(agent is Player)) return false;

        Player player = (Player)agent;
        return (player.currentHp < player.maxHp);
    }

    /// <summary>
    /// Changes the given <c>Player</c> health by <c>healthRestoreValue</c>.
    /// </summary>
    /// <param name="agent">The <c>Agent</c> to give health to.</param>
    public void Interact(Agent agent)
    {
        if (!CanInteract(agent))
            return;

        if (agent is Player)
        {
            Player player = (Player)agent;
            
            player.ChangeHpAmount(healthRestoreValue);
            Destroy(this.gameObject);
        }
    }
}
