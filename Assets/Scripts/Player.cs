using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum Direction
{
    East = 0, North = 1, West = 2, South = 3
}

public class Player : Agent
{
    public int pointsPerAmmo = 1;
    public int pointsPerHealthKit = 1;
    public Text healthText;
    public Text ammoText;
    public Text addingAmmo;
    public Text addingHealth;
    public LineRenderer lineRenderer;
    public Transform firePoint;
    public int hp = 10, ammo = 5;

    private Animator animator;
    private Vector2Int inputDir = new Vector2Int(0,0);
    private Direction faceDir = Direction.East;

    private bool inputChanged = false;
    /*
    private bool m_FacingRight = true;
    private bool m_FacingLeft = false;
    private bool m_FacingUp = false;
    private bool m_FacingDown = false;
    */

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

        // shooting projectile
        if (Input.GetButtonDown("Fire1") && ammo != 0)
        {
            StartCoroutine(Shoot());
        }

        Vector2Int curInputDir = new Vector2Int((int)Input.GetAxisRaw("Horizontal"), (int)Input.GetAxisRaw("Vertical"));
        inputChanged = (inputDir != curInputDir);
        inputDir = curInputDir;

        if (inputChanged)
        {
            faceDir = base.DirChange(inputDir, faceDir);

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90 * (int)faceDir));

            if (inputDir != Vector2Int.zero)
            {
                base.Move(inputDir);
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
            addingAmmo.text = "+ " + delta;
        else if (delta < 0)
            addingAmmo.text = "- " + delta;
    }

    public void ChangeHealthAmount(int delta)
    {
        hp += delta;
        healthText.text = "Health: " + hp;

        if(delta > 0)
            addingHealth.text = "+ " + delta;
        else if(delta < 0)
            addingHealth.text = "- " + delta;   
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

    private void GameOver()
    {
        if (hp <= 0)
        {

        }
    }
}
