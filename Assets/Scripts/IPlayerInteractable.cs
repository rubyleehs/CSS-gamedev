using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Anything an Agent can interact with
public interface IAgentInteractable
{
    bool CanInteract(Agent agent);
    void Interact(Agent agent);
}
