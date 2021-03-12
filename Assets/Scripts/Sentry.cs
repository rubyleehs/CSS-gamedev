using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
public class Sentry : Agent
{
    public int playerDamage = -2;
    public Transform firePoint;
    public LineRenderer lineRenderer;

    private Transform target;
    private Animator animator;
    private float timer;
    private float waitTime = 2.0f;
    private bool loaded = false;

    protected Direction currentFaceDir = Direction.East;

    // Start is called ONCE before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        //target = GameObject.FindGameObjectWithTag("Player").transform;
        target = Player.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2Int delta = new Vector2Int((int)(target.position.x - transform.position.x), (int)(target.position.y - transform.position.y));

        Vector2Int faceDirection = Vector2Int.zero;

        if (delta.x != 0 && CanMove(Vector2Int.right * (delta.x > 0 ? 1 : -1)))
        {
            faceDirection.x = (int)target.position.x > (int)transform.position.x ? 1 : -1;
        }
        else if (delta.y != 0 && CanMove(Vector2Int.up * (delta.y > 0 ? 1 : -1)))
        {
            faceDirection.y = (int)target.position.y > (int)transform.position.y ? 1 : -1;
        }
        currentFaceDir = GetFaceDirection(faceDirection, currentFaceDir);
        Face(currentFaceDir);

        timer += Time.deltaTime;

        if (timer > waitTime && !loaded)
        {
            animator.SetTrigger("Loading");

            loaded = true;

            timer = 0;
        }

        if (loaded)
            Shoot();

        loaded = false;
    }
    **/

public class Sentry : Enemy
{
    public Transform firePoint;
    public LineRenderer lineRenderer;

    public float attackRadius = 15f;
    public float loadTime = 3f;
    public float fireDuration = 1f;

    public void Start ()
    {
        attackDamage = -1;
    }

    public override bool CanAttack (Agent target)
    {
        return (Vector2.Distance (target.transform.position, this.transform.position) < attackRadius);
    }

    public override void Attack (Agent target)
    {
        StartCoroutine (AttackAnim ());
    }

    public override void ChangeHpAmount (int delta)
    {
        actionTimeRemaining = 0;
    }

    public override void Move (Vector2Int direction)
    {
        //Prevent Moving
        return;
    }

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
                    agent.ChangeHpAmount(attackDamage);
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