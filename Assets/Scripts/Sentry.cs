using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : Enemy
{
    public Transform firePoint;
    public LineRenderer lineRenderer;

    public float attackRadius = 10f;
    public float loadTime = 3f;
    public float fireDuration = 1f;

    public AudioSource loadingSound;
    public AudioSource shootingSound;

    private void Start()
    {
        loadingSound = sounds[0];
        shootingSound = sounds[1];
    }
    /// <summary>
    /// Checks if this can attack the target under current conditions.
    /// </summary>
    /// <param name="target"> The Agent to attack. </param>
    /// <returns> If this is able to attack the target under current conditions. </returns>
    public override bool CanAttack (Agent target)
    {
        return (Vector2.Distance (target.transform.position, this.transform.position) < attackRadius);
    }

    /// <summary>
    /// Starts a new attack sequence against a given target.
    /// </summary>
    /// <param name="target"> Agent to attack </param>
    public override void Attack (Agent target)
    {
        StartCoroutine (AttackAnim ());
    }

    /// <summary>
    /// Overriden to instead intitate another action immediately.
    /// </summary>
    /// <param name="delta"> Amount to change by. </param>
    public override void ChangeHpAmount (int delta)
    {
        actionTimeRemaining = 0;
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
    public IEnumerator AttackAnim ()
    {
        animator.SetTrigger ("Loading");
        loadingSound.Play();
        yield return new WaitForSeconds (loadTime);
        animator.SetTrigger("Shooting");
        shootingSound.Play();

        float laserStartTime = Time.time;
        while (Time.time - laserStartTime < fireDuration)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, transform.right, Mathf.Infinity, blockingLayerMask);//add Contact filter if want go though walls
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