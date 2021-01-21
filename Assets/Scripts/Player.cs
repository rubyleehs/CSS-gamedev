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
    private bool m_FacingRight = true;
    private bool m_FacingUp = false;
    private bool m_FacingLeft = false;
    private bool m_FacingDown = false;

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

        if ((m_FacingRight == true && horizontal == -1) || (m_FacingLeft == true && horizontal == 1))
            HFlip();
        else if ((m_FacingDown == true && vertical == 1) || m_FacingUp == true && vertical == -1)
            VFlip();
        else if ((m_FacingRight == true && vertical == 1) || (m_FacingUp == true && horizontal == -1) || (m_FacingLeft == true && vertical == -1) || (m_FacingDown == true && horizontal == 1))
            rotateLeft();
        else if ((m_FacingUp == true && horizontal == 1) || (m_FacingRight == true && vertical == -1) || (m_FacingDown == true && horizontal == -1) || (m_FacingLeft == true && vertical == 1))
            rotateRight();

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
<<<<<<< HEAD
<<<<<<< HEAD
            ammo += pointsPerAmmo;
=======
=======
>>>>>>> parent of 5720c8b... Revert "Additions in Health Pickup"
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
<<<<<<< HEAD
        animator = GetComponent<Animator>();
=======
>>>>>>> parent of 5720c8b... Revert "Additions in Health Pickup"

        ammo--;

        ammoText.text = "Ammo: " + ammo;

        addingAmmo.text = "- 1";
<<<<<<< HEAD
>>>>>>> parent of 6be0e2f... Chaning some assets around.
=======
>>>>>>> parent of 5720c8b... Revert "Additions in Health Pickup"

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

<<<<<<< HEAD
<<<<<<< HEAD
        if (collision.tag == "HealthKit")
=======
=======
>>>>>>> parent of 5720c8b... Revert "Additions in Health Pickup"
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(.02f);

<<<<<<< HEAD
        lineRenderer.enabled = false;
=======
        lineRenderer.SetVertexCount(0);
>>>>>>> parent of 5720c8b... Revert "Additions in Health Pickup"
    }

    private void GameOver()
    {
        if (hp <= 0)
<<<<<<< HEAD
>>>>>>> parent of 6be0e2f... Chaning some assets around.
=======
>>>>>>> parent of 5720c8b... Revert "Additions in Health Pickup"
        {

        }
    }

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
}
