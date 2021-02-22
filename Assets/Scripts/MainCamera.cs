using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour
{
    private Transform camTransform;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    public IEnumerator ShakeCamera(float duration, float magnitude = 0.7f)
    {
        while (duration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * magnitude;

            duration -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        camTransform.localPosition = originalPos;
    }
}
