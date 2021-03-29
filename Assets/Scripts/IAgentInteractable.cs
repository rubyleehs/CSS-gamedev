using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAgentInteractable
{  /// <summary>
   /// Interface for any interactable an Agent can interact with
   /// </summary>
    bool CanInteract(Agent agent);
    void Interact(Agent agent);
}
