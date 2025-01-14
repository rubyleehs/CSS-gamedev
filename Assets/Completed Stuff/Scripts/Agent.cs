﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Completed
{
    //Anythign that can move or have Ai
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
        protected virtual void Start()
        {
            ResetStats();
        }

        /// <summary>
        /// Resets the <c>Agent</c> stats so it can be reused.
        /// </summary>
        public virtual void ResetStats()
        {
            currentHp = maxHp;
        }

        /// <summary>
        /// Move the <c>Agent</c> in given direction.
        /// </summary>
        /// <param name="direction"> Direction in move in. </param>
        public virtual bool Move(Vector2Int direction)
        {
            lock (moveLock)
            {
                if (direction == Vector2Int.zero)
                return false;
            
                transform.position += new Vector3Int(direction.x, direction.y, 0);

                if (moveSFX != null)
                    moveSFX.Play();

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
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, direction.magnitude, blockingLayerMask);

                if (hits.Length > 0)
                {
                    for (int x = 0; x < hits.Length; x++)
                    {
                        if (hits[x].transform != this.transform)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Makes this <c>Agent</c> face a given direction.
        /// </summary>
        /// <param name="direction"> Direction to face. </param>
        public void Face(Vector2Int direction)
        {
            if (direction == Vector2Int.zero)
                return;
            transform.rotation = Quaternion.Euler(Vector3.forward * Vector2.SignedAngle(Vector2.right, direction));
        }

        /// <summary>
        /// Alters this <c>Agent</c> <c>currentHp</c>, capped to <c>maxHp</c>.
        /// Calls <c>Die()</c> if <c>currentHp < 0</c>
        /// </summary>
        /// <param name="delta"> Amount to change by. </param>
        public virtual void ChangeHpAmount(int delta)
        {
            currentHp += delta;
            currentHp = Mathf.Min(currentHp, maxHp);
            if (delta < 0 && damagedSFX != null)
            {
                damagedSFX.Play();
            }
            if (currentHp <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Method called when this <c>Agent</c> dies
        /// </summary>
        public virtual void Die()
        {
            Destroy(this.gameObject);
        }

        /// <summary>
        /// Called when the player's collider enters a trigger.
        /// Attempts to interact with any <c>IAgentInteractable</c> it collides with.
        /// </summary>
        /// <param name="other"> The collider of the object this collided with. </param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            IAgentInteractable agentInteractable = other.GetComponent<IAgentInteractable>();
            if (agentInteractable != null)
            {
                if(agentInteractable.CanInteract(this))
                    agentInteractable.Interact(this);
            }
        }
    }
}
