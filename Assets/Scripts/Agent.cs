using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Anything that can interact with the world should be an Agent.
public abstract class Agent : MonoBehaviour
{
    private static Object moveLock = new Object();

    [HideInInspector]
    public int currentHp = 10;
    public int maxHp = 10;

    public AudioSource moveSFX;
    public AudioSource attackSFX;
    public AudioSource damagedSFX;

    public LayerMask blockingLayerMask;


    // Start() is called before the first frame update.
    protected virtual void Start()
    {
        // Setup stuff. 
        // TODO: Reset the stats of the Agent by calling ResetStats()
    }

    /// <summary>
    /// Resets the <c>Agent</c> stats so it can be reused.
    /// </summary>
    public virtual void ResetStats()
    {
        // In this game, the only atribute that all Agents share that needs resetting is hp.
        // TODO: So lets reset that by making currentHp be equal to the maxHp.
    }

    /// <summary>
    /// Move the <c>Agent</c> in given direction.
    /// </summary>
    /// <param name="direction"> Direction in move in. </param>
    /// <returns> If <c>Agent</c> was sucessful in moving in given direction </returns>
    public virtual bool Move(Vector2Int direction)
    {
        lock (moveLock)
        {
            // TODO: Immediately return false if direction given is stationary/zero.
            // HINT: Can create a new zero vector with: new Vector2Int(0,0) or use Vector2Int.zero

            // TODO: Face the direction we are moving in by calling Face(direction)

            // TODO: Move the transform to the new position. 
            // TODO: If moveSFX is given, play it.

            return true;
        }        
    }

    /// <summary>
    /// Checks if <c>Agent</c> is able to move in a given direction in current conditions.
    /// </summary>
    /// <param name="direction"> Direciton to move in. </param>
    /// <returns> If <c>Agent</c> is able to move in given direction in currenct conditions. </returns>
    protected virtual bool CanMove(Vector2Int direction)
    {
        lock (moveLock)
        {
            // We want to check if we can move in a particular direction.
            // If there is an obstacle in our way, this should return false. Otherwise, true.
            // An easy way to do so is to litterally check if there is an obstacle by firing a Ray and see if it collides with anything.

            // TODO: Uncomment out the following line below.
            // RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, direction.magnitude, blockingLayerMask);

            // TODO: Check if the ray has hit something other than itself. If so, return false.
            // HINT: RaycastHit2D has a field called 'transform' that contains a reference to the transform the ray hit.

            return true;
        }            
    }

    /// <summary>
    /// Makes this <c>Agent</c> face a given direction.
    /// </summary>
    /// <param name="direction"> Direction to face. </param>
    public void Face(Vector2Int direction)
    {
        // TODO: Return if direction is zero.
        // TODO: Set the rotation of this transform so the right of the transform is facing the given direction.
        // HINT: Unity uses Quaternions to represent rotations. Quarternion.Euler() function can convert a Vector3 to a Quartenion.
        // HINT: Vector2.SignedAngle() returns the angular difference between two given vectors.
        // HINT: The axis of rotation for our case would be the z axis, aka the axis perpendicular your screen.

    }

    /// <summary>
    /// Alters this <c>Agent</c> <c>currentHp</c>, capped to <c>maxHp</c>.
    /// Calls <c>Die()</c> if <c>currentHp < 0</c>
    /// </summary>
    /// <param name="delta"> Amount to change by. </param>
    public virtual void ChangeHpAmount(int delta)
    {
        // TODO: Change currentHp by delta. Make sure it is capped by maxHp.

        // TODO: If the agent takes damaged and damagedSFX is given, play it.

        // TODO: Call Die() if the agent ends up dying from the change.
    }

    /// <summary>
    /// Method called when this <c>Agent</c> dies
    /// </summary>
    public virtual void Die()
    {
        // TODO: Destroy the game object this script is currently on.
    }

    /// <summary>
    /// Called when the player's collider enters a trigger.
    /// Attempts to interact with any <c>IAgentInteractable</c> it collides with.
    /// </summary>
    /// <param name="other"> The collider of the object this collided with. </param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // TODO: Check if the thing this has collided with has a component that implements IAgentInteractable. If so, if it is able to interact with it, do so.
        // HINT: thing.GetComponent<TheTypeYouAreLookingFor>() will return a component that thing has, or null if thing does not have said component.
    }
}

