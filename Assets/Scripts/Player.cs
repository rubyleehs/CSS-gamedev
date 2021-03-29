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
    public int playerDamage = 15;

    [HideInInspector]
    public int currentAmmo = 10;

    [HideInInspector]
    public Animator animator;
    private Vector2Int prevMoveDir = new Vector2Int(0, 0);

    private float lastMoveTime;
    private Vector2Int lastMoveDir;
    public float moveWaitTime = .3f;
    public float statChangeAnimationDuration = 2;

    public AudioSource healSFX;
    public AudioSource reloadSFX;

    public float killzoneHeightFromCam = -8;

    [HideInInspector]
    public bool isDead = false;

    // Awake() is called before Start(), while the script instance is being loaded.
    // Normally used to initalize values.
    private void Awake()
    {
        // Singleton design pattern.
        if (instance != null)
            Destroy(this);
        else instance = this;

        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Rests the stats of the player 
    /// </summary>
    public override void ResetStats()
    {
        // TODO: call base.ResetStats() to the stuff from Agent will also execute.

        // TODO: set isDead to false, max out currentAmmo and set the player position to playerSpawnPosition.

        // Call the methods below with no change so that UI and stuff will properly reflect any changes made above.
        ChangeHpAmount(0);
        ChangeAmmoAmount(0);
    }

    // Update is called once per frame.
    void Update()
    {
        // TODO: If player isDead, return immediately.

        // Player Attack.
        // TODO: Make the player Shoot() if the left mouse button is pressed.
        // HINT: Unity has an input system that allows you to cofigure key mappings. The LMB is mapped to "Fire1"
        // HINT: Input.GetButtonDown("TheButtonName")

        // Player Movement.
        // TODO: Get the direction the player want to move in.
        // HINT: Input.GetAxisRaw("Horizontal")

        // TODO: Process the direction so its a Vector2Int Up/Down/Left/Right.
        // HINT: While keyboards buttons only have 2 distinct states - pressed(1) or not pressed(0), joysticks/other controllers may have more/be continuous.
        // HINT: if (curentInputDirection.y > 0.5f) moveDirection = Vector2Int.up;

        // TODO: Check if it CanMove() in the processed direction. If so, pass it to Move().

        // Kills the player if the player is below the camera by killZoneHeightFromCam
        if (transform.position.y < MainCamera.instance.transform.position.y + killzoneHeightFromCam)
            Die();        
    }


    /// <summary>
    /// Moves the <c>Player</c> in a given direction.
    /// </summary>
    /// <param name="direction"> Direction to move. </param>
    public override bool Move(Vector2Int direction)
    {
        if (base.Move(direction))
        {
            // TODO: The player have sucessfully moved, set lastMoveTime to the current time with Time.time

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
        // TODO: Return false if the player isDead or if lastMoveTime is less than moveWaitTime.

        return base.CanMove(direction);
    }

    public void Shoot()
    {
        // TODO: Check if currentAmmo is less than or equal 0. If so, play noAmmoSFX if it exist before returning.
        // TODO: Otherwise use up an ammo by calling ChangeAmmoAmount()

        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, Mathf.Infinity, blockingLayerMask);
        if (hitInfo)
        {
            // TODO: Check if the ray have hit an Enemy, if so deal damage to it equivalent to playerDamage with enemy.ChangeHpAmount()
            // HINT: Get the transform of the hit, then GetComponent<Enemy>() which will either return an Enemy or null
            //Enemy enemy = hitInfo.transform.GetComponent<Enemy>();


            StartCoroutine(ShootAnim(hitInfo.point));
        }
        else
            StartCoroutine(ShootAnim(firePoint.position + firePoint.right * 100));
    }

    /// <summary>
    /// Alters the <c>Player</c> current HP.
    /// </summary>
    /// <param name="delta"> Amount to change by. </param>
    public override void ChangeHpAmount(int delta)
    {
        base.ChangeHpAmount(delta);

        //TODO: Update healthText.text with the current hp.

        string deltaString = "";
        if (delta > 0)
        {
            deltaString = "+" + delta;
            healSFX.Play();
        }
        else if (delta < 0)
        {
            deltaString = "" + delta;
            animator.SetTrigger("Damaged");
            damagedSFX.Play();
            StartCoroutine(MainCamera.instance.ShakeCamera(0.04f, 0.1f));
        }

        StartCoroutine(DeltaTextAnim(deltaString, healthDeltaInfo.transform.position, Vector3.up * 12, healthDeltaInfo.color, 2, healthDeltaInfo.transform.parent));
    }

    /// <summary>
    /// Alters the <c>Player</c> currernt ammo. 
    /// </summary>
    /// <param name="delta">Amount to change by. </param>
    public void ChangeAmmoAmount(int delta)
    {
        // TODO: Change currentAmmo by delta amount, Make sure it is capped by maxAmmo.
        // HINT: You have done this for health in the Agent class!

        // Update ammoText.text with currentAmmo
        ammoText.text = "Ammo: " + currentAmmo;

        string deltaString = "";

        if (delta > 0)
        {
            deltaString = "+" + delta;
            reloadSFX.Play();
        }
        else if (delta < 0)
            deltaString = "" + delta;

        StartCoroutine(DeltaTextAnim(deltaString, ammoDeltaInfo.transform.position, Vector3.up * 12, ammoDeltaInfo.color, 2, ammoDeltaInfo.transform.parent));
    }

    /// <summary>
    /// Initiates the stuff necessary for the Game Over screen.
    /// </summary>
    public override void Die()
    {
        // We do NOT call base.Die() coz we dont want the player to get destoyed after dying.
        
        // TODO: Set isDead to true.
        // TODO: Start the Coroutine for DeathAnim()
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
    IEnumerator DeltaTextAnim(string s, Vector3 startPosition, Vector3 deltaPosition, Color startColor, float duration, Transform parent)
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
    /// <param name="hitInfo"> Information regarding the thing it hit</param>
    IEnumerator ShootAnim(Vector3 endPosition)
    {
        if (attackSFX != null)
            attackSFX.Play();

        StartCoroutine(MainCamera.instance.ShakeCamera(0.02f, 0.03f));

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, endPosition);

        animator.SetTrigger("Shooting");
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.02f);

        lineRenderer.enabled = false;
    }

    IEnumerator DieAnim()
    {
        animator.SetTrigger("Dead");

        // Wait for a bit before showing gameover screen
        yield return new WaitForSeconds(1.5f);

        // TODO: Uncomment out the following line
        // GameManager.instance.InitGameOver();
    }
}