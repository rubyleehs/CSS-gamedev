﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : Agent
{
    public static Player instance;

    public Vector2 playerSpawnPosition = new Vector2(7, 2);

    public Text healthText;
    public Text ammoText;
    public Text healthDeltaInfo;
    public Text ammoDeltaInfo;

    public AudioSource[] sounds;
    public AudioSource moveSound;
    public AudioSource shootingSound;
    public AudioSource enemyDamagedSound;

    public GameObject statDeltaTextGO;

    public LineRenderer lineRenderer;
    public Transform firePoint;
    public int maxAmmo = 10;

    [HideInInspector]
    public int currentAmmo = 10;

    public new Transform camera;
    public int cameraSpeed = 8;

    [HideInInspector]
    public Animator animator;
    private Vector2Int prevMoveDir = new Vector2Int(0,0);
    private Direction faceDir = Direction.East;

    private float lastMoveTime;
    public float moveWaitTime = .3f;
    public float statChangeAnimationDuration = 2;

    private bool isDead = false;

    private void Awake()
    {
        // Singleton design pattern.
        if (instance != null)
            Destroy(this);
        else instance = this;
    }

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        sounds = GetComponents<AudioSource>();
        moveSound = sounds[0];
        shootingSound = sounds[1];
        enemyDamagedSound = sounds[2];

        ChangeHpAmount(0);
        ChangeAmmoAmount(0);
    }

    // Update is called once per frame.
    void Update()
    {
        Vector2Int curInputDir = Vector2Int.zero;
        // Checks if the current position of the player is more than 1 higher than that of the camera.
        if (gameObject.transform.position.y > camera.position.y + 1) {
            camera.position += Vector3.up * Time.deltaTime * cameraSpeed;
        }

        // Player shooting.
        if (Input.GetButtonDown("Fire1") && currentAmmo != 0)
        {
            StartCoroutine(Shoot());
        }

        // Player movement.
        curInputDir = new Vector2Int((int)Input.GetAxisRaw("Horizontal"), (int)Input.GetAxisRaw("Vertical"));
        Move(curInputDir);
        if (canMoveSound)
        {
            moveSound.Play();
            canMoveSound = false;
        }
    }


    /// <summary>
    /// Moves the <c>Player</c> in a given direction.
    /// </summary>
    /// <param name="direction"> Direction to move. </param>
    public override void Move(Vector2Int direction)
    {
        base.Move(direction);

        prevMoveDir = direction;
        if (direction != Vector2Int.zero)
            lastMoveTime = Time.time;
    }

    /// <summary>
    /// Checks if <c>Player</c> is able to move in a given direction.
    /// </summary>
    /// <param name="direction"> Direction to check. </param>
    /// <returns> If this is able to move in the given direction. </returns>
    protected override bool CanMove(Vector2Int direction)
    {
        if (isDead)
            return false;
        //Can hold and move slower or press fast move faster
        if(direction != prevMoveDir || Time.time - lastMoveTime >= moveWaitTime)
            if((gameObject.transform.position + (Vector3Int)prevMoveDir).y > (camera.position.y - 6))
                return base.CanMove(direction);
        return false;
    }

    /// <summary>
    /// Alters the <c>Player</c> current HP.
    /// </summary>
    /// <param name="delta"> Amount to change by. </param>
    public override void ChangeHpAmount(int delta)
    {
        base.ChangeHpAmount(delta);
        healthText.text = "Health: " + currentHp;

        string deltaString = "";

        if (delta > 0)
            deltaString = "+" + delta;
        else if (delta < 0)
        {
            deltaString = "" + delta;
            animator.SetTrigger("Damaged");
            StartCoroutine(MainCamera.instance.ShakeCamera(0.04f, 0.1f));
        }

        StartCoroutine(AnimateDeltaText(deltaString, healthDeltaInfo.transform.position, Vector3.up * 12,healthDeltaInfo.color, 2,healthDeltaInfo.transform.parent));
    }

    /// <summary>
    /// Initiates the stuff necessary for the Game Over screen.
    /// </summary>
    public override void Die()
    {
        // Overriden so player dont get destroyed
        Debug.Log("test");
        StartCoroutine(DyingAnimation());

        isDead = true;


        //Show die animation
        //wait for animation to end then show gameover screen
        
        return;
    }

    /// <summary>
    /// Called when the player's collider enters a trigger.
    /// Attempts to interact with any <c>IAgentInteractable</c> it collides with.
    /// </summary>
    /// <param name="other"> The collider of the object this collided with. </param>
    //prob move to agent
    private void OnTriggerEnter2D(Collider2D other)
    {

        IAgentInteractable playerInteractable = other.gameObject.GetComponent<IAgentInteractable>();
        if (playerInteractable != null)
        {
            playerInteractable.Interact(this);
        }
        
    }

    /// <summary>
    /// Alters the <c>Player</c> currernt ammo. 
    /// </summary>
    /// <param name="delta"></param>
    public void ChangeAmmoAmount(int delta)
    {
        currentAmmo += delta;
        currentAmmo = Mathf.Min(currentAmmo, maxAmmo);
        ammoText.text = "Ammo: " + currentAmmo;

        string deltaString = "";

        if (delta > 0)
            deltaString = "+" + delta;
        else if (delta < 0)
            deltaString = "" + delta;

        StartCoroutine(AnimateDeltaText(deltaString, ammoDeltaInfo.transform.position, Vector3.up * 12, ammoDeltaInfo.color, 2, ammoDeltaInfo.transform.parent));
    }

    /// <summary>
    /// 
    /// </summary>
    public override void ResetStats() {
        base.ResetStats();
        isDead = false;
        currentAmmo = maxAmmo;
        gameObject.transform.position = playerSpawnPosition;
    }

    /// <summary>
    /// Instantiates a new text object and animates its color and position.
    /// Currently used to anima changes in player stats.
    /// </summary>
    /// <param name="s">String the text should have.</param>
    /// <param name="startPosition">Starting position of the animation.</param>
    /// <param name="deltaPosition">Where the text object should move during the animation.</param>
    /// <param name="startColor">Starting text color.</param>
    /// <param name="duration">How long the animation should take.</param>
    /// <param name="parent">Parent Transform to temporarily hold the text object</param>
    IEnumerator AnimateDeltaText(string s, Vector3 startPosition, Vector3 deltaPosition, Color startColor, float duration, Transform parent)
    {
        Text text = Instantiate(statDeltaTextGO, startPosition, Quaternion.identity, parent).GetComponent<Text>();

        text.text = s;
        text.color = startColor;


        float startTime = Time.time;
        float progress = 0;
        Color endColor = startColor;
        endColor.a = 0;
        while (progress < 1)
        {
            progress = Mathf.SmoothStep(0, 1, (Time.time - startTime) / duration);
            text.color = Color.Lerp(startColor, endColor, progress);
            text.transform.position = Vector3.Lerp(startPosition, startPosition + deltaPosition, progress);

            yield return new WaitForEndOfFrame();
        }

        Destroy(text.gameObject);
    }

    /// <summary>
    /// decreases the ammo count and emits a raycast to hit or miss the zombie
    /// </summary>
    /// <returns>flashes the laser travel line</returns>
    IEnumerator Shoot() 
    {
        //Try to seperate logic and animation
        ChangeAmmoAmount(-1);

        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, Mathf.Infinity, blockingLayerMask);
        lineRenderer.SetPosition(0, firePoint.position);

        animator.SetTrigger("Shooting");
        shootingSound.Play();
        StartCoroutine(MainCamera.instance.ShakeCamera(0.02f,0.03f));

        if (hitInfo)
        {
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ChangeHpAmount(-1);
                enemyDamagedSound.Play();
            }
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100);
        }

        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.02f);

        lineRenderer.enabled = false;
    }

    IEnumerator DyingAnimation()
    {

        animator.SetTrigger("Dead");

        yield return new WaitForSeconds(1.5f);

    }
}
