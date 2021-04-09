using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public static GameCamera instance;
    public Vector3 startPos = new Vector3(10, 5, -10f);
    private IEnumerator camShakeIEnum;

    public float playerTresholdFromCenter = -2;
    public float baseMoveCreepSpeed = 1;
    public float moveLerpSpeed = 8;


    private Vector3 originalPos;

    void Awake()
    {
        // Singleton Design Pattern.
        if (instance == null)
            instance = this;       
        else
           Destroy(this);        
    }

    public void ResetStats()
    {
        transform.position = startPos;
    }

    private void Update()
    {
        // Smoothly speed up the camera movement if player is moving fast / current position is high up on the screen.
        if (transform.position.y < Player.instance.transform.position.y + playerTresholdFromCenter)
        {
            // We do not want a flat increase - what if player somehow move faster than that?
            // Ideally, the further the camera is from the position we want it to be, the faster it should move.
            // A simple way to do so is just move the camera by a % of the difference between the target position and current position.

            if (transform.position.y < Player.instance.transform.position.y + playerTresholdFromCenter)
                transform.position = new Vector3(transform.position.x,
                    Mathf.Lerp(transform.position.y, Player.instance.transform.position.y + playerTresholdFromCenter, Time.deltaTime * moveLerpSpeed),
                    transform.position.z);
        }


        // Make the camera creep upwards, speed dependant on current difficulty
        transform.position += Vector3.up * baseMoveCreepSpeed * GameManager.instance.difficultyLevel * Time.deltaTime;
    }

    /// <summary>
    /// Shakes the camera.
    /// </summary>
    /// <param name="duration"> Duration in seconds to shake the camera. </param>
    /// <param name="magnitude"> Magnitude of the shake in local space. </param>
    public IEnumerator ShakeCamera(float duration, float magnitude = 0.7f)
    {
        // TODO: This function actually have a small bug, can you find it?
        // HINT: What if this is called many times in quick sucession?

        originalPos = transform.localPosition;
        while (duration > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * magnitude;

            duration -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.localPosition = originalPos;
    }
}

