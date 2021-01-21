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


<<<<<<< HEAD
=======

>>>>>>> parent of 5720c8b... Revert "Additions in Health Pickup"
public class HealthPickup : MonoBehaviour, IPlayerInteractable
{
    bool isFromPlayer = true; //check if is picked up by player?

<<<<<<< HEAD
    public bool CanInteract(Player player)
=======
    public bool CanInteract(Agent player)
>>>>>>> parent of 5720c8b... Revert "Additions in Health Pickup"
    {
        return isFromPlayer;
    }

<<<<<<< HEAD
    public void Interact(Player player)
=======
    public void Interact(Agent player)
>>>>>>> parent of 5720c8b... Revert "Additions in Health Pickup"
    {
        if (!CanInteract(player))
            return;

        if (player is Player)
        {
            Player player = (Player)player;
            player.Changehp(10);
            //change player hp
<<<<<<< HEAD
        }
    }
}
=======




        }
    }
    void Awake()
    {
        player
    }
}

>>>>>>> parent of 5720c8b... Revert "Additions in Health Pickup"
