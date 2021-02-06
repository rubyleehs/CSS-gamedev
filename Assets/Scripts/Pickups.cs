using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthPickup : MonoBehaviour, IAgentInteractable
{
   
    public bool CanInteract (Agent agent)
    {
        return (agent is Player);
    }

    public void Interact (Agent agent)

    {
        if (!CanInteract (agent))
            return;

        if (agent is Player)
        {
            Player player = (Player)agent;
            void OnTriggerEnter2D(Collider2D col)
            { 
                if(player.hp<10)
                {
                    player.ChangeHealthAmount(10);
                    Destroy(this);
;               }
            }
            
            
        }
    }
}


public class AmmoPickup : MonoBehaviour, IAgentInteractable
{
    bool isFromPlayer = true; //check if is picked up by player

    public bool CanInteract(Agent agent) => isFromPlayer;

    public void Interact(Agent agent)
    {
        if (!CanInteract(agent))
            return;

        if (agent is Player)
        {
            Player player = (Player)agent;
            void OnTriggerEnter2D(Collider2D col)
            {

                if(player.ammo<5)
                {
                    player.ChangeAmmoAmount(5); //change player ammo
                    Destroy(this);
                }
                    
                
            }

        }
    }
}
