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

    void FixedUpdate()
    {
        if (!GameManager.Instance.isGameStarted) return;
        if(transform.position.y >= starCloud.position.y)return;
        transform.position += transform.up * smoothSpeed * Time.deltaTime; 
    }
}

