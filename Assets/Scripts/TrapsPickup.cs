using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPickup : MonoBehaviour, IAgentInteractable
{
    public bool CanInteract(Agent agent)
    {
        return (agent is Player);
    }

    public void Interact(Agent agent)
    {
        if (!CanInteract(agent))
            return;

        Player player = (Player)agent;

        player.ChangeHpAmount(-player.currentHp);
        player.animator.SetTrigger("FallingInHoleAnim");
    }
}