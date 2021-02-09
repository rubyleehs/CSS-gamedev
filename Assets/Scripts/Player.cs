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
    public int hp = 10, ammo = 5;

    // Camera control is given to the Player
    public new Transform camera;
    public int cameraSpeed = 8;

    private Animator animator;
    private Vector2Int inputDir = new Vector2Int(0,0);
    private Direction faceDir = Direction.East;

    private bool inputChanged = false;
    private float timer;
    private float waitTime = .1f;

    private void Awake()
    {
        // this is called a singleton design pattern
        if (instance != null)
            Destroy(this);
        else instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Pause Game
        if (Input.GetKeyDown("p"))
            GameManager.instance.TogglePause();

        // Check if the current position of the player is more than 1 higher than that of the camera
        if (gameObject.transform.position.y > camera.position.y + 1) {

            // Moves the camera by deltatime
            camera.position += Vector3.up * Time.deltaTime * cameraSpeed;
        }

        // shooting projectile
        if (Input.GetButtonDown("Fire1") && ammo != 0)
        {
            StartCoroutine(Shoot());
        }

        Vector2Int curInputDir = new Vector2Int((int)Input.GetAxisRaw("Horizontal"), (int)Input.GetAxisRaw("Vertical"));
        inputChanged = (inputDir != curInputDir);
        inputDir = curInputDir;

        timer += Time.deltaTime;

        if (inputChanged)
        {
            if (inputDir != Vector2Int.zero)
            {
                // Only allows movement if player does not go below camera
                if ((gameObject.transform.position + (Vector3Int)inputDir).y > (camera.position.y - 6) && timer > waitTime) {
                    Move(inputDir);
                    timer = 0;
                }
            }
        }

        healthText.text = "Health: " + hp;
        ammoText.text = "Ammo: " + ammo;
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
        ammo += delta;
        ammoText.text = "Ammo: " + ammo;

        if (delta > 0)
            addingAmmo.text = "+" + delta;
        else if (delta < 0)
            addingAmmo.text = "" + delta;

        StartCoroutine(WaitUI());
    }

    public void ChangeHealthAmount(int delta)
    {
        hp += delta;
        healthText.text = "Health: " + hp;

        if (delta > 0)
            addingHealth.text = "+" + delta;
        else if (delta < 0)
        {
            addingHealth.text = "" + delta;
            animator.SetTrigger("Damaged");
        }

        StartCoroutine(WaitUI());
    }

    public void ResetPlayer() {
        hp = 10;
        ammo = 200;
        gameObject.transform.position = new Vector3(7, 2, 0f);
    }

    IEnumerator WaitUI()
    {
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
                enemy.TakeDamage(1);
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
