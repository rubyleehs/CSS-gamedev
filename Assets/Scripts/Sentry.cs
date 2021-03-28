using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Sentry : Enemy
    {
        public Transform firePoint;
        public LineRenderer lineRenderer;

        public float attackRadius = 20f;
        public float loadTime = 3f;
        public float fireDuration = 1f;
        public float laserloadingWidth = 0.08f;
        public float laserWidth = 0.25f;

        public AudioSource loadingSFX;

        /// <summary>
        /// Checks if this can attack the target under current conditions.
        /// </summary>
        /// <param name="target"> The Agent to attack. </param>
        /// <returns> If this is able to attack the target under current conditions. </returns>
        public override bool CanAttack(Agent target)
        {
            // TODO: ensure that the player is within the shooting range of the sentry.
            // HINT: an attackRadius (float) is provided
            return (Vector2.Distance(target.transform.position, this.transform.position) < attackRadius);
        }

        /// <summary>
        /// Starts a new attack sequence against a given target.
        /// </summary>
        /// <param name="target"> Agent to attack </param>
        public override bool Attack(Agent target)
        {
            // TODO: call the function AttackAnim with...
            // HINT: remember during the Player class
            return true;
        }

        /// <summary>
        /// Overriden to instead intitate another action immediately.
        /// </summary>
        /// <param name="delta"> Amount to change by. </param>
        public override void ChangeHpAmount(int delta)
        {
            // TODO: most (possibly all) logic from the ChangeHpAmount is needed so determine if it should be 
            //       called here. and also remember the actionTimeRemaining at Enemy class.
        }

        /// <summary>
        /// Overriden to do nothing as the sentry is not able to move (on its own).
        /// </summary>
        /// <param name="direction"> Direction to move in. </param>
        protected override bool CanMove(Vector2Int direction)
        {
            return false;
        }

        /// <summary>
        /// Attack animation/sequence.
        /// </summary>
        public IEnumerator AttackAnim()
        {
            float laserStartTime = Time.time;
            animator.SetTrigger("Loading");
            loadingSFX.Play();

            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.widthMultiplier = laserloadingWidth;
            while (Time.time - laserStartTime < loadTime)
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(1, Vector3.right * attackRadius);
                yield return new WaitForSeconds(0.1f);
            }

            animator.SetTrigger("Shooting");
            attackSFX.Play();

            laserStartTime = Time.time;
            while (Time.time - laserStartTime < fireDuration)
            {
                lineRenderer.widthMultiplier = laserWidth;
                RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, transform.right, Mathf.Infinity, blockingLayerMask);

                if (hitInfo.transform)
                {
                    Agent agent = hitInfo.transform.GetComponent<Agent>();
                    if (agent != null)
                    {
                        agent.ChangeHpAmount(-attackDamage);
                    }
                    lineRenderer.SetPosition(1, Vector3.right * Vector3.Magnitude(firePoint.position - (Vector3)hitInfo.point));
                }
                else
                {
                    lineRenderer.SetPosition(1, Vector3.right * attackRadius);
                }
                yield return new WaitForSeconds(0.1f);
            }


            lineRenderer.enabled = false;
        }
    }