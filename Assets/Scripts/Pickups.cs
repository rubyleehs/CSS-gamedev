using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour, IPlayerInteractable {
    bool isStillDangerous = true;

    public bool CanInteract(Agent agent) {
        return isStillDangerous;
    }

    public void Interact(Agent agent) {
        if (!CanInteract(agent))
            return;

        if (agent is Player) {
            Player player = (Player)agent;
            player.ChangeAmmoAmount(10);
            //change player hp
        }
    }
}

