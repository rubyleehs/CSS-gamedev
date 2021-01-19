using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInteractable
{
    bool CanInteract(Agent agent);
    void Interact(Agent agent);
}
