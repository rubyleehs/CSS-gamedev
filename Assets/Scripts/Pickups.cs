using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour, IPlayerInteractable
{
    bool isStillDangerous = true;

    public bool CanInteract(Agent agent)
    {
        return isStillDangerous;
    }

    public void Interact(Agent agent)
    {
        if (!CanInteract(agent))
            return; 

        if (agent is Player)
        {
            Player player = (Player)agent;
            player.ChangeAmmoAmount(10);
            //change player hp
        }
    }
}


public class HealthPickup : MonoBehaviour, IAgentInteractable
{
    bool isFromPlayer = true; //check if is picked up by player

    public bool CanInteract(Agent agent);
    {
     return (agent is Player);
    }
   public void Interact(Player player)
    {
        if (!CanInteract(player))
            return;

        if (player is Player)
        {
            Player player = (Player)player;
            player.Changehp(10);
            //change player hp
        }
    }
}



public class AmmoPickup : MonoBehaviour, IPlayerInteractable
{
    bool isFromPlayer = true; //check if is picked up by player

    public bool CanInteract(Player player) => isFromPlayer;

    public void Interact(Player player)
    {
        if (!CanInteract(player))
            return;

        if (player is Player)
        {
            Player player = (Player)player;
            player.Changeammo(5);
            //change player ammo
        }
    }
}