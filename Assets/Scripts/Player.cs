using System.Collections;
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

    public GameObject statDeltaTextGO;

    public LineRenderer lineRenderer;
    public Transform firePoint;
    public int maxAmmo = 10;

    [HideInInspector]
    public int currentAmmo = 10;

    [HideInInspector]
    public Animator animator;
    private Vector2Int prevMoveDir = new Vector2Int(0,0);
    private Direction faceDir = Direction.East;

    private float lastMoveTime;
    private Vector2Int lastMoveDir;
    public float moveWaitTime = .3f;
    public float statChangeAnimationDuration = 2;

    public AudioSource healSFX;
    public AudioSource reloadSFX;

    public float killzoneHeightFromCam = -8;

    [HideInInspector]
    public bool isDead = false;

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

        
    }

    // Update is called once per frame.
    void Update()
    {
        // Player shooting.
        if (Input.GetButtonDown("Fire1") && currentAmmo != 0)
        {
            StartCoroutine(Shoot());
        }

        // Player movement.
        Vector2Int curInputDir = new Vector2Int((int)Input.GetAxisRaw("Horizontal"), (int)Input.GetAxisRaw("Vertical"));
        Move(curInputDir);

        if(transform.position.y < MainCamera.instance.transform.position.y + killzoneHeightFromCam)
        {
            Die();
        }
    }


    /// <summary>
    /// Moves the <c>Player</c> in a given direction.
    /// </summary>
    /// <param name="direction"> Direction to move. </param>
    public override bool Move(Vector2Int direction)
    {
        
        //if (direction == Vector2Int.zero)
        //    return false;
        Vector2Int moveDir = Vector2Int.zero;

        if (direction.y > 0)
            moveDir = Vector2Int.up;
        else if (direction.y < 0)
            moveDir = Vector2Int.down;
        if (direction.x > 0)
            moveDir = Vector2Int.right;
        else if (direction.x < 0)
            moveDir = Vector2Int.left;

        //if(moveDir.sqrMagnitude > 1)
        //{
        //    moveDir += lastMoveDir;
        //}

        if (base.Move(moveDir))
        {
            lastMoveTime = Time.time;
            lastMoveDir = moveDir;
            return true;
        }
        return false;
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
        if (Time.time - lastMoveTime >= moveWaitTime)
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

        if (delta > 0) {
            deltaString = "+" + delta;            
            healSFX.Play(); // This can also be on the health pickup itself, would depend on your sound design
        }
           
        else if (delta < 0)
        {
            deltaString = "" + delta;
            animator.SetTrigger("Damaged");
            damagedSFX.Play();
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
        StartCoroutine(DyingAnimation());

        isDead = true;
        //Show die animation
        //wait for animation to end then show gameover screen
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
        {
            deltaString = "+" + delta;
            reloadSFX.Play(); // This can also be on the ammo pickup itself, would depend on your sound design
        }            
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

        ChangeHpAmount(0);
        ChangeAmmoAmount(0);
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
        attackSFX.Play();
        StartCoroutine(MainCamera.instance.ShakeCamera(0.02f,0.03f));

        if (hitInfo)
        {
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ChangeHpAmount(-1);
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
