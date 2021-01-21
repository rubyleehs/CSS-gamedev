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
    public int hp = 10, ammo = 5;

    private Animator animator;

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
        if (Input.GetKeyDown("k") && ammo != 0)
        {
            animator = GetComponent<Animator>();

            ammo--;

            ammoText.text = "Ammo: " + ammo + " - 1";
        }

        int horizontal = (int) (Input.GetAxisRaw("Horizontal"));
        int vertical = (int) (Input.GetAxisRaw("Vertical"));

        if (vertical != 0)
            horizontal = 0;

        Vector2Int direction = new Vector2Int(horizontal, vertical);

        if(vertical != 0 || horizontal != 0) {
            base.Move(direction);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ammo")
        {
<<<<<<< HEAD
            ammo += pointsPerAmmo;
=======
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
        animator = GetComponent<Animator>();

        ammo--;

        ammoText.text = "Ammo: " + ammo;

        addingAmmo.text = "- 1";
>>>>>>> parent of 6be0e2f... Chaning some assets around.

            ammoText.text = "Ammo: " + ammo + "+ " + pointsPerAmmo;

            collision.gameObject.SetActive(false);
        }

<<<<<<< HEAD
        if (collision.tag == "HealthKit")
=======
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(.02f);

        lineRenderer.enabled = false;
    }

    private void GameOver()
    {
        if (hp <= 0)
>>>>>>> parent of 6be0e2f... Chaning some assets around.
        {
            hp += pointsPerHealthKit;

            healthText.text = "Health: " + hp + " + " + pointsPerHealthKit;

            collision.gameObject.SetActive(false);
        }

        if (collision.tag == "Hole")
            hp = 0;
    }

    private void ChangeAmmoAmount(int delta)
    {
        ammo += delta;
        ammoText.text = "Ammo: " + ammo;
    }

    private void Lost()
    {
        if (hp <= 0)
        {

        }
    }
}
