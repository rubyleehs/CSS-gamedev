using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Completed
{
    public class MainCamera : MonoBehaviour
    {
        public static MainCamera instance;
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

        private void Update()
        {
            // Smoothly speed up the camera movement if player is moving fast / current position is high up on the screen.
            if (transform.position.y < Player.instance.transform.position.y + playerTresholdFromCenter)
                transform.position = new Vector3(transform.position.x,
                    Mathf.Lerp(transform.position.y, Player.instance.transform.position.y + playerTresholdFromCenter, Time.deltaTime * moveLerpSpeed),
                    transform.position.z);

            transform.position += Vector3.up * baseMoveCreepSpeed * GameManager.instance.difficultyLevel * Time.deltaTime;
        }

        /// <summary>
        /// Shakes the camera. There is a bug this may cause, can you find it?
        /// </summary>
        /// <param name="duration"> Duration in seconds to shake the camera. </param>
        /// <param name="magnitude"> Magnitude of the shake in local space. </param>
        public IEnumerator ShakeCamera(float duration, float magnitude = 0.7f)
        {
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
}
