using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : Enemy
{
    public Transform firePoint;
    public LineRenderer lineRenderer;

    public float attackRadius = 15f;
    public float loadTime = 3f;
    public float fireDuration = 1f;

    /// <summary>
    /// if the sentry is facing directly vertical or horizontal from the sentry it will allow damages to the player during the sentry shooting phase.
    /// </summary>
    /// <param name="target"></param>
    /// <returns>a boolean to determine if damage should be applied to the player</returns>
    public override bool CanAttack (Agent target)
    {
        return (Vector2.Distance (target.transform.position, this.transform.position) < attackRadius);
    }

    /// <summary>
    /// this is for the attacking sequence to begin.
    /// </summary>
    /// <param name="target"></param>
    public override void Attack (Agent target)
    {
        StartCoroutine (AttackAnim ());
    }

    /// <summary>
    /// This is for
    /// </summary>
    /// <param name="delta"></param>
    public override void ChangeHpAmount (int delta)
    {
        actionTimeRemaining = 0;
    }

    /// <summary>
    /// since the sentry is stationary then this function is just being overided to do nothing.
    /// </summary>
    /// <param name="direction"></param>
    public override void Move (Vector2Int direction)
    {
        //Prevent Moving
        return;
    }

    /// <summary>
    /// This is for the sentry attack animation
    /// </summary>
    /// <param name="target"></param>
    public IEnumerator AttackAnim ()
    {
        animator.SetTrigger ("Loading");
        yield return new WaitForSeconds (loadTime);
        animator.SetTrigger("Shooting");

        float laserStartTime = Time.time;
        while (Time.time - laserStartTime < fireDuration)
        {
            LayerMask mask = LayerMask.GetMask("BlockingLayer");
            RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, transform.right, Mathf.Infinity, ~mask);//add Contact filter if want go though walls
            lineRenderer.SetPosition(0, Vector3.zero);
           
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
                lineRenderer.SetPosition(1, Vector3.right * 100);
            }

            lineRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    

        lineRenderer.enabled = false;
    }
}