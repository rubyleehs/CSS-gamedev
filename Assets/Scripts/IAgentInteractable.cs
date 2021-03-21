using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for any interactable an Agent can interact with
/// </summary>
public interface IAgentInteractable
{
    bool CanInteract(Agent agent);
    void Interact(Agent agent);
}
