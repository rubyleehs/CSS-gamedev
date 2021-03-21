using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPickup : MonoBehaviour, IAgentInteractable
{
    /// <summary>
    /// Checks if it's player on same tile as traps pickup item
    /// </summary>
    /// <param name="agent">The agent is player</param>
    /// <returns>If agent is the player</returns>

    public bool CanInteract(Agent agent)
    {
        return (agent is Player);
    }

    /// <summary>
    /// Changes Player ammo if agent is player and on same tile as traps pickup item
    /// </summary>
    /// <param name="agent">The agent is player</param>
    /// <returns>If agent is the player</returns>
    
    public void Interact(Agent agent)
    {
        if (!CanInteract(agent))
            return;

        Player player = (Player)agent;

        //kills player
        player.ChangeHpAmount(-player.currentHp);
        player.animator.SetTrigger("FallingInHoleAnim");
    }
}