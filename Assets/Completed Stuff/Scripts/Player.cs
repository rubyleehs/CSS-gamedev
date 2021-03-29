using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Completed
{
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

        private float lastMoveTime;
        public float moveWaitTime = .3f;
        public float statChangeAnimationDuration = 2;

        public AudioSource healSFX;
        public AudioSource noAmmoSFX;
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

            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Rests the stats of the player 
        /// </summary>
        public override void ResetStats()
        {
            base.ResetStats();
            isDead = false;
            currentAmmo = maxAmmo;
            gameObject.transform.position = playerSpawnPosition;

            ChangeHpAmount(0);
            ChangeAmmoAmount(0);
        }

        // Update is called once per frame.
        void Update()
        {
            if (isDead)
                return;

            // Player shooting.
            if (Input.GetButtonDown("Fire1"))
                Shoot();
            

            // Player movement.
            Vector2 curInputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector2Int moveDir = Vector2Int.zero;

            if (curInputDir.y > 0.5f)
                moveDir = Vector2Int.up;
            else if (curInputDir.y < -0.5f)
                moveDir = Vector2Int.down;

            if (curInputDir.x > 0.5f)
                moveDir = Vector2Int.right;
            else if (curInputDir.x < -0.5f)
                moveDir = Vector2Int.left;

            Face(moveDir);

            if (CanMove(moveDir))
                Move(moveDir);

            if (transform.position.y < MainCamera.instance.transform.position.y + killzoneHeightFromCam)
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
            if (base.Move(direction))
            {
                lastMoveTime = Time.time;
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
            if (isDead || Time.time - lastMoveTime < moveWaitTime)
                return false;


            return base.CanMove(direction);
        }

        public void Shoot()
        {
            if (currentAmmo <= 0)
            {
                if (noAmmoSFX != null)
                    noAmmoSFX.Play();
                return;
            }

            ChangeAmmoAmount(-1);
            RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, Mathf.Infinity, blockingLayerMask);
            if (hitInfo)
            {
                Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.ChangeHpAmount(-playerDamage);
                }
                StartCoroutine(ShootAnim(hitInfo.point));
            }
            else
            {
                StartCoroutine(ShootAnim(firePoint.position + firePoint.right * 100));
            }
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
            {
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

            StartCoroutine(DeltaTextAnim(deltaString, healthDeltaInfo.transform.position, Vector3.up * 12, healthDeltaInfo.color, 2, healthDeltaInfo.transform.parent));
        }

        /// <summary>
        /// Makes the player die, prepare for game over screen.
        /// </summary>
        public override void Die()
        {
            isDead = true;
            StartCoroutine(DieAnim());
        }

        /// <summary>
        /// Alters the <c>Player</c> currernt ammo. 
        /// </summary>
        /// <param name="delta">Amount to change by. </param>
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

            StartCoroutine(DeltaTextAnim(deltaString, ammoDeltaInfo.transform.position, Vector3.up * 12, ammoDeltaInfo.color, 2, ammoDeltaInfo.transform.parent));
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
            GameManager.instance.InitGameOver();
        }
    }
}
