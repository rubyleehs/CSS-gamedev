using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : Agent
{
    public static Player instance;

    public int pointsPerAmmo = 1;
    public int pointsPerHealthKit = 1;
    public Text healthText;
    public Text ammoText;
    public Text addingAmmo;
    public Text addingHealth;
    public LineRenderer lineRenderer;
    public Transform firePoint;
    public int maxAmmo = 5;

    [HideInInspector]
    public int currentAmmo = 5;

    // Camera control is given to the Player
    public new Transform camera;
    public int cameraSpeed = 8;

    private Animator animator;
    private Vector2Int prevMoveDir = new Vector2Int(0,0);
    private Direction faceDir = Direction.East;

    private float lastMoveTime;
    public float moveWaitTime = .3f;

    private void Awake()
    {
        // this is called a singleton design pattern
        if (instance != null)
            Destroy(this);
        else instance = this;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        ChangeHpAmount(0);
        ChangeAmmoAmount(0);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the current position of the player is more than 1 higher than that of the camera
        if (gameObject.transform.position.y > camera.position.y + 1) {
            // Moves the camera by deltatime
            camera.position += Vector3.up * Time.deltaTime * cameraSpeed;
        }

        // shooting projectile
        if (Input.GetButtonDown("Fire1") && currentAmmo != 0)
        {
            StartCoroutine(Shoot());
        }

        Vector2Int curInputDir = new Vector2Int((int)Input.GetAxisRaw("Horizontal"), (int)Input.GetAxisRaw("Vertical"));
        Move(curInputDir);
    }

    public override void Move(Vector2Int direction)
    {
        base.Move(direction);

        prevMoveDir = direction;
        if (direction != Vector2Int.zero)
            lastMoveTime = Time.time;
    }

    protected override bool CanMove(Vector2Int direction)
    {
        //Can hold and move slower or press fast move faster
        if(direction != prevMoveDir || Time.time - lastMoveTime >= moveWaitTime)
            if((gameObject.transform.position + (Vector3Int)prevMoveDir).y > (camera.position.y - 6))
                return base.CanMove(direction);
        return false;
    }

    public override void ChangeHpAmount(int delta)
    {
        base.ChangeHpAmount(delta);

        healthText.text = "Health: " + currentHp;

        if (delta > 0)
            addingHealth.text = "+" + delta;
        else if (delta < 0)
        {
            addingHealth.text = "" + delta;
            animator.SetTrigger("Damaged");
        }

        StartCoroutine(WaitUI());
    }

    public override void Die()
    {
        //override so player dont get destroyed
        return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Interactable")
        {
            IAgentInteractable playerInteractable = collision.gameObject.GetComponent<IAgentInteractable>();
            if (playerInteractable != null)
            {
                playerInteractable.Interact(this);
            }
        }
    }

    public void ChangeAmmoAmount(int delta)
    {
        currentAmmo += delta;
        ammoText.text = "Ammo: " + currentAmmo;

        if (delta > 0)
            addingAmmo.text = "+" + delta;
        else if (delta < 0)
            addingAmmo.text = "" + delta;

        StartCoroutine(WaitUI());
    }

    public override void ResetStats() {
        base.ResetStats();
        currentAmmo = maxAmmo;
        gameObject.transform.position = new Vector3(7, 2, 0f);
    }

    IEnumerator WaitUI()
    {
        //????? what is this for?
        yield return new WaitForSeconds(2);
        addingHealth.text = "";
        addingAmmo.text = "";
    }

    IEnumerator Shoot() 
    {
        //Try to seperate logic and animation
        ChangeAmmoAmount(-1);

        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
        lineRenderer.SetPosition(0, firePoint.position);

        animator.SetTrigger("Shooting");

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
