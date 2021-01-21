using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

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
    private int Facing = 1;
    private int dirFacing;
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

        int horizontal = 0;
        int vertical = 0;

        vertical = (int) (Input.GetAxisRaw("Vertical"));
        horizontal = (int) (Input.GetAxisRaw("Horizontal"));

        if (vertical != 0)
            horizontal = 0;

        if (horizontal == 1)
            dirFacing = 1;
        else if (vertical == 1)
            dirFacing = 2;
        else if (horizontal == -1)
            dirFacing = 3;
        else if (vertical == -1)
            dirFacing = 4;

        if (Facing > dirFacing)
        {
            for (int x = 0; x != Facing - dirFacing; x++)
            {
                RevRotation();
            }
        }
        else if(Facing < dirFacing)
        {
            for (int x = 0; x != dirFacing - Facing ; x++)
            {
                Rotation();
            }
        }

        Facing = dirFacing;

        Vector2Int direction = new Vector2Int(horizontal, vertical);

        if(vertical != 0 || horizontal != 0) {
            base.Move(direction);
        }

        healthText.text = "Health: " + hp;
        ammoText.text = "Ammo: " + ammo;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Interactable")
        {
            IPlayerInteractable playerInteractable = collision.gameObject.GetComponent<IPlayerInteractable>();
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

        addingAmmo.text = "+ " + delta;
    }

    public void ChangeHealthAmount(int delta)
    {
        hp += delta;
        healthText.text = "Health: " + hp;

        addingHealth.text = "+ " + delta;
    }

    public void Damaged (int delta)
    {
        hp -= delta;

        healthText.text = "Health: " + hp;
        addingHealth.text = "- " + delta;
    }

    IEnumerator Shoot() 
    { 
        ammo--;

        ammoText.text = "Ammo: " + ammo;

        addingAmmo.text = "- 1";

        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);

        if (hitInfo)
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
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

    private void Rotation()
    {
        transform.Rotate(0f, 0f, 90f);
    }

    private void RevRotation()
    {
        transform.Rotate(0f, 0f, -90f);
    }

    /*
    private void HFlip()
    {
        if (m_FacingRight)
        {
            m_FacingRight = !m_FacingRight;
            m_FacingLeft = !m_FacingLeft;
        }
        else if (m_FacingLeft)
        {
            m_FacingLeft = !m_FacingLeft;
            m_FacingRight = !m_FacingRight;
        }

        m_FacingUp = false;
        m_FacingDown = false;

        transform.Rotate(0f, 0f, 180f);
    }

    private void VFlip()
    {
        if (m_FacingUp)
        {
            m_FacingUp = !m_FacingUp;
            m_FacingDown = !m_FacingDown;
        }
        else if (m_FacingDown)
        {
            m_FacingDown = !m_FacingDown;
            m_FacingUp = !m_FacingUp;
        }

        m_FacingRight = false;
        m_FacingLeft = false;

        transform.Rotate(0f, 0f, 180f);
    }

    private void rotateLeft()
    {
        if (m_FacingRight == true)
        {
            m_FacingRight = !m_FacingRight;
            m_FacingUp = true;
        }
        else if (m_FacingUp == true)
        {
            m_FacingUp = !m_FacingUp;
            m_FacingLeft = true;
        }
        else if (m_FacingLeft == true)
        {
            m_FacingLeft = !m_FacingLeft;
            m_FacingDown = true;
        }
        else if (m_FacingDown == true)
        {
            m_FacingDown = !m_FacingDown;
            m_FacingRight = true;
        }

        transform.Rotate(0f, 0f, 90f);
    }

    private void rotateRight()
    {
        if (m_FacingRight == true)
        {
            m_FacingRight = !m_FacingRight;
            m_FacingDown = true;
        }
        else if (m_FacingUp == true)
        {
            m_FacingUp = !m_FacingUp;
            m_FacingRight = true;
        }
        else if (m_FacingLeft == true)
        {
            m_FacingLeft = !m_FacingLeft;
            m_FacingUp = true;
        }
        else if (m_FacingDown == true)
        {
            m_FacingDown = !m_FacingDown;
            m_FacingLeft = true;
        }

        transform.Rotate(0f, 0f, -90f);
    }
    */
}
