using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : Agent
{
    public static Player instance;

    public Vector2 startingPosition = new Vector2(7, 2);

    public Text healthText;
    public Text ammoText;
    public Text addingAmmo;
    public Text addingHealth;

    public LineRenderer lineRenderer;
    public Transform firePoint;
    public int maxAmmo = 5;

    [HideInInspector]
    public int currentAmmo = 5;

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
        ChangeHpAmount(0);
        ChangeAmmoAmount(0);
    }

    // Update is called once per frame.
    void Update()
    {
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
        Vector2Int curInputDir = new Vector2Int((int)Input.GetAxisRaw("Horizontal"), (int)Input.GetAxisRaw("Vertical"));
        Move(curInputDir);
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
        StartCoroutine(AnimateHealthChange(delta));
    }

    /// <summary>
    /// Initiates the stuff necessary for the Game Over screen.
    /// </summary>
    public override void Die()
    {
        // Overriden so player dont get destroyed

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
        if(other.tag == "Interactable")
        {
            IAgentInteractable playerInteractable = other.gameObject.GetComponent<IAgentInteractable>();
            if (playerInteractable != null)
            {
                playerInteractable.Interact(this);
            }
        }
    }

    /// <summary>
    /// Alters the <c>Player</c> currernt ammo. 
    /// </summary>
    /// <param name="delta"></param>
    public void ChangeAmmoAmount(int delta)
    {
        currentAmmo += delta;
        StartCoroutine(AnimateAmmoChange(delta));
    }

    /// <summary>
    /// 
    /// </summary>
    public override void ResetStats() {
        base.ResetStats();
        isDead = false;
        currentAmmo = maxAmmo;
        gameObject.transform.position = startingPosition;
    }

    /// <summary>
    /// Health animation above the healthText
    /// </summary>
    /// <param name="delta"></param>
    /// <returns>flashes the health change ("+1" or "-1")</returns>
    IEnumerator AnimateHealthChange(int delta)
    {
        healthText.text = "Health: " + currentHp;

        if (delta > 0)
            addingHealth.text = "+" + delta;
        else if (delta < 0)
        {
            addingHealth.text = "" + delta;
            animator.SetTrigger("Damaged");
            StartCoroutine(MainCamera.instance.ShakeCamera(0.04f, 0.1f));
        }
        yield return new WaitForSeconds(statChangeAnimationDuration);
        addingHealth.text = "";
    }

    /// <summary>
    /// Ammo animation above the ammoText
    /// </summary>
    /// <param name="delta"></param>
    /// <returns>flashes the ammo change ("+1" or "-1")</returns>
    IEnumerator AnimateAmmoChange(int delta)
    {
        ammoText.text = "Ammo: " + currentAmmo;

        if (delta > 0)
            addingAmmo.text = "+" + delta;
        else if (delta < 0)
            addingAmmo.text = "" + delta;
        yield return new WaitForSeconds(statChangeAnimationDuration);
        addingAmmo.text = "";
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
        StartCoroutine(MainCamera.instance.ShakeCamera(0.02f,0.03f));

        if (hitInfo)
        {
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
                enemy.ChangeHpAmount(1);
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
}
