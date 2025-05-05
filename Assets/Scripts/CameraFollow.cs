using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float limitRight, limitLeft, limitUp, limitDown;
    public Transform starCloud;

    private bool isIntro = true;

    public static CameraFollow Instance = null;

    void Awake()
    {
        if(!Instance) Instance = this;
        else Destroy(Instance);
    }

    public IEnumerator IntroMove()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(transform.position.x, starCloud.position.y, transform.position.z);
        float duration = 48f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        isIntro = false;
    }

    void FixedUpdate()
    {
        
        // if (isIntro || target == null) return;

        // Vector3 desiredPosition = target.position + offset;
        // desiredPosition.z = transform.position.z;
        // Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, limitLeft, limitRight);
        // smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, limitDown, limitUp);
        // transform.position = smoothedPosition;
        if(transform.position.y >= starCloud.position.y)return;
        transform.position += transform.up * smoothSpeed * Time.deltaTime; 
    }
}

