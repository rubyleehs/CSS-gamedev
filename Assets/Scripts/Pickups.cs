using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthPickup : MonoBehaviour, IAgentInteractable
{
    public int healthRestoreValue = 10;

    public bool CanInteract (Agent agent)
    {
        return (agent is Player);
    }

    public void Interact (Agent agent)
    {
        if (!CanInteract (agent))
            return;

        Player player = (Player)agent;

        if(player.currentHp<player.maxHp)
        {
            player.TakeDamage(-healthRestoreValue);
            Destroy(this);
        }            
    }
}


public class AmmoPickup : MonoBehaviour, IAgentInteractable
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

        if(player.currentAmmo<5)
        {
            player.ChangeAmmoAmount(5); //change player ammo
            Destroy(this);
        }     
    }
}
